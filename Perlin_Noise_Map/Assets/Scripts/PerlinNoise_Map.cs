using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise_Map : MonoBehaviour
{
    Dictionary<int, GameObject> tileset;
    Dictionary<int, GameObject> tile_groups;

    public GameObject plain;
    public GameObject forest;
    public GameObject stone;
    public GameObject water;

    public int map_width = 16;
    public int map_height=9;

    List<List<int>> noise_grid = new List<List<int>>();
    List<List<GameObject>> tile_grid = new List<List<GameObject>>();

    //Ȯ����, 4~20 ����
    public float magnification = 7.0f;

    public int x_offset = 0;
    public int y_offset = 0;

    void Start()
    {
        CreateTileset();
        CreateTileGroups();
        GenerateMap();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            x_offset = Random.Range(0, 999999);
            y_offset = Random.Range(0, 999999);
            magnification = Random.Range(4f, 20f);
            for(int i=0;i<transform.childCount;i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            CreateTileset();
            CreateTileGroups();
            GenerateMap();
        }
    }
    //�����յ�� Ÿ�ϼ� ��ųʸ� ����
    void CreateTileset()
    {
        tileset = new Dictionary<int, GameObject>();
        tileset.Add(0, water); //0 = water
        tileset.Add(1, plain); //1 = plain
        tileset.Add(2, forest); //2 = forest
        tileset.Add(3, stone); //3 = stone
    }
    //���� Ÿ�� ������ �ڽ����� �� Ÿ�ϱ׷���� ����
    void CreateTileGroups()
    {
        tile_groups = new Dictionary<int, GameObject>();
        //prefab_pair.Key - int��, .Value - GameObject
        foreach(KeyValuePair<int,GameObject> prefab_pair in tileset)
        {
            //Ÿ���������� �̸������� ���ӿ�����Ʈ �����ϰ�, �θ� �� ������Ʈ�� ����
            GameObject tile_group = new GameObject(prefab_pair.Value.name);
            tile_group.transform.parent = gameObject.transform;
            tile_group.transform.localPosition = new Vector3(0,0,0);
            tile_groups.Add(prefab_pair.Key, tile_group); //�����ĺ�intŰ�� ���ӿ�����Ʈ
        }
    }
    //����, �ʺ�, Ȯ������� offset���� �� ����
    void GenerateMap()
    {
        for(int x=0;x<map_width;x++)
        {
            noise_grid.Add(new List<int>());
            tile_grid.Add(new List<GameObject>());
            for (int y=0;y<map_height;y++)
            {
                int tile_id = GetIdUsingPerlin(x, y);
                noise_grid[x].Add(tile_id);
                CreateTile(tile_id, x, y); //Ÿ�ϻ���
            }
        }
    }
    int GetIdUsingPerlin(int x,int y)
    {
        float row_perlin = Mathf.PerlinNoise
            ((x - x_offset) / magnification,
             (y - y_offset) / magnification);
        float clamp_perlin = Mathf.Clamp01(row_perlin);
        float scaled_perlin = clamp_perlin * tileset.Count;

        if(scaled_perlin==4)
        {
            scaled_perlin = 3;
        }

        return Mathf.FloorToInt(scaled_perlin);
    }

    void CreateTile(int tile_id,int x, int y)
    {
        GameObject tile_prefab = tileset[tile_id];
        GameObject tile_group = tile_groups[tile_id];
        GameObject tile = Instantiate(tile_prefab, tile_group.transform);
        tile.name = string.Format("tile_x{0}_y{1}",x, y);
        tile.transform.localPosition = new Vector3(x, y,0);

        tile_grid[x].Add(tile);
    }
}
