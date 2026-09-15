using UnityEngine;
using TMPro;

public class RaceHUD : MonoBehaviour
{
    public TMP_Text lapText;
    public TMP_Text timeText;
    public GameObject finishPanel;
    public TMP_Text resultText;

    void Start()
    {
        RaceManager.Instance.LapCompleted += OnLap;
        RaceManager.Instance.RaceFinished += OnFinished;
        UpdateLap();
        if (finishPanel) finishPanel.SetActive(false);
    }

    void Update()
    {
        if (!RaceManager.Instance) return;
        timeText.text = "Time: " + FormatTime(RaceManager.Instance.raceTime);
    }

    void OnLap(int _) => UpdateLap();
    void UpdateLap() => lapText.text = $"Lap: {RaceManager.Instance.lap} / {RaceManager.Instance.lapsToWin}";

    void OnFinished(float time)
    {
        if (finishPanel) finishPanel.SetActive(true);
        if (resultText) resultText.text = "Race Complete!\nTime: " + FormatTime(time);
    }

    string FormatTime(float t)
    {
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);
        int millis = Mathf.FloorToInt((t - Mathf.Floor(t)) * 100f);
        return $"{minutes:00}:{seconds:00}.{millis:00}";
    }
}
