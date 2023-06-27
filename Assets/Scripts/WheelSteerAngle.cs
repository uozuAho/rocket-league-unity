using UnityEngine;

public class WheelSteerAngle : MonoBehaviour
{
    public float steerSpeed;
    public float maxAngle;

    private Quaternion startRotation;
    
    void Start()
    {
        startRotation = transform.localRotation;
    }

    void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var steerAngle = horizontalInput * maxAngle;

        var target = Quaternion.Euler(
            startRotation.eulerAngles.x,
            startRotation.eulerAngles.y + steerAngle,
            startRotation.eulerAngles.z);
        
        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            target,
            Time.deltaTime * steerSpeed);
    }
}
