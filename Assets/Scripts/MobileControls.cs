using UnityEngine;
using UnityEngine.EventSystems;

public class MobileControls : MonoBehaviour
{
    public CarController car;

    public void AccelerateDown() => car.SetThrottle(1f);
    public void BrakeDown() => car.SetThrottle(-1f);
    public void GasUp() => car.SetThrottle(0f);
    public void BrakeUp() => car.SetThrottle(0f);
    public void LeftDown() => car.SetSteering(-1f);
    public void RightDown() => car.SetSteering(1f);
    public void SteeringUp() => car.SetSteering(0f);
}
