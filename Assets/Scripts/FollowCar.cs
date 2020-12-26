using UnityEngine;

public class FollowCar : MonoBehaviour
{

    public Transform cameraTarget;
    public Vector3 velocity = Vector3.zero;
    public Transform lookTarget;

    void FixedUpdate()
    {
        Vector3 sPos = Vector3.Lerp(transform.position, cameraTarget.position, 0.2f);//interpolation
        transform.position = sPos;
        transform.LookAt(lookTarget.position);
    }

}