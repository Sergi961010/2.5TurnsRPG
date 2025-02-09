using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const string IS_WALKING = "IsWalking";
    const float timePerStep = 0.5f;

    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float speed;
    [SerializeField] LayerMask grassLayer;
    [SerializeField] int stepsInGrass;

    PlayerControls playerControls;
    Rigidbody rb;
    Vector3 movement;
    float stepTimer;

    void Awake()
    {
        playerControls = new PlayerControls();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        playerControls.Enable();
    }

    void Update()
    {
        Vector2 inputMovement = playerControls.Player.Move.ReadValue<Vector2>();
        movement = new Vector3(inputMovement.x, 0f, inputMovement.y).normalized;

        animator.SetBool(IS_WALKING, movement != Vector3.zero);

        if (movement != Vector3.zero)
        {
            spriteRenderer.flipX = movement.x < 0f;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + speed * Time.fixedDeltaTime * movement);

        Collider[] colliders = Physics.OverlapSphere(transform.position, 1, grassLayer);
        bool movingInGras = colliders.Length != 0 && movement != Vector3.zero;

        if (movingInGras)
        {
            stepTimer += Time.fixedDeltaTime;
            if (stepTimer >= timePerStep)
            {
                stepsInGrass++;
                stepTimer = 0f;
            }
        }
    }
}