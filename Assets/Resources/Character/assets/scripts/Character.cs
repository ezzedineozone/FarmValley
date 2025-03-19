using UnityEngine;
using System.Collections;
using System;

public class Character : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float movement_sensitivity = 1f;
    public Animator anim;
    Rigidbody2D rb;
    public static Character instance;
    public static event Action playerInteracted;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        anim = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        HandleCharacterMovement();
        if (Input.GetMouseButton(0))
        {
            StartCoroutine(CorutineSwing());
        }
    }
    void HandleCharacterMovement()
    {
        if (anim.GetBool("in_action"))
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("is_moving", false);
            anim.SetFloat("x_speed", 0);
            anim.SetFloat("y_speed", 0);
            return;
        }
        float hz = Input.GetAxis("Horizontal");
        float vt = Input.GetAxis("Vertical");
        if (hz > 0)
        {
            anim.SetFloat("left_right_dir", 1);
            anim.SetFloat("movement_mode", 0);
            anim.SetBool("is_moving", true);
        }
        else if (hz < 0)
        {
            anim.SetFloat("left_right_dir", 0);
            anim.SetFloat("movement_mode", 0);
            anim.SetBool("is_moving", true);
        }
        if (vt > 0)
        {
            anim.SetFloat("up_down_dir", 0);
            anim.SetFloat("movement_mode", 1);
            anim.SetBool("is_moving", true);
        }
        else if (vt < 0)
        {
            anim.SetFloat("up_down_dir", 1);
            anim.SetFloat("movement_mode", 1);
            anim.SetBool("is_moving", true);
        }
        if (hz != 0 && vt != 0)
        {
            Vector2 movement = new Vector2(hz, vt) * movement_sensitivity;
            rb.linearVelocity = movement;
            anim.SetFloat("x_speed", hz);
            anim.SetFloat("y_speed", vt);
            anim.SetFloat("movement_mode", 0);
        }
        else if (hz == 0 && vt != 0)
        {
            anim.SetFloat("x_speed", 0);
            anim.SetFloat("y_speed", vt);
            anim.SetFloat("movement_mode", 1);
            Vector2 movement = new Vector2(0, vt) * movement_sensitivity;
            rb.linearVelocity = movement;
        }
        else if (hz != 0 && vt == 0)
        {
            anim.SetFloat("x_speed", hz);
            anim.SetFloat("y_speed", 0);
            anim.SetFloat("movement_mode", 0);
            Vector2 movement = new Vector2(hz, 0) * movement_sensitivity;
            rb.linearVelocity = movement;
        }
        else
        {
            anim.SetFloat("x_speed", 0);
            anim.SetFloat("y_speed", 0);
            anim.SetBool("is_moving", false);
            rb.linearVelocity = Vector2.zero;
        }
    }
    void Swing()
    {
        playerInteracted?.Invoke();
        anim.SetBool("in_action", true);
        anim.SetBool("is_swinging", true);
    }
    IEnumerator CorutineSwing()
    {
        if(anim.GetBool("in_action"))
        {
            yield break;
        }
        Swing();
        yield return new WaitForSeconds(1.0f);
        anim.SetBool("is_swinging", false);
        anim.SetBool("in_action", false);
    }
}
