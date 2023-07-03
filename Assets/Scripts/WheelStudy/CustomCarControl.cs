using UnityEngine;

public class CustomCarControl : MonoBehaviour
{
    public float accelPower;
    public float forwardFriction;
    public float sidewaysFriction;
    public float spinFriction;
    public float jumpForce;
    public float turnSpeed;
    public float airYawForce;

    private Rigidbody _rb;
    private GameObject _rayOrigin;
    private float turnInput;
    private bool jumpPressed;
    private float accelInput;
    private bool _isOnGround;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rayOrigin = GameObject.Find("RayOrigin");
    }

    void Update()
    {
        turnInput = Input.GetAxis("Horizontal");
        jumpPressed = Input.GetKeyDown(KeyCode.Space);
        accelInput = Input.GetAxis("Vertical");

        // jump - keydown doesn't register well in FixedUpdate
        if (jumpPressed)
            _rb.AddForce(transform.up * jumpForce * 1000, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        var forwardSpeed = Vector3.Dot(_rb.velocity, transform.forward);

        CheckGround();

        // steering / air yaw
        if (_isOnGround)
            transform.Rotate(transform.up, turnInput * forwardSpeed * turnSpeed * Time.deltaTime);
        else
        {
            _rb.AddTorque(transform.up * turnInput * airYawForce * 1000);
        }

        // acceleration
        if (_isOnGround)
            _rb.AddForce(transform.forward * accelInput * accelPower * 1000);

        // forward deceleration due to friction
        if (_isOnGround)
            _rb.AddForce(-transform.forward * forwardSpeed * forwardFriction * 1000);

        // sideways deceleration due to friction
        if (_isOnGround)
        {
            var sidewaysSpeed = Vector3.Dot(_rb.velocity, transform.right);
            _rb.AddForce(-transform.right * sidewaysSpeed * sidewaysFriction * 1000);
        }

        // spin friction
        if (_isOnGround)
        {
            _rb.AddTorque(-_rb.angularVelocity * spinFriction * 1000);
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
            _isOnGround = true;
            Debug.DrawRay(
                _rayOrigin.transform.position,
                transform.TransformDirection(Vector3.down) * rayLength, Color.red);
        }
        else
        {
            _isOnGround = false;
            Debug.DrawRay(_rayOrigin.transform.position,
                transform.TransformDirection(Vector3.down) * rayLength, Color.blue);
        }
    }
}
