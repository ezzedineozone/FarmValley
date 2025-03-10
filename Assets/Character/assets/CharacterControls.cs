using UnityEngine;

public class CharacterControlls : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float movement_sensitivity = 0.5f;
    Animator anim;

    void Start()
    {
       anim = this.GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 )
        {
            float hz = Input.GetAxis("Horizontal");
            float vt = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(hz, vt, 0);
            transform.position += movement * movement_sensitivity * Time.deltaTime;
            float x_speed = hz;
            float y_speed = vt;
            anim.SetFloat("x_speed", x_speed);
            anim.SetFloat("y_speed", y_speed);
            //set movement mode 0 if left right 1 if up down
            if (x_speed != 0)
            {
                anim.SetFloat("movement_mode", 0);
            }
            else if( y_speed != 0)
            {
                anim.SetFloat("movement_mode", 1);
            } 
        }
    }
}
