using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    public int width = 256;
    public int height = 256;

    public int scale=10;

    public float offsetX = 100f;
    public float offsetY = 100f;

    void Update()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture();

        if(Input.GetKeyDown(KeyCode.R))
        {
            offsetX = Random.Range(0, 999999);
            offsetY = Random.Range(0, 999999);
        }
    }
      
    Texture2D GenerateTexture()
    {
        //너비와 높이를 이용해 새로운 텍스처2D생성
        Texture2D texture = new Texture2D(width, height);

        //Perlin Noise텍스처 생성

        for (int x = 0; x < width; x++)
        {
            for (int y= 0; y < height;y++)
            {
                Color color = CalculateColor(x,y);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return texture;
    }

    Color CalculateColor(int x, int y)
    {
        float xCoord = (float)x / width * scale+offsetX;
        float yCoord = (float)y / height * scale+offsetY;
        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);
    }
}
