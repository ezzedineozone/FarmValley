using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float zoom_sensitivity = 1f;
    public float max_zoom = 5f;
    public float min_zoom = 0.5f;
    public GameObject character;
    private Vector2 game_width_height = new Vector2(10, 10);
    private Vector2 origin = new Vector2(0, 0);


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangeCameraZoom();
        FollowCharacter();
    }

    void FollowCharacter()
    {
        float camHeight = Camera.main.orthographicSize;
        float camWidth = camHeight * Camera.main.aspect;
        float clampedX = Mathf.Clamp(character.transform.position.x, origin.x + camWidth, origin.x + game_width_height.x - camWidth);
        float clampedY = Mathf.Clamp(character.transform.position.y, origin.y + camHeight, origin.y + game_width_height.y - camHeight);
        Camera.main.transform.position = new Vector3(clampedX, clampedY, Camera.main.transform.position.z);
    }

    public void setGameWidthHeight(Vector2 game_width_height, Vector2 origin)
    {
        this.game_width_height = game_width_height;
        this.origin = origin;
        Debug.Log("Game Origin: " + origin);
        Debug.Log("Game Width Height: " + game_width_height);
    }
    void ChangeCameraZoom(){
        float mw = Input.GetAxis("Mouse ScrollWheel");
        // get ctrl button
        bool ctrl_clicked = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
        if(!ctrl_clicked) return;
        if (mw > 0f) 
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + mw * zoom_sensitivity * -1, min_zoom, max_zoom);
        }
        else if (mw < 0f) 
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + mw * zoom_sensitivity * -1, min_zoom, max_zoom);
        }
    }
}