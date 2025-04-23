using UnityEngine;
using System.Collections;
using System;
using Unity.VisualScripting;

public class Character : MonoBehaviour
{
    [SerializeField] public float movement_sensitivity = 1f;
    [SerializeField] GameEvents gameEvents; 

    public Animator anim;
    Rigidbody2D rb;
    public static Character instance;
    public static event Action playerInteracted;

    private Vector2 look_direction = Vector2.down;
    private float z_index = -1.0f;

    private Vector2 game_width_height = new Vector2(10, 10);
    private Vector3 origin = new Vector3(0, 0,0);

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            instance.tag = "Player";
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Entered: " + other.gameObject.name);
        
        if(true)
        {
            Item item = other.gameObject.GetComponent<Item>();
            if(item && !item.isPickable) return;
            if (item != null)
            {
                gameEvents.onItemPickup.Invoke(item);
            }
        }
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        this.transform.position = new Vector3(0, 0, z_index);
    }

    void Update()
    {
        HandleCharacterMovement();

        if (Input.GetMouseButton(0))
        {
            StartCoroutine(CorutineSwing());
        }
        // E key call interact
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key pressed");
            Interact();
        }
    }

void HandleCharacterMovement()
{
    if (anim.GetBool("in_action"))
    {
        StopMovement();
        return;
    }

    float hz = Input.GetAxis("Horizontal");
    float vt = Input.GetAxis("Vertical");

    Vector2 input = new Vector2(hz, vt);
    Vector2 movement = input.normalized * movement_sensitivity;
    Vector3 newPosition = rb.position + movement * Time.deltaTime;
    float clampedX = Mathf.Clamp(newPosition.x, origin.x, origin.x + game_width_height.x);
    float clampedY = Mathf.Clamp(newPosition.y, origin.y, origin.y + game_width_height.y);
    rb.position = new Vector2(clampedX, clampedY);

    bool isMoving = input != Vector2.zero;
    anim.SetBool("is_moving", isMoving);

    if (isMoving)
    {
        look_direction = input.normalized;
        if (transform.childCount > 0)
        {
            Horse mountedHorse = GetComponentInChildren<Horse>();
            if (mountedHorse != null)
            {
                mountedHorse.UpdateMountedAnimation(movement);
            }
        }
        anim.SetFloat("x_speed", hz);
        anim.SetFloat("y_speed", vt);

        if (Mathf.Abs(hz) > Mathf.Abs(vt))
        {
            anim.SetFloat("movement_mode", 0);
            anim.SetFloat("left_right_dir", hz > 0 ? 1 : 0);
        }
        else
        {
            anim.SetFloat("movement_mode", 1);
            anim.SetFloat("up_down_dir", vt > 0 ? 0 : 1);
        }
    }
    else
    {
        anim.SetFloat("x_speed", 0);
        anim.SetFloat("y_speed", 0);
    }
}

    void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
        anim.SetBool("is_moving", false);
        anim.SetFloat("x_speed", 0);
        anim.SetFloat("y_speed", 0);
    }

    void Swing()
    {
        playerInteracted?.Invoke();
        anim.SetBool("in_action", true);
        anim.SetBool("is_swinging", true);

        if (gameEvents != null)
        {
            gameEvents.onPlayerInteract.Invoke(look_direction);
        }
    }
    void Interact(){

        if (gameEvents != null)
        {
            gameEvents.onPlayerInteractWithItem.Invoke(look_direction, GameObject.FindObjectOfType<Hotbar>().GetItem());
        }
    }
    IEnumerator CorutineSwing()
    {
        if (anim.GetBool("in_action"))
            yield break;

        Swing();
        yield return new WaitForSeconds(1.0f);
        anim.SetBool("is_swinging", false);
        anim.SetBool("in_action", false);
    }
    public void setGameWidthHeight(Vector2 game_width_height, Vector3 origin)
    {
        this.game_width_height = game_width_height;
        this.origin = origin;
        Debug.Log("Game Origin: " + origin);
        Debug.Log("Game Width Height: " + game_width_height);
    }
}
