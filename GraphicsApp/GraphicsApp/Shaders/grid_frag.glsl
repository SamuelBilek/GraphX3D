#version 330 core

in vec3 fragPos;

uniform vec3 cameraPos;

out vec4 FragColor;

void main()
{
    float gridSpacing = 1.0; // The spacing of the grid lines
    float lineThickness = 0.01; // The thickness of the grid lines

    // Calculate the grid lines
    vec2 grid = abs(fract(fragPos.xz / gridSpacing - 0.5) - 0.5) / fwidth(fragPos.xz / gridSpacing);
    float line = min(grid.x, grid.y);

    // Set the alpha value based on the distance to the nearest grid line
    float alpha = 1.0 - smoothstep(1.0 - lineThickness, 1.0, line);

    // Set the final fragment color with transparency
    vec3 gridColor = vec3(0.2);

    // Hide the ugly grid ends in fog
    vec3 fogColor = vec3(0.15);
    float fogStart = 10.0;
    float fogEnd = 50.0;

    float distance = length(fragPos);
    float fogFactor = clamp((fogEnd - distance) / (fogEnd - fogStart), 0.0, 1.0);
    vec3 finalColor = mix(fogColor, gridColor, fogFactor);

    FragColor = vec4(finalColor, alpha);
}