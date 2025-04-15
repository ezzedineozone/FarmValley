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
        // Get the size of the tile from the prefab's Renderer
        Vector3 tileSize = tileSetPrefabLarge.GetComponent<Renderer>().bounds.size;

        // Loop through the grid dimensions
        for (int i = 0; i < width_height; i++)
        {
            for (int j = 0; j < width_height; j++)
            {
                // Calculate the position for each tile
                Vector3 position = new Vector3(i * tileSize.x, j * tileSize.y, 0);

                // Instantiate the tile at the calculated position
                GameObject tileSet = Instantiate(tileSetPrefabLarge, position, Quaternion.identity);


                // Set the tile's parent to the tileSetGrid for better hierarchy organization
                tileSet.transform.parent = tileSetGrid.transform;
                tileSet.transform.localScale = new Vector3(1f, 1f, 1f);

            }
        }
    }
    public Vector2 setGameWidthHeight(int width_height)
    {
        this.width_height = width_height;
        Vector3 tileSize = tileSetPrefabLarge.GetComponent<Renderer>().bounds.size;
        Vector2 gameDimensions = new Vector2(width_height * tileSize.x, width_height * tileSize.y);
        return gameDimensions;
    }
}
