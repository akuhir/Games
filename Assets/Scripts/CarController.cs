using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Handling")]
    public float acceleration = 18f;
    public float maxSpeed = 35f;
    public float braking = 30f;
    public float steeringSpeed = 80f;
    public float friction = 8f;
    public float downForce = 30f;

    [HideInInspector] public float throttle;
    [HideInInspector] public float steering;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0f, -0.35f, 0f);
    }

    void FixedUpdate()
    {
        float keyboardThrottle = Input.GetAxisRaw("Vertical");
        float keyboardSteering = Input.GetAxisRaw("Horizontal");
        float finalThrottle = Mathf.Abs(throttle) > 0.01f ? throttle : keyboardThrottle;
        float finalSteering = Mathf.Abs(steering) > 0.01f ? steering : keyboardSteering;

        Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
        float forwardSpeed = localVelocity.z;

        if (finalThrottle > 0f)
            rb.AddForce(transform.forward * finalThrottle * acceleration, ForceMode.Acceleration);
        else if (finalThrottle < 0f)
            rb.AddForce(-transform.forward * braking * -finalThrottle, ForceMode.Acceleration);
        else
            rb.AddForce(-transform.forward * forwardSpeed * friction, ForceMode.Acceleration);

        float speedFactor = Mathf.Clamp01(Mathf.Abs(forwardSpeed) / 2f);
        float steerAmount = finalSteering * steeringSpeed * speedFactor * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, steerAmount, 0f));

        Vector3 horizontal = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (horizontal.magnitude > maxSpeed)
        {
            horizontal = horizontal.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(horizontal.x, rb.linearVelocity.y, horizontal.z);
        }

        rb.AddForce(Vector3.down * downForce, ForceMode.Acceleration);
    }

    public void SetThrottle(float value) => throttle = Mathf.Clamp(value, -1f, 1f);
    public void SetSteering(float value) => steering = Mathf.Clamp(value, -1f, 1f);
}
