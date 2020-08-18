using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Dither : MonoBehaviour
{
    [SerializeField] private Texture2D currentImage;
    [SerializeField] private Texture2D One_Ass;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float minRenderInterval;
    [SerializeField] private int maxRendersPerFrame;
    [SerializeField] private List<CollageTile> Tiles;
    [SerializeField] private GameObject spritePrefab;
    private int x=0;
    private int y=0;
    private float error_distribute;
    private Color[] pixels;
    private float[] pixel_error;
    private bool _dithering;
    private int _ditherCount;
    private float _ditherStartTime;
    private Vector3 cursorPos;
    private int zoomLevel;
    private int _lastZoomX;
    private int _lastZoomY;
    private GameObject _lastZoomSprites;


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
        start_dither(currentImage, 0, 0, 2);

        _lastZoomSprites = new GameObject();
        _lastZoomSprites.transform.parent = transform;
    }

    // Update is called once per frame
    void Update()
    {
        int renderCount = 0;
        while (_dithering && _ditherCount * minRenderInterval < Time.time - _ditherStartTime && renderCount < maxRendersPerFrame)
        {
            dither_iterate();
            _ditherCount++;
            renderCount++;
        }
    }

    int threshold(float pixel_value)
    {
        //return the number of the tile that closest matches the pixel value
        return BinarySearch(Tiles.ToArray(), pixel_value);
    }

    public int ZoomResDivisor()
    {
        switch (zoomLevel)
        {
            case 0:
                return 1;
            case 1:
                return 2;
            case 2:
                return 4;
        }
        return 1;
    }

    public int ZoomToMip()
    {
        switch (zoomLevel)
        {
            case 0:
                return 2;
            case 1:
                return 1;
            case 2:
                return 0;
        }
        return 2;
    }

    public void ZoomIn(Vector3 pos)
    {
        zoomLevel++;

        int posx = (int)(pos.x * (currentImage.width / Mathf.Pow(2, zoomLevel) - 56));
        int posy = (int)(pos.y * (currentImage.height / Mathf.Pow(2, zoomLevel) - 30));
        
        Debug.Log( "posx: " + posx + " posy: " + posy);
        
        start_dither(currentImage, posx, posy, ZoomToMip());
    }

    public void ZoomOut()
    {
        zoomLevel = 0;
        start_dither(currentImage, 0, 0, 2);
    }

    void start_dither(Texture2D input_image, int posx, int posy, int mipLevel)
    {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        
        
        //Code currently assumes that input_image is already WIDTH and HEIGHT pixels large.
        // If not, the pixels[x+y*width] in the for loop will need to be changed

        _dithering = true;
        _ditherCount = 0;
        _ditherStartTime = Time.time;

        pixels = input_image.GetPixels(posx, posy, width, height, mipLevel);

        pixel_error = new float[pixels.Length];
        x=0;
        y=0;
        
    }


    void dither_iterate()
    {
        Color pixel = pixels[x + (height - (y + 1)) * width];

        pixel_error[x + y * width] += pixel.grayscale;

        int tile_number = threshold(pixel.grayscale);

        error_distribute = pixel.grayscale - Tiles[tile_number].brightness;

        if (x < width - 1)
            pixel_error[x + 1 + y * width] += 7 / 16 * error_distribute;

        if (y < height - 1 && x < width - 1)
            pixel_error[x + 1 + (y + 1) * width] += 1 / 16 * error_distribute;

        if (y < height - 1)
            pixel_error[x + (y + 1) * width] += 5 / 16 * error_distribute;

        if (x > 0 && y < height - 1)
            pixel_error[x - 1 + (y + 1) * width] += 3 / 16 * error_distribute;

        GameObject spriteObject = Instantiate(spritePrefab, transform);
        spriteObject.transform.localPosition = Vector3.right * x + Vector3.down * y;
        spriteObject.GetComponent<SpriteRenderer>().sprite = Tiles[tile_number].sprite;
        
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

    int IComparable<CollageTile>.CompareTo(CollageTile next)
    {
        return CompareTo(next);
    }
}