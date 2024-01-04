using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour
{
    [SerializeField] private Toggle doubleScoreToggle;
    [SerializeField] private Toggle scoreByTimerToggle;

    private Action<bool> doubleScoreToggleAction;
    private Action<bool> scoreByTimerToggleAction;


    public void SetActionsToDoubleScore(params Action<bool>[] actions)
    {
        foreach (var item in actions)
        {
            doubleScoreToggleAction += item;
        }

        doubleScoreToggle.onValueChanged.AddListener(DoubleScoreCallback);
    }
    
    public void SetActionsToAutoScore(params Action<bool>[] actions)
    {
        foreach (var item in actions)
        {
            scoreByTimerToggleAction += item;
        }
        
        scoreByTimerToggle.onValueChanged.AddListener(AutoScoreCallback);
    }

    private void DoubleScoreCallback(bool value) => scoreByTimerToggleAction?.Invoke(value);
    private void AutoScoreCallback(bool value) => scoreByTimerToggleAction?.Invoke(value);
}
