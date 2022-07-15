#version 330 core
out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D tex1;
uniform sampler2D tex2;

const vec3 weights = vec3(0.299, 0.587, 0.114);
const uint quantization = 16u;

const float lut[8] = float[](
      1.0, 0.0, 0.5, 1.0, // black -> white
      0.0, 1.0, 0.5, 0.0 // white -> black
);

// r = screen value (i.e. the level the panel has)
// g = desired value (i.e. the exact pixel value)
// b = cycle count
// a = table

void main()
{
  vec4 data = texture(tex2, TexCoords);

  uint screenPixel = uint(data.r * 255.0);
  uint desiredPixel = uint(data.g * 255.0);
  uint cycle = uint(data.b * 255.0);
  uint table = uint(data.a * 255.0);

  if(cycle > 0u) {
    float lut_value = lut[(table * 4u) + (cycle/5u)];
    cycle--;
 
    if(cycle > 1u) {
      screenPixel = uint(lut_value * 255);
    } else {
      screenPixel = desiredPixel;
    }
  } else {
    uint newPixel = uint(dot(texture(tex1, TexCoords).rgb, weights) * 255.0); // Weighted grayscale
    uint factor = (256u / quantization);
    newPixel = (newPixel / factor) * factor; // Quantization
	
    if(abs(newPixel - desiredPixel) > (128u / quantization)) {
      cycle = 20u;
      if(newPixel >= desiredPixel) {
	table = 0u;
      } else {
	table = 1u;
      }

      desiredPixel = newPixel;
    }
  }

  FragColor = vec4(screenPixel / 255.0, desiredPixel / 255.0, cycle / 255.0, table / 255.0);
}
