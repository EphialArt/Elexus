#version 330 core

in vec3 vertexColor;
out vec4 FragColor;

uniform vec4 highlightColor;

void main()
{
    FragColor = vec4(vertexColor, 1.0);
    if (highlightColor.a > 0.0)
    {
        FragColor = mix(FragColor, highlightColor, 0.5);
    }
}
