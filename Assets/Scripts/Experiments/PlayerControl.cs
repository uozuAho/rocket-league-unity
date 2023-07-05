using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float forwardFriction;
    public float sidewaysFriction;
    public float accelForce;
    public float turnForce;
    public float groundTurnForce;
    public float jumpForce;
    public float speed;

    private Rigidbody _playerRb;

    void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Jumps();
        Acceleration();
        Steering();
    }

    private void Steering()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        speed = _playerRb.velocity.magnitude;

        // todo: change direction based on direction of travel
        if (_playerRb.velocity.magnitude > 0.1)
        {
            _playerRb.AddTorque(
                Vector3.up
                * Time.deltaTime
                * horizontalInput
                * groundTurnForce);
        }
        // todo: stop spin after steering. feels like wheel steering should be
        //       firmly connected to the ground, not based on friction

        // todo: in-air spin:
        // _playerRb.AddTorque(Vector3.up * Time.deltaTime * turnForce * horizontalInput);
    }

    private void Acceleration()
    {
        var forwardInput = Input.GetAxis("Vertical");

        // gas
        _playerRb.AddForce(transform.forward * Time.deltaTime * accelForce * forwardInput);

        // forward/back friction
        _playerRb.AddForce(-_playerRb.velocity.normalized * Time.deltaTime * forwardFriction);

        // steer

        // spin friction
        // todo: prevent wobble
        _playerRb.AddTorque(-_playerRb.angularVelocity.normalized * Time.deltaTime * sidewaysFriction);

        // todo
        // - sideways friction
        // - no friction in air
    }

    private void Jumps()
    {
        var jumpInput = Input.GetKeyDown(KeyCode.Space);

        if (jumpInput)
        {
            _playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
