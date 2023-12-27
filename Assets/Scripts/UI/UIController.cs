using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    #region Inspector variables

    [SerializeField] private GameController gameController;
    [SerializeField] private ObjectPool objectPool;
    [SerializeField] private MatchThreeController_v2 controller;
    [SerializeField] private MatchThreeButtons matchThreeButtons;
    [SerializeField] private GameObject buttonOpen;
    [SerializeField] private GameObject buttonClose;

    #endregion Inspector variables

    #region Unity functions

    private void Start()
    {
        SceneManager.sceneLoaded += SetActionWhenSceneLoaded;
    }

    #endregion Unity functions

    #region public functions

    public void SetGameController(GameController controller)
    {
        gameController = controller;
    }

    #endregion public function
    
    #region private functions

    private void SetActionWhenSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 1)
        { 
            StartCoroutine(SetObjectsFromPoolToMatchThreePanel());
        }
    }
    
    private IEnumerator SetObjectsFromPoolToMatchThreePanel()
    {
        yield return new WaitForEndOfFrame();
        var tempCount = controller.ColumnCount * controller.LineCount;
        GameObject[] tempArray = new GameObject[tempCount];
        for (int i = 0; i < tempCount; i++)
        {
            tempArray[i] = objectPool.GetObjectByType(ObjectType.MatchThreeSprite, ElementType.NoElement);
            tempArray[i].SetActive(true);
            var dragDrop = tempArray[i].GetComponentInChildren<DragDrop>();
            dragDrop.SetActionOnDragWithParams(controller.SetValuesFromBeginDragPoint);
            dragDrop.SetActionOnEndDrag(controller.SetFirstsXY);
            dragDrop.SetActionCheckConnection(() => controller.CheckSlideConnectionBetweenOnBeginDragAndOnEndDrag());
        }
        matchThreeButtons.ButtonRecolorPanel.GetComponent<Button>().onClick.AddListener(controller.ResetAllCells);
        controller.SetObjectToPanel(tempArray);
        controller.HidePanel();
    }

    public void ChangeVisibleState(GameObject item) => item.SetActive(!gameObject.activeSelf);

    #endregion private functions
}
