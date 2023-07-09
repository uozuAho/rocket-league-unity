using UnityEngine;

public class CubeControl : MonoBehaviour
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

    private Rigidbody _rb;
    private PlayerControls _controls;
    private bool jumpPressed;
    private float throttleInput;
    private float joystickSteer;
    private float joystickPitch;
    private float airRoll;

    void Awake()
    {
        _controls = new PlayerControls();
        InitControls();
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void InitControls()
    {
        _controls.Player.Jump.performed += ctx => Jump();

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
        // flip
        _rb.AddTorque(transform.right * jumpForce, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        var forwardSpeed = Vector3.Dot(_rb.velocity, transform.forward);

        // air yaw
        _rb.AddTorque(transform.up * joystickSteer * airYawForce);

        // pitch
        _rb.AddTorque(transform.right * joystickPitch * airYawForce);

        // roll
        // todo: why does this need to be backwards to work?
        _rb.AddTorque(-transform.forward * airRoll * airYawForce);

        // spin friction
        // _rb.AddTorque(-_rb.angularVelocity * spinDrag * 1000);
    }
}
