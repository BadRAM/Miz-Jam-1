using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct CollageTile : IComparable<CollageTile>
{
    public float brightness;
    public Sprite sprite;

    public CollageTile(float brightness, Sprite sprite)
    {
        this.brightness = brightness;
        this.sprite = sprite;
    }

    public int CompareTo(CollageTile other)
    {
        return brightness.CompareTo(other.brightness);
    }
    
    public int CompareTo(float other)
    {
        return brightness.CompareTo(other);
    }

    int IComparable<CollageTile>.CompareTo(CollageTile next)
    {
        return CompareTo(next);
    }
}
public class Dither : MonoBehaviour
{
    [SerializeField] private Texture2D Debug_texture;
    [SerializeField] private Texture2D One_Ass;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float render_time;
    [SerializeField] private List<CollageTile> Tiles;
    [SerializeField] private GameObject spritePrefab;
    private int x=0;
    private int y=0;
    private float error_distribute;
    private Color[] pixels;
    private float[] pixel_error;
    private bool _dithering;
    private Vector3 cursorPos;
    
    
    [ContextMenu("Bake Tiles")]
    void BakeTiles()
    {
        Debug.Log("Baking Tiles...");
    
        Tiles = new List<CollageTile>();

        string spriteSheet = AssetDatabase.GetAssetPath(One_Ass);
        Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheet)
            .OfType<Sprite>().ToArray();

        foreach (Sprite s in sprites)
        {
            Debug.Log("Baked a tile... Delicious!");
            int count = 0;
            float brightness = 0;
            Rect rect = s.textureRect;
            foreach (Color p in s.texture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height, 0))
            {
                brightness += p.grayscale;
                count++;
            }
            brightness = brightness / count;
            
            Tiles.Add(new CollageTile(brightness, s));
        }
        
        Debug.Log("Sorting cookies");
        
        Tiles.Sort();
        
        Debug.Log("DING!");
    }

    // Start is called before the first frame update
    void Start()
    {
        //Array.Sort(Tiles);
        start_dither(Debug_texture, 0, 0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (_dithering)
        {
            dither_iterate();
        }
    }

    int threshold(float pixel_value)
    {
        //return the number of the tile that closest matches the pixel value
        return BinarySearch(Tiles.ToArray(), pixel_value);
    }

    void start_dither(Texture2D input_image, int posx, int posy, int mipLevel)
    {
        
        //Code currently assumes that input_image is already WIDTH and HEIGHT pixels large.
        // If not, the pixels[x+y*width] in the for loop will need to be changed

        _dithering = true;

        pixels = input_image.GetPixels(posx, posy, width, height, mipLevel);

        pixel_error = new float[pixels.Length];
        x=0;
        y=0;
        
    }


    void dither_iterate()
    {
        int tile_number;

        pixel_error[x + y * width] += pixels[x + y * width].grayscale;

        tile_number = threshold(pixels[x + y * width].grayscale);

        error_distribute = pixels[x + y * width].grayscale - Tiles[tile_number].brightness;

        if (x < width - 1)
            pixel_error[x + 1 + y * width] += 7 / 16 * error_distribute;

        if (y < height - 1 && x < width - 1)
            pixel_error[x + 1 + (y + 1) * width] += 1 / 16 * error_distribute;

        if (y < height - 1)
            pixel_error[x + (y + 1) * width] += 5 / 16 * error_distribute;

        if (x > 0 && y < height - 1)
            pixel_error[x - 1 + (y + 1) * width] += 3 / 16 * error_distribute;

        x++;
        if (x >= width)
        {
            y++;
            x = 0;

            if (y >= height)
            {
                //nothing more to dither, stop dithering
                _dithering = false;
            }
        }

        GameObject spriteObject = Instantiate(spritePrefab, transform);
        spriteObject.transform.localPosition = Vector3.right * x + Vector3.down * y;
        spriteObject.GetComponent<SpriteRenderer>().sprite = Tiles[tile_number].sprite;
    }
    
    public static int BinarySearch(CollageTile[] a, float item)
    {
        int first = 0;
        int last = a.Length - 1;
        int mid = 0;
        do
        {
            mid = first + (last - first) / 2;
            if (item > a[mid].brightness)
                first = mid + 1;
            else
                last = mid - 1;
            if (a[mid].brightness == item)
                return mid;
        } while (first <= last);
        return mid;
    }
}