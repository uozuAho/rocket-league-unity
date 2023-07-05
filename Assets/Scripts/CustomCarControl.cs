using UnityEngine;

public class CustomCarControl : MonoBehaviour
{
    // config
    public float accelPower;
    public float forwardFriction;
    public float sidewaysFriction;
    public float spinFriction;
    public float spinDrag;
    public float jumpForce;
    public float turnSpeed;
    public float airYawForce;

    public bool IsOnGround { get; private set; }

    private Rigidbody _rb;
    private GameObject _rayOrigin;
    private PlayerControls _controls;
    private bool jumpPressed;
    private float throttleInput;
    private bool hasDoubleJump = true;
    private float joystickSteer;

    void Awake()
    {
        _controls = new PlayerControls();
        InitControls();
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rayOrigin = GameObject.Find("RayOrigin");
    }

    void InitControls()
    {
        _controls.Player.Jump.performed += ctx => Jump();

        _controls.Player.Steer.performed += ctx => joystickSteer = ctx.ReadValue<float>();
        _controls.Player.Steer.canceled += ctx => joystickSteer = 0f;

        _controls.Player.Throttle.performed += ctx => throttleInput = ctx.ReadValue<float>();
        _controls.Player.Throttle.canceled += ctx => throttleInput = 0f;
    }

    void OnEnable()
    {
        _controls.Player.Enable();
    }

    void OnDisable()
    {
        _controls.Player.Disable();
    }

    void Update()
    {
    }

    private void Jump()
    {
        if (IsOnGround)
            _rb.AddForce(transform.up * jumpForce * 1000, ForceMode.Impulse);
        else if (hasDoubleJump)
        {
            _rb.AddForce(transform.up * jumpForce * 1000, ForceMode.Impulse);
            hasDoubleJump = false;
        }
    }

    void FixedUpdate()
    {
        var forwardSpeed = Vector3.Dot(_rb.velocity, transform.forward);

        CheckGround();

        // steering / air yaw
        if (IsOnGround)
            transform.Rotate(transform.up, joystickSteer * forwardSpeed * turnSpeed * Time.deltaTime);
        else
        {
            _rb.AddTorque(transform.up * joystickSteer * airYawForce * 1000);
        }

        // acceleration
        if (IsOnGround)
            _rb.AddForce(transform.forward * throttleInput * accelPower * 1000);

        // forward deceleration due to friction
        if (IsOnGround)
            _rb.AddForce(-transform.forward * forwardSpeed * forwardFriction * 1000);

        // sideways deceleration due to friction
        if (IsOnGround)
        {
            var sidewaysSpeed = Vector3.Dot(_rb.velocity, transform.right);
            _rb.AddForce(-transform.right * sidewaysSpeed * sidewaysFriction * 1000);
        }

        // spin friction
        if (IsOnGround)
        {
            _rb.AddTorque(-_rb.angularVelocity * spinFriction * 1000);
        }
        else
        {
            _rb.AddTorque(-_rb.angularVelocity * spinDrag * 1000);
        }
    }

    private void CheckGround()
    {
        const float rayLength = 0.25f;

        RaycastHit hit;

        if (Physics.Raycast(_rayOrigin.transform.position,
                transform.TransformDirection(Vector3.down),
                out hit,
                rayLength,
                LayerMask.GetMask("Ground")))
        {
            IsOnGround = true;
            hasDoubleJump = true;
            Debug.DrawRay(
                _rayOrigin.transform.position,
                transform.TransformDirection(Vector3.down) * rayLength, Color.red);
        }
        else
        {
            IsOnGround = false;
            Debug.DrawRay(_rayOrigin.transform.position,
                transform.TransformDirection(Vector3.down) * rayLength, Color.blue);
        }
    }
}