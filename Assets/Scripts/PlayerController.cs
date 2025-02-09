using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    const string IS_WALKING = "IsWalking";
    const string BATTLE_SCENE = "BattleScene";
    const float TIME_PER_STEP = 0.5f;

    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float speed;
    [SerializeField] LayerMask grassLayer;
    [SerializeField] int minStepsToEncounter;
    [SerializeField] int maxStepsToEncounter;


    PlayerControls playerControls;
    Rigidbody rb;
    Vector3 movement;
    float stepTimer;
    int stepsInGrass;
    int stepsToEncounter;

    void Awake()
    {
        playerControls = new PlayerControls();
        CalculateStepsToEncounter();
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
            if (stepTimer >= TIME_PER_STEP)
            {
                stepsInGrass++;
                stepTimer = 0f;
            }

            if (stepsInGrass >= stepsToEncounter)
            {
                SceneManager.LoadScene(BATTLE_SCENE);
            }
        }
    }

    void CalculateStepsToEncounter()
    {
        stepsToEncounter = Random.Range(minStepsToEncounter, maxStepsToEncounter);
    }
}