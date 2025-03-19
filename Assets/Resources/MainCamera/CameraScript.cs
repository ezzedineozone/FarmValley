using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float zoom_sensitivity = 1f;
    public float max_zoom = 10f;
    public float min_zoom = 1f;
    public GameObject character;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mw = Input.GetAxis("Mouse ScrollWheel");
        if (mw > 0f) 
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + mw * zoom_sensitivity * -1, min_zoom, max_zoom);
        }
        else if (mw < 0f) 
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + mw * zoom_sensitivity * -1, min_zoom, max_zoom);
        }
        FollowCharacter();
    }
    void FollowCharacter()
    {
        Camera.main.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, Camera.main.transform.position.z);
    }
}
