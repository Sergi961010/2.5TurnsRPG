using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    PlayerControls playerControls;
    Rigidbody rb;
    Vector3 movement;

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
    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + speed * Time.fixedDeltaTime * movement);
    }
}