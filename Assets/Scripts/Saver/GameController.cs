using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    #region private variables
    
    private UIController uiController;
    private MatchThreeButtons matchThreeButtons;
    private MatchThreeController_v2 matchThreeControllerV2;
    private UpgradeController upgradeController;

    #endregion private variables

    #region properties
    
    public UIController UIController => uiController;

    #endregion properties
    
    #region Unity functions

    private void OnEnable()
    {
        SceneManager.sceneLoaded += LoadAfterGameSceneWasLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= LoadAfterGameSceneWasLoaded;
    }

    #endregion Unity functions
    
    #region private functions

    private void LoadAfterGameSceneWasLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene [{scene.name}] was loaded");
        if (scene.buildIndex == 1)
        {
            if (uiController == null)
            {
                uiController = FindObjectOfType<UIController>();
                uiController.SetGameController(this);
            }

            if (matchThreeButtons == null)
            {
                matchThreeButtons = FindObjectOfType<MatchThreeButtons>();
            }

            if (upgradeController == null)
            {
                upgradeController = FindObjectOfType<UpgradeController>();
            }

            if (matchThreeControllerV2 == null)
            {
                matchThreeControllerV2 = FindObjectOfType<MatchThreeController_v2>();
            }
            
            SetActions();
        }
    }
    /// <summary>
    /// call only after Game scene was loaded
    /// </summary>
    private void SetActions()
    {
        matchThreeButtons.ButtonClosePanel.GetComponent<Button>().onClick.AddListener(() => uiController.ChangeVisibleState(matchThreeButtons.ButtonClosePanel));
        matchThreeButtons.ButtonOpenPanel.GetComponent<Button>().onClick.AddListener(() => uiController.ChangeVisibleState(matchThreeButtons.ButtonOpenPanel));
        
        upgradeController.SetActionsToDoubleScore((bool value) => matchThreeControllerV2.ChangeDoubleScoreState(true));
        upgradeController.SetActionsToAutoScore((bool value) => matchThreeControllerV2.ChangeAutoScoreState());
    }

    #endregion private functions
}
