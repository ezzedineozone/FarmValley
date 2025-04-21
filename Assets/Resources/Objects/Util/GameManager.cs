using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject camera;
    [SerializeField]
    private TileSetSpawner tileSetManager;
    [SerializeField]
    private int game_width_height = 10;
    [SerializeField]
    private Character mainCharacter;
    [SerializeField]
    private GameObject dayNightCycle;
    [SerializeField]
    private GameObject sleepIndicator;
    [SerializeField]
    private GameEvents gameEvents;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 bottomLeft = tileSetManager.tileSetPrefabLarge.GetComponent<Renderer>().bounds.min;
        Vector2 game_boundaries = tileSetManager.setGameWidthHeight(game_width_height);
        camera.GetComponent<CameraScript>().setGameWidthHeight(game_boundaries, bottomLeft);
        mainCharacter.setGameWidthHeight(game_boundaries, bottomLeft);
        tileSetManager.spawnTileMaps();
        sleepIndicator.SetActive(true);
        gameEvents.onDayPassed.AddListener(checkPlantedTiles);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void checkPlantedTiles(){
        GameObject[] all_tiles = GameObject.FindGameObjectsWithTag("Tile");
        Debug.Log("Checking planted tiles...");
        Debug.Log("Checking planted tiles...");
                Debug.Log("Checking planted tiles...");
                        Debug.Log("Checking planted tiles...");
                                Debug.Log("Checking planted tiles...");
                                        Debug.Log("Checking planted tiles...");
        foreach (GameObject tile in all_tiles)
        {
            Tile tileScript = tile.GetComponent<Tile>();
            tileScript.GrowPlant();
        }
    }
}
