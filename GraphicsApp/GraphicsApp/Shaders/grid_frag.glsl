#version 330 core

in vec3 fragPos;

uniform vec3 cameraPos;

out vec4 FragColor;

void main()
{
    float gridSpacing = 1.0; // The spacing of the grid lines
    float lineThickness = 0.015; // The thickness of the grid lines

    // Calculate the grid lines
    vec2 grid = abs(fract(fragPos.xz / gridSpacing - 0.5) - 0.5) / fwidth(fragPos.xz / gridSpacing);
    float line = min(grid.x, grid.y);

    // Default grid color is gray
    vec3 gridColor = vec3(0.3);

    // Calculate the distance from the X and Z axes
    float distToXAxis = abs(fragPos.z);
    float distToZAxis = abs(fragPos.x);

    // Calculate the line thickness based on the screen space derivatives
    float thickness = lineThickness / fwidth(fragPos.x + fragPos.z);

    // Calculate the alpha value for the grid lines
    float alpha = 1.0 - smoothstep(0.0, thickness, line);

    // Define the threshold for the axis lines
    float axisThreshold = lineThickness * 1.25; // Make axis lines slightly thicker

    // Colors for the X and Z axes
    vec3 xAxisColor = vec3(0.5, 0.0, 0.0); // Red for X axis
    vec3 zAxisColor = vec3(0.0, 0.5, 0.0); // Green for Z axis

    // Calculate the contribution of the axis lines
    float xAxisLine = 1.0 - smoothstep(0.0, axisThreshold, distToXAxis);
    float zAxisLine = 1.0 - smoothstep(0.0, axisThreshold, distToZAxis);

    // Blend the colors based on the distance to the axes
    vec3 blendedColor = mix(gridColor, xAxisColor, xAxisLine);
    blendedColor = mix(blendedColor, zAxisColor, zAxisLine);

    // Ensure the axes lines are visible and maintain the same thickness
    alpha = max(alpha, xAxisLine);
    alpha = max(alpha, zAxisLine);

    // Hide the ugly grid ends in fog
    vec3 fogColor = vec3(0.15);
    float fogStart = 10.0;
    float fogEnd = 50.0;

    float distance = length(fragPos);
    float fogFactor = clamp((fogEnd - distance) / (fogEnd - fogStart), 0.0, 1.0);
    vec3 finalColor = mix(fogColor, blendedColor, fogFactor);

    FragColor = vec4(finalColor, alpha);
}