#version 330 core
out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D tex;

vec3 white = vec3(0.829, 0.837, 0.853);
vec3 black = vec3(0.147, 0.155, 0.171);

void main()
{
    vec4 c = texture(tex, TexCoords);
    float pixel = c.r;
    
    FragColor = vec4(
    	      mix(black.r, white.r, pixel),
	      mix(black.g, white.g, pixel),
	      mix(black.b, white.b, pixel),
	      1.0
    );
}