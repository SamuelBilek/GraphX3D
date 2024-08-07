﻿#version 330 core

layout(location = 0) in vec3 aPosition;

layout(location = 1) in vec2 aTexCoord;

layout(location = 2) in vec3 aNormal;

out vec2 TexCoord;

out vec3 Normal;

out vec3 FragPos;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    TexCoord = aTexCoord;

    // note that we read the multiplication from right to left
    gl_Position =  vec4(aPosition, 1.0) * model * view * projection;
    FragPos = vec3(model * vec4(aPosition, 1.0));
    Normal = aNormal * mat3(transpose(inverse(model)));
}