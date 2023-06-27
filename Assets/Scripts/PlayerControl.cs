using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float accelForce;
    public float turnForce;
    public float jumpForce;

    private Rigidbody _playerRb;
    
    void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        var forwardInput = Input.GetAxis("Vertical");
        var horizontalInput = Input.GetAxis("Horizontal");
        var jumpInput = Input.GetKeyDown(KeyCode.Space);
        
        _playerRb.AddForce(Vector3.forward * Time.deltaTime * accelForce * forwardInput);
        _playerRb.AddTorque(Vector3.up * Time.deltaTime * turnForce * horizontalInput);

        HandleJumps(jumpInput);
    }

    private void HandleJumps(bool jumpInput)
    {
        if (jumpInput)
        {
            _playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
