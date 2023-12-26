using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIActions : MonoBehaviour
{

    #region Inspector variables

    [SerializeField] private GameObject continueGameObject;
    [SerializeField] private GameObject chooseLevelGameObject;
    [SerializeField] private GameObject upgradesGameObject;
    [SerializeField] private GameObject settingsGameObject;
    [SerializeField] private GameObject exitGameObject;
    [SerializeField] private GameObject mainPanelGameObject;
    [SerializeField] private GameObject MatchThreePanel;

    #endregion Inspector variables
    
    
    #region public functions

    public void OpenContinue()
    {
        HideAll();
        ShowObject(continueGameObject);
    }

    public void OpenChooseLevel()
    {
        HideAll();
        ShowObject(chooseLevelGameObject);
    }

    public void OpenUpgrades()
    {
        HideAll();
        ShowObject(upgradesGameObject);
    }

    public void OpenSettings()
    {
        HideAll();
        ShowObject(settingsGameObject);
    }

    public void OpenMatchThreePanel()
    {
        HideAll();
        MatchThreePanel.SetActive(true);
    }
    
    public void HideAll()
    {
        continueGameObject.SetActive(false);
        chooseLevelGameObject.SetActive(false);
        upgradesGameObject.SetActive(false);
        settingsGameObject.SetActive(false);
        exitGameObject.SetActive(false);
        MatchThreePanel.SetActive(false);
    }

    public void HideAllAndHideMainPanel()
    {
        HideAll();
        mainPanelGameObject.SetActive(false);
    }

    public void ShowObject(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }
    
    public void ExitApplication()
    {
        Application.Quit();
    }

    public void ContinueLevel()
    {
        SceneManager.LoadSceneAsync(1);
    }
    
    #endregion public functions


}
