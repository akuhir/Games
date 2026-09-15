using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 8f;
    public float height = 4f;
    public float followSpeed = 5f;
    public float lookAhead = 3f;

    void LateUpdate()
    {
        if (!target) return;
        Vector3 desired = target.position - target.forward * distance + Vector3.up * height;
        transform.position = Vector3.Lerp(transform.position, desired, followSpeed * Time.deltaTime);
        Vector3 lookPoint = target.position + Vector3.up + target.forward * lookAhead;
        transform.LookAt(lookPoint);
    }
}
