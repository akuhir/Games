using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointIndex;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<CarController>())
            RaceManager.Instance?.CheckpointPassed(checkpointIndex);
    }
}
