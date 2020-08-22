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
    [SerializeField] private bool colour;
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
    private int _midZoomX;
    private int _midZoomY;
    private Transform _spritesParent;
    private Transform _lastSpritesParent;
    public CensorBox censor;
    public Texture2D lastinput_image;
    public int lastposx;
    public int lastposy;
    public int lastmipLevel;

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
        if (zoomLevel == 2)
        {
            return;
        }
        
        zoomLevel++;

        int posx = 0;
        int posy = 0;
        
        if (zoomLevel == 1)
        {
            posx = (int) (pos.x * (width));
            posy = (int) (pos.y * (height));
            _midZoomX = posx;
            _midZoomY = posy;
        }
        else //zoomlevel must be 2
        {
            posx = (int) (pos.x * (width)) + _midZoomX*2;
            posy = (int) (pos.y * (height)) + _midZoomY*2;
        }

        //Debug.Log( "posx: " + posx + " posy: " + posy);
        
        start_dither(currentImage, posx, posy, ZoomToMip());
        
        _lastSpritesParent.localScale = Vector3.one * 2;
        _lastSpritesParent.localPosition += Vector3.right * width * -pos.x + Vector3.up * height * (1f-pos.y) + Vector3.forward;
    }

    public void ZoomOut()
    {
        if (zoomLevel == 0)
        {
            return;
        }
        
        // catches an edge case if zooming out while zooming in.
        if (_lastSpritesParent != null)
        {
            Destroy(_lastSpritesParent.gameObject);
            _lastSpritesParent = null;
        }
        
        if (zoomLevel == 2)
        {
            zoomLevel = 1;
            start_dither(currentImage, _midZoomX, _midZoomY, 1);
        }
        else
        {
            zoomLevel = 0;
            start_dither(currentImage, 0, 0, 2);
        }
        
        Destroy(_lastSpritesParent.gameObject);
        _lastSpritesParent = null;
    }

    public void kill_child()
    {
        Destroy(_lastSpritesParent.gameObject);
        _lastSpritesParent = null;
    }

    public void start_dither(Texture2D input_image, int posx, int posy, int mipLevel)
    {
        lastinput_image = input_image;
        lastposx = posx;
        lastposy = posy;
        lastmipLevel = mipLevel;
                            Debug.Log(lastposy);
//        foreach (Transform child in transform) {
//            Destroy(child.gameObject);
//        }


        //Code currently assumes that input_image is already WIDTH and HEIGHT pixels large.
        // If not, the pixels[x+y*width] in the for loop will need to be changed

        _dithering = true;
        _ditherCount = 0;
        _ditherStartTime = Time.time;

        _lastSpritesParent = _spritesParent;
        
        _spritesParent = new GameObject().transform;
        _spritesParent.transform.parent = transform;
        _spritesParent.localPosition = Vector3.zero;

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

        GameObject spriteObject = Instantiate(spritePrefab, _spritesParent);
        spriteObject.transform.localPosition = Vector3.right * x + Vector3.down * y;
        spriteObject.GetComponent<SpriteRenderer>().sprite = Tiles[tile_number].sprite;
        
        //colour

        if (colour)
        {
            spriteObject.GetComponent<SpriteRenderer>().color = pixel / pixel.maxColorComponent;
        }
        
        if (censor.rectanglesToCensor.Count > 0)
        {
            for (int i = 0; i < censor.rectanglesToCensor.Count; i++)
            {
                if (censor.rectanglesToCensor[i].Contains(new Vector2((lastposx + x)*Mathf.Pow(2,lastmipLevel) , (lastposy +30-y)*Mathf.Pow(2,lastmipLevel))))
                {

                    spriteObject.GetComponent<SpriteRenderer>().sprite = Tiles[0].sprite;
                }
            }
        }
        x++;
        if (x >= width)
        {
            y++;
            x = 0;

            if (y >= height)
            {
                //nothing more to dither, stop dithering
                _dithering = false;

                if (_lastSpritesParent != null)
                {
                    Destroy(_lastSpritesParent.gameObject);
                    _lastSpritesParent = null;
                }
            }
        }
    }

    public int GetZoom()
    {
        return zoomLevel;
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