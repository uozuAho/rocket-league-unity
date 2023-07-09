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
    public float boostForce;
    public float flipBoostForce;
    public float flipTorque;

    public bool IsOnGround { get; private set; }

    private Rigidbody _rb;
    private GameObject _rayOrigin;
    private GameObject _driftOrigin;
    private MeshRenderer _boostFire;
    private PlayerControls _controls;
    private bool jumpPressed;
    private float throttleInput;
    private bool hasDoubleJump = true;
    private float joystickSteer;
    private float joystickPitch;
    private float airRoll;
    private bool handbrakeOn = false;

    void Awake()
    {
        _controls = new PlayerControls();
        InitControls();
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rayOrigin = GameObject.Find("RayOrigin");
        _driftOrigin = GameObject.Find("DriftOrigin");
        _boostFire = GameObject.Find("BoostFire").GetComponent<MeshRenderer>();
    }

    void InitControls()
    {
        _controls.Player.Jump.performed += ctx => Jump();

        _controls.Player.Boost.performed += ctx => _boostFire.enabled = true;
        _controls.Player.Boost.canceled += ctx => _boostFire.enabled = false;

        _controls.Player.Steer.performed += ctx => joystickSteer = ctx.ReadValue<float>();
        _controls.Player.Steer.canceled += ctx => joystickSteer = 0f;

        _controls.Player.Pitch.performed += ctx => joystickPitch = ctx.ReadValue<float>();
        _controls.Player.Pitch.canceled += ctx => joystickPitch = 0f;

        _controls.Player.AirRollLeft.performed += ctx => airRoll = -1f;
        _controls.Player.AirRollLeft.canceled += ctx => airRoll = 0f;

        _controls.Player.AirRollRight.performed += ctx => airRoll = 1f;
        _controls.Player.AirRollRight.canceled += ctx => airRoll = 0f;

        _controls.Player.Throttle.performed += ctx => throttleInput = ctx.ReadValue<float>();
        _controls.Player.Throttle.canceled += ctx => throttleInput = 0f;

        _controls.Player.Powerslide.performed += ctx => handbrakeOn = true;
        _controls.Player.Powerslide.canceled += ctx => handbrakeOn = false;
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
            hasDoubleJump = false;

            if (joystickPitch > 0.1)
            {
                // flip
                _rb.AddForce(transform.forward * flipBoostForce * 1000, ForceMode.Impulse);
                _rb.AddTorque(transform.right * flipTorque * 1000, ForceMode.Impulse);
            }
            else
            {
                // straight jump
                _rb.AddForce(transform.up * jumpForce * 1000, ForceMode.Impulse);
            }
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

        // air pitch
        if (!IsOnGround)
        {
            _rb.AddTorque(transform.right * joystickPitch * airYawForce * 1000);
        }

        // air roll
        if (!IsOnGround)
        {
            // todo: why does this need to be backwards to work?
            _rb.AddTorque(-transform.forward * airRoll * airYawForce * 500);
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
            var friction = -transform.right * sidewaysSpeed * sidewaysFriction * 1000;
            _rb.AddForce(friction);

            if (handbrakeOn)
            {
                var driftForce = -friction / 2;
                _rb.AddForceAtPosition(driftForce, _driftOrigin.transform.position);
            }
        }

        // spin friction
        if (IsOnGround)
            _rb.AddTorque(-_rb.angularVelocity * spinFriction * 1000);
        else
            _rb.AddTorque(-_rb.angularVelocity * spinDrag * 1000);

        if (_boostFire.enabled)
            _rb.AddForce(transform.forward * boostForce * 1000);
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
