using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private float durationMinutes = 15f;
    [SerializeField] private TextMeshProUGUI timerText;

    private float timeRemaining;
    private bool isRunning;

    private void Start()
    {
        timeRemaining = durationMinutes * 60f;
        isRunning = true;
        UpdateTimerUI();
    }

    private void Update()
    {
        if (isRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                if (timeRemaining < 0)
                {
                    timeRemaining = 0;
                    isRunning = false;
                    UIManager.Instance.ActivateGameOverPanel();
                }
                UpdateTimerUI();
            }
            else
            {
                timeRemaining = 0;
                isRunning = false;
                UIManager.Instance.ActivateGameOverPanel();
                UpdateTimerUI();
            }
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = (int)timeRemaining / 60;
            int seconds = (int)timeRemaining % 60;
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}
