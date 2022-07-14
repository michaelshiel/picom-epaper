#version 330 core
out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D tex1;
uniform sampler2D tex2;

const vec3 weights = vec3(0.299, 0.587, 0.114);
const float quantization_levels = 16.0;
const float speed = 0.5;

const float lut[8] = float[](
      1.0, -1.0, 1.0, 1.0, // black -> white
      1.0, 1.0, -1.0, -1.0 // white -> black
);

// r = screen value (i.e. the level the panel has)
// g = desired value (i.e. the exact pixel value)
// b = cycle count
// a = table

void main()
{
    vec4 data = texture(tex2, TexCoords);

    float screenPixel = data.r;
    float desiredPixel = data.g;
    int cycle = int(data.b);
    int table = int(data.a);
        
    if(cycle != 0) {
    	  float lut_value = lut[(table * 4) + int(cycle / 10)];
	  //screenPixel += (speed * lut_value);

	  float e = desiredPixel - screenPixel;
	  screenPixel += e * 0.8;
	  screenPixel = lut_value;
	  
	  cycle--;
    } else {
        float newPixel = dot(texture(tex1, TexCoords).rgb, weights); // Weighted grayscale
    	newPixel = floor(newPixel * quantization_levels) / (quantization_levels - 1.0); // Quantization
	
        if(newPixel != desiredPixel) {
	    cycle = 40;
	    if(newPixel > desiredPixel) {
	        table = 0;
	    } else {
	        table = 1;
	    }

	    desiredPixel = newPixel;
	}
    }

    FragColor = vec4(screenPixel, desiredPixel, cycle, table);
}