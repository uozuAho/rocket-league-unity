using UnityEngine;

public class CamPlayerFollower : MonoBehaviour
{
    public GameObject playerCar;
    public Vector3 offset;

    void Start()
    {
    }

    void Update()
    {
    }

    void LateUpdate()
    {
        transform.position = playerCar.transform.position + offset;

        var camAngle = Quaternion.FromToRotation(transform.position, playerCar.transform.position);
        transform.localRotation = camAngle;
    }
}
