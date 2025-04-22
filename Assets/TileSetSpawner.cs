using UnityEngine;

public class TileSetSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject tileSetGrid;
    [SerializeField]
    public GameObject tileSetPrefabLarge;
    private int width_height = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void spawnTileMaps()
    {
        Vector3 tileSize = tileSetPrefabLarge.GetComponent<Renderer>().bounds.size;

        float totalWidth = width_height * tileSize.x;
        float totalHeight = width_height * tileSize.y;

        Vector3 originOffset = new Vector3(totalWidth / 2f - tileSize.x / 2f, totalHeight / 2f - tileSize.y / 2f, 0);

        for (int i = 0; i < width_height; i++)
        {
            for (int j = 0; j < width_height; j++)
            {
                Vector3 position = new Vector3(i * tileSize.x, j * tileSize.y, 0) - originOffset;
                GameObject tileSet = Instantiate(tileSetPrefabLarge, position, Quaternion.identity);
                tileSet.transform.parent = tileSetGrid.transform;
                tileSet.transform.localScale = Vector3.one;
            }
        }

        // Now place walls *around* the grid
        CreateBorder("LeftWall",    new Vector2(-totalWidth / 2f - 0.5f, 0), new Vector2(1f, totalHeight));
        CreateBorder("RightWall",   new Vector2(totalWidth / 2f + 0.5f, 0), new Vector2(1f, totalHeight));
        CreateBorder("TopWall",     new Vector2(0, totalHeight / 2f + 0.5f), new Vector2(totalWidth, 1f));
        CreateBorder("BottomWall",  new Vector2(0, -totalHeight / 2f - 0.5f), new Vector2(totalWidth, 1f));
    }


    private void CreateBorder(string name, Vector2 position, Vector2 size)
    {
        GameObject wall = new GameObject(name);
        wall.transform.position = position;
        wall.transform.parent = tileSetGrid.transform;

        BoxCollider2D collider = wall.AddComponent<BoxCollider2D>();
        collider.size = size;
        collider.isTrigger = false;

        Rigidbody2D rb = wall.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
    }

    public Vector2 setGameWidthHeight(int width_height)
    {
        this.width_height = width_height;
        Vector3 tileSize = tileSetPrefabLarge.GetComponent<Renderer>().bounds.size;
        Vector2 gameDimensions = new Vector2(width_height * tileSize.x, width_height * tileSize.y);
        return gameDimensions;
    }
}
