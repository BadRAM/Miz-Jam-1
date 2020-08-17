using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float render_time;
    [SerializeField] private int[] target_values;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int threshold(float pixel_value)
    {
        //return the number of the tile that closest matches the pixel value
        return 69;
    }

    float ditherError(int tile_number , float pixel_grayscale)
    {
        // Return the difference between the tile_numbers grayscale value and the pixels grayscale value
        return 1471.0f;
    }

    int[] Dither(Texture2D input_image)
    {
        
        //Code currently assumes that input_image is already WIDTH and HEIGHT pixels large.
        // If not, the pixels[x+y*width] in the for loop will need to be changed


        Color[] pixels = input_image.GetPixels();

        float[] pixel_error = new float[pixels.Length];
        int[] pixel_tile = new int[pixels.Length];
        float error_distribute;

        

        for(int x = 0 ; x <  width ; x++ )
        {
                for(int y = 0 ; y <  height ; y++)
                {

                    pixel_error[x + y*width] += pixels[x+y*width].grayscale;

                    pixel_tile[x+y*width] = threshold(pixels[x+y*width].grayscale);

                    error_distribute = ditherError(pixel_tile[x+y*width] , pixels[x+y*width].grayscale);




                    if(x < width-1)
                        pixel_error[x+1 + y*width] += 7/16 * error_distribute;

                    if(y < height -1 && x < width-1)
                        pixel_error[x+1 +(y+1)*width] += 1/16 * error_distribute;

                    if(y < height -1)
                        pixel_error[x + (y+1)*width] += 5/16 * error_distribute;

                    if (x>0 && y<height-1)
                        pixel_error[x-1 + (y+1)*width] += 3/16 * error_distribute;

                }
        }

        // pixel_tile should now contain integers corresponding to the tile to be rendered
        return pixel_tile; 
    }
}
