using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceManager : MonoBehaviour
{
    public static RaceManager Instance { get; private set; }
    public int totalCheckpoints = 6;
    public int lapsToWin = 3;
    public float raceTime;
    public int lap;
    public bool raceStarted;
    public bool raceOver;

    int expectedCheckpoint;
    bool crossedStart;
    public event Action<int> LapCompleted;
    public event Action<float> RaceFinished;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Update()
    {
        if (raceStarted && !raceOver) raceTime += Time.deltaTime;
    }

    public void CheckpointPassed(int index)
    {
        if (raceOver || index != expectedCheckpoint) return;
        if (!raceStarted) StartRace();

        if (index == 0)
        {
            if (crossedStart)
            {
                lap++;
                LapCompleted?.Invoke(lap);
                if (lap >= lapsToWin)
                {
                    raceOver = true;
                    RaceFinished?.Invoke(raceTime);
                }
            }
            crossedStart = true;
        }
        expectedCheckpoint = (index + 1) % totalCheckpoints;
    }

    public void StartRace()
    {
        raceStarted = true;
        raceOver = false;
        raceTime = 0f;
        lap = 0;
        expectedCheckpoint = 0;
        crossedStart = false;
    }

    public void RestartRace() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
