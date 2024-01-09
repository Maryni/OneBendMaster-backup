using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour
{
    #region Inspector variables

    [SerializeField] private Toggle doubleScoreToggle;
    [SerializeField] private Toggle scoreByTimerToggle;

    #endregion Inspector variables

    #region private variables

    private Action<bool> doubleScoreToggleAction;
    private Action<bool> scoreByTimerToggleAction;

    #endregion private variables

    #region public functions

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

    #endregion public functions

    #region private functions

    private void DoubleScoreCallback(bool value) => doubleScoreToggleAction?.Invoke(value);
    private void AutoScoreCallback(bool value) => scoreByTimerToggleAction?.Invoke(value);

    #endregion private functions
}
