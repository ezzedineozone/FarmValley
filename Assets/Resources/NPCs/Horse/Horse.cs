using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Horse : NPC
{
    public float moveSpeed = 2f;
    public float rayDistance = 1.5f;
    public float minRoamTime = 1f;
    public float maxRoamTime = 3f;
    public float minIdleTime = 1f;
    public float maxIdleTime = 2.5f;

    private Vector2 roamingDirection;
    private float stateTimer;
    private bool isWalking;
    private bool isEating;

    private Rigidbody2D rb;
    private Animator anim;

    protected override void Start()
    {
        base.Start();
        UpdateTexture(Texture.Horse);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        StartRoaming();

        StartCoroutine(RandomEatingCoroutine()); // ✅ Start only once!
        StartCoroutine(ChangeRoamingDirectionCoroutine()); // ✅ Start only once!
    }

    private void Update()
    {
        if (!isRoaming || isEating) return; // ✅ Don't move if eating

        stateTimer -= Time.deltaTime;

        if (stateTimer <= 0f)
        {
            isWalking = Random.value > 0.5f;
            stateTimer = isWalking
                ? Random.Range(minRoamTime, maxRoamTime)
                : Random.Range(minIdleTime, maxIdleTime);


            if (isWalking && !IsObstacleAhead())
            {
                rb.MovePosition(rb.position + roamingDirection * moveSpeed * Time.deltaTime);

                if (anim != null)
                {
                    anim.SetBool("is_moving", true);
                    anim.SetFloat("move_x", roamingDirection.x);
                    anim.SetFloat("move_y", roamingDirection.y);

                    if (roamingDirection.x < -0.1f)
                        transform.localScale = new Vector3(-1, 1, 1);
                    else if (roamingDirection.x > 0.1f)
                        transform.localScale = new Vector3(1, 1, 1);
                }
            }
            else
            {
                if (anim != null)
                    anim.SetBool("is_moving", false);
            }
        }

        if (isWalking && !IsObstacleAhead())
        {
            rb.MovePosition(rb.position + roamingDirection * moveSpeed * Time.deltaTime);
        }
    }

    public override void StartRoaming()
    {
        base.StartRoaming();
        stateTimer = 0f;
        Debug.Log("Horse started randomly walking/standing.");
    }

    private void ChooseRoamingDirection()
    {
        Vector2[] directions = new Vector2[]
        {
            new Vector2(1, 0),
            new Vector2(-1, 0),
            new Vector2(0, 1),
            new Vector2(0, -1),
            new Vector2(1, 1),
            new Vector2(-1, -1),
            new Vector2(-1, 1),
            new Vector2(1, -1)
        };

        roamingDirection = directions[Random.Range(0, directions.Length)].normalized;
    }


    private bool IsObstacleAhead()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, roamingDirection, rayDistance);
        return hit.collider != null && hit.collider.attachedRigidbody != null;
    }

    private IEnumerator RandomEatingCoroutine()
    {
        while (true)
        {
            float waitTime = Random.Range(10f, 15f);
            yield return new WaitForSeconds(waitTime);

            yield return StartCoroutine(EatingCoroutine());
        }
    }

    private IEnumerator EatingCoroutine()
    {
        isEating = true;
        Debug.Log("Horse is eating.");

        if (anim != null)
            anim.SetBool("is_eating", true);

        yield return new WaitForSeconds(1f);

        if (anim != null)
            anim.SetBool("is_eating", false);

        isEating = false;
    }

    //change roaming direction corutine
    private IEnumerator ChangeRoamingDirectionCoroutine()
    {
        while (isRoaming)
        {
            yield return new WaitForSeconds(Random.Range(5f, 8f));
            ChooseRoamingDirection();
        }
    }
}
