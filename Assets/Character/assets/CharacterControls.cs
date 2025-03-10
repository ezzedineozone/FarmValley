using UnityEngine;

public class CharacterControlls : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed = 0.5f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 )
        {
            Debug.Log("Horizontal: " + Input.GetAxis("Horizontal") + " Vertical: " + Input.GetAxis("Vertical"));
            float hz = Input.GetAxis("Horizontal");
            float vt = Input.GetAxis("Vertical");
            this.transform.position += new Vector3(hz , vt, 0 ) * speed * Time.deltaTime;
        }
    }
}
