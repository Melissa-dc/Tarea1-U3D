#version 330 core

out vec4 FragColor;

void main()
{
    // Color base lila claro
    vec3 colorBase = vec3(0.8, 0.6, 1.0);

    // Simulación de sombra (basado en la altura del fragmento)
    float sombra = clamp(0.5 + gl_FragCoord.y / 600.0, 0.3, 1.0);

    // Aplicar efecto de sombra
    FragColor = vec4(colorBase * sombra, 1.0);
}
