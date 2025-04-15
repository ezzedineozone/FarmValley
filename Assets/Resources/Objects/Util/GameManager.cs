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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 bottomLeft = tileSetManager.tileSetPrefabLarge.GetComponent<Renderer>().bounds.min;
        Vector2 game_boundaries = tileSetManager.setGameWidthHeight(game_width_height);
        camera.GetComponent<CameraScript>().setGameWidthHeight(game_boundaries, bottomLeft);
        mainCharacter.setGameWidthHeight(game_boundaries, bottomLeft);
        tileSetManager.spawnTileMaps();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
