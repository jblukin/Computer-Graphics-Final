using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{

    [SerializeField, Range(128, 1024)] private int size = 256;

    [SerializeField, Range(20, 40)] private float scale = 30;

    [SerializeField] private float offsetX = 0, offsetY = 0;

    private static int Size;

    private static float Scale;

    private static float OffsetX, OffsetY;

    // Start is called before the first frame update
    void Start()
    {

        if (offsetX == 0 && offsetY == 0)
        {

            offsetX = Random.Range(-1000000f, 1000000f);
            offsetY = Random.Range(-1000000f, 1000000f);

        }

        Size = size;

        Scale = scale;

        OffsetX = offsetX; 
        
        OffsetY = offsetY;

        //Test Line
        //GetComponent<MeshRenderer>().material.mainTexture = GenerateNormalMap();

    }

    public static Texture2D GeneratePerlin()
    {

        Texture2D tex = new(Size, Size);

        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {

                tex.SetPixel(x, y, Sample(x, y));

            }
        }

        tex.Apply();

        return tex;

    }

    public static Texture2D GenerateNormalMap()
    {

        Texture2D tex = GeneratePerlin();

        Texture2D texOut = new(tex.width, tex.height);

        for (int x = 0; x < tex.height; x++)
        {
            for (int y = 0; y < tex.width; y++)
            {

                float xLeft = tex.GetPixel(x - 1, y).grayscale;
                float xRight = tex.GetPixel(x + 1, y).grayscale;
                float yUp = tex.GetPixel(x, y - 1).grayscale;
                float yDown = tex.GetPixel(x, y + 1).grayscale;
                float xDelta = ((xLeft - xRight) + 1) * 0.5f;
                float yDelta = ((yUp - yDown) + 1) * 0.5f;

                texOut.SetPixel(x, y, new Color(xDelta, yDelta, 1.0f, 1.0f));

            }

        }

        texOut.Apply();

        return texOut;

    }

    public static Texture2D GenerateNormalMap(Texture2D tex)
    {

        Texture2D texOut = new(tex.width, tex.height);

        for (int x = 0; x < tex.height; x++)
        {
            for (int y = 0; y < tex.width; y++)
            {

                float xLeft = tex.GetPixel(x - 1, y).grayscale;
                float xRight = tex.GetPixel(x + 1, y).grayscale;
                float yUp = tex.GetPixel(x, y - 1).grayscale;
                float yDown = tex.GetPixel(x, y + 1).grayscale;
                float xDelta = ((xLeft - xRight) + 1) * 0.5f;
                float yDelta = ((yUp - yDown) + 1) * 0.5f;

                texOut.SetPixel(x, y, new Color(xDelta, yDelta, 1.0f, 1.0f));

            }

        }

        texOut.Apply();

        return texOut;

    }

    private static Color Sample(float x, float y)
    {

        float perlinX = OffsetX + ( ( x / Size ) * Scale );
        float perlinY = OffsetY + ( ( y / Size ) * Scale );

        float sample = Mathf.PerlinNoise(perlinX, perlinY);

        return new(sample, sample, sample);

    }

}
