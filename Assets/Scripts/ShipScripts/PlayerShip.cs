using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShip : Ship
{
    private float defaultHealth;
    public static PlayerShip Instance;

    private PlayerInputActions input;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction shootAction;

    public Animator animator;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        input = new PlayerInputActions();
        moveAction = input.Player.Move;
        lookAction = input.Player.Look;
        shootAction = input.Player.Shoot;
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        input?.Player.Enable();
    }

    void OnDisable()
    {
        input?.Player.Disable();
    }

    protected override void CustomStart()
    {
        defaultHealth = health;
    }

    protected override void Move()
    {
        if (moveDirection.magnitude > 0)
        {
            rigidBody.linearVelocity = moveDirection * moveSpeed;
        }
        else
        {
            rigidBody.linearVelocity -= rigidBody.linearVelocity * friction;
        }
    }


    Vector2 GetMousePos()
    {
        Vector2 screenPos = lookAction.ReadValue<Vector2>();
        Vector3 mousePos = new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        return new Vector2(worldPos.x, worldPos.y);
    }

    void Update()
    {
        moveDirection = moveAction.ReadValue<Vector2>().normalized;
        Vector2 shootDirection = (GetMousePos() - new Vector2(transform.position.x, transform.position.y)).normalized;
        transform.eulerAngles = new Vector3(0, 0, -90 + Mathf.Atan2(shootDirection.y, shootDirection.x) * 180 / Mathf.PI);


        if (shootAction.IsPressed() && canShoot)
        {
        StartCoroutine(Shoot(shootDirection, shootForce));
        }

        animator.SetBool("isShooting", shootAction.IsPressed());

    }
}