using UnityEngine;

public class FinishScreen : MonoBehaviour
{
    public void Restart() => RaceManager.Instance.RestartRace();
}
