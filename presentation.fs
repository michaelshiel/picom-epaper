#version 330 core
out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D tex;

vec3 white = vec3(0.729, 0.737, 0.753);
vec3 black = vec3(0.247, 0.255, 0.271);

void main()
{
    vec4 c = texture2D(tex, TexCoords);
    float pixel = c.r;
    
    FragColor = vec4(
    	      pixel, //mix(black.r, white.r, pixel),
	      pixel, //mix(black.g, white.g, pixel),
	      pixel, //mix(black.b, white.b, pixel),
	      1.0
    );
}