using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Horse : NPC
{
    [SerializeField] private GameEvents gameEvents;

    public float moveSpeed = 2f;
    public float rayDistance = 1.5f;
    public float minRoamTime = 1f;
    public float maxRoamTime = 3f;
    public float minIdleTime = 1f;
    public float maxIdleTime = 2.5f;
    public Vector2 roamingBounds = new Vector2(5f, 5f);

    private Vector2 roamingDirection;
    private float stateTimer;
    private bool isWalking;
    private bool isEating;
    private bool follows_player;
    private bool is_tamed;
    private bool isMounted;

    private Rigidbody2D rb;
    private Animator anim;
    private Coroutine eatingCoroutine;
    private Coroutine roamDirectionCoroutine;

    protected override void Start()
    {
        base.Start();
        UpdateTexture(Texture.Horse);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        StartRoaming();

        StartCoroutines();
        gameEvents.onHotbarChange.AddListener(playerHotBarSlotChange);
        gameEvents.onPlayerInteractWithItem.AddListener(onPlayerInteract);
    }
    public void UpdateMountedAnimation(Vector2 movement)
    {
        if (!isMounted || anim == null) return;

        bool isMoving = movement != Vector2.zero;
        anim.SetBool("is_moving", isMoving);

        if (isMoving)
        {
            anim.SetFloat("move_x", movement.x);
            anim.SetFloat("move_y", movement.y);

            transform.localScale = new Vector3(movement.x < -0.1f ? -1 : 1, 1, 1);
        }
    }

    private void onPlayerInteract(Vector2 lookDir, Item item)
    {
        if (item.texture != Item.ItemTexture.Wheat) return;

        if (!IsPlayerInRange())
        {
            Debug.Log("Player is not in range to interact with the horse.");
            return;
        }

        if (!IsPlayerLookingAt(lookDir))
        {
            Debug.Log("Player is not looking at the horse.");
            return;
        }

        if (isMounted)
        {
            Debug.Log("Already mounted.");
            return;
        }

        if (is_tamed)
        {
            MountHorse();
            return;
        }

        is_tamed = true;
        follows_player = true;
        StopCoroutines();
        isWalking = false;
        anim.SetBool("is_tamed", true);
        gameEvents.onHotbarChange.Invoke(null);
    }

    private void MountHorse()
    {
        if (isMounted) return;

        Character player = Character.instance;
        if (player == null) return;

        isMounted = true;

        transform.SetParent(player.transform);
        transform.localPosition = new Vector3(0, -0.05f, 0);
        rb.isKinematic = true;
        Character.instance.movement_sensitivity = 1.5f; 
        anim.SetBool("is_mounted", true);
        Vector3 newPosition = this.transform.position;
        newPosition.z = -2.0f;
        this.transform.position = newPosition;

        // Disable AI movement while mounted
        StopCoroutines();
        isWalking = false;
        follows_player = false;
    }



    private void UnmountHorse()
    {
        if (!isMounted) return;

        Character player = Character.instance;
        if (player == null) return;

        isMounted = false;

        transform.SetParent(null);
        transform.position = player.transform.position + new Vector3(1f, 0, 0); // dismount offset
        rb.isKinematic = false;
        anim.SetBool("is_mounted", false);

        // Optionally resume roaming or following
        if (is_tamed)
        {
            follows_player = true;
        }
        StartCoroutines();
    }

    private void Update()
    {
        if (isMounted && Input.GetKeyDown(KeyCode.Space))
        {
            UnmountHorse();
            return;
        }

        if (!isRoaming || isEating) return;

        stateTimer -= Time.deltaTime;

        if (follows_player || is_tamed && !isMounted)
        {
            Character player = Character.instance;
            Vector2 playerPosition = player.transform.position;
            Vector2 horsePosition = transform.position;
            float distance = Vector2.Distance(playerPosition, horsePosition);

            if (distance > 0.5f)
            {
                Vector2 directionToPlayer = (playerPosition - horsePosition).normalized;
                rb.MovePosition(rb.position + directionToPlayer * moveSpeed * Time.deltaTime);
                anim.SetBool("is_moving", true);
                anim.SetFloat("move_x", directionToPlayer.x);
                anim.SetFloat("move_y", directionToPlayer.y);

                transform.localScale = new Vector3(directionToPlayer.x < -0.1f ? -1 : 1, 1, 1);
            }
            else
            {
                anim.SetBool("is_moving", false);
            }

            return;
        }

        bool shouldMove = isWalking && !IsObstacleAhead();

        if (anim != null)
        {
            anim.SetBool("is_moving", shouldMove);

            if (shouldMove)
            {
                anim.SetFloat("move_x", roamingDirection.x);
                anim.SetFloat("move_y", roamingDirection.y);

                transform.localScale = new Vector3(roamingDirection.x < -0.1f ? -1 : 1, 1, 1);
            }
        }

        if (shouldMove)
        {
            rb.MovePosition(rb.position + roamingDirection * moveSpeed * Time.deltaTime);
        }

        if (stateTimer <= 0f)
        {
            isWalking = Random.value > 0.5f;
            stateTimer = isWalking
                ? Random.Range(minRoamTime, maxRoamTime)
                : Random.Range(minIdleTime, maxIdleTime);
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
        Vector2[] directions = {
            new Vector2(1, 0), new Vector2(-1, 0),
            new Vector2(0, 1), new Vector2(0, -1),
            new Vector2(1, 1), new Vector2(-1, -1),
            new Vector2(-1, 1), new Vector2(1, -1)
        };

        List<Vector2> validDirections = new List<Vector2>();
        Vector2 currentPosition = rb.position;
        Vector2 boundingBoxMin = currentPosition - roamingBounds;
        Vector2 boundingBoxMax = currentPosition + roamingBounds;

        foreach (Vector2 direction in directions)
        {
            Vector2 newPosition = currentPosition + direction * rayDistance;
            if (newPosition.x >= boundingBoxMin.x && newPosition.x <= boundingBoxMax.x &&
                newPosition.y >= boundingBoxMin.y && newPosition.y <= boundingBoxMax.y)
            {
                validDirections.Add(direction.normalized);
            }
        }

        roamingDirection = validDirections.Count > 0 ? validDirections[Random.Range(0, validDirections.Count)] : Vector2.zero;
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
        anim?.SetBool("is_eating", true);
        yield return new WaitForSeconds(1f);
        anim?.SetBool("is_eating", false);
        isEating = false;
    }

    private IEnumerator ChangeRoamingDirectionCoroutine()
    {
        while (isRoaming)
        {
            yield return new WaitForSeconds(Random.Range(5f, 8f));
            ChooseRoamingDirection();
        }
    }

    private void playerHotBarSlotChange(Item item)
    {
        if (is_tamed) return;

        if (item == null)
        {
            follows_player = false;
            isWalking = true;
            StartCoroutines();
            return;
        }

        if (item.texture == Item.ItemTexture.Wheat && IsPlayerInRange())
        {
            follows_player = true;
            StopCoroutines();
            isWalking = false;
        }
        else
        {
            follows_player = false;
            isWalking = true;
            StartCoroutines();
        }
    }

    private void StartCoroutines()
    {
        if (eatingCoroutine == null)
            eatingCoroutine = StartCoroutine(RandomEatingCoroutine());

        if (roamDirectionCoroutine == null)
            roamDirectionCoroutine = StartCoroutine(ChangeRoamingDirectionCoroutine());
    }

    private void StopCoroutines()
    {
        if (eatingCoroutine != null)
        {
            StopCoroutine(eatingCoroutine);
            eatingCoroutine = null;
        }

        if (roamDirectionCoroutine != null)
        {
            StopCoroutine(roamDirectionCoroutine);
            roamDirectionCoroutine = null;
        }
    }

    public bool IsPlayerInRange()
    {
        Transform ts = Character.instance.transform;
        float distance = Vector3.Distance(ts.position, this.transform.position);
        return distance < 2.5f;
    }

    public bool IsPlayerLookingAt(Vector2 lookDir)
    {
        return true; // Simplified - you can implement this better based on direction checks
    }
}
