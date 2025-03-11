using UnityEngine;

public class CharacterControlls : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float movement_sensitivity = 1f;
    Animator anim;
    Rigidbody2D rb;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 )
        {
            float hz = Input.GetAxis("Horizontal");
            float vt = Input.GetAxis("Vertical");
            Vector3 movement = new Vector2(hz, vt) * movement_sensitivity;
            rb.linearVelocity = movement;
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
        } else
        {
            anim.SetFloat("x_speed", 0);
            anim.SetFloat("y_speed", 0);
            anim.SetFloat("movement_mode", 1);
            rb.linearVelocity = Vector2.zero;
        }
    }
}
