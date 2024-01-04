using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class MatchThreeController_v2 : MonoBehaviour
{
    #region Inspector variables

    [SerializeField] private float valueLocalScaleConnectedElement;
    [SerializeField] private GameObject gamePanelGameObject;
    [SerializeField] private GameObject panelForHideObjects;
    [SerializeField] private TMP_Text text;

    [Header("Sprites in cells"),SerializeField] private Sprite spriteFire;
    [SerializeField] private Sprite spriteWater;
    [SerializeField] private Sprite spriteEnergy;
    [SerializeField] private Sprite spriteNature;
    [SerializeField] private Sprite spriteMagic;
    [SerializeField] private Sprite spriteClear;

    [SerializeField] private int countConnectedCells;
    [SerializeField] private ElementType elementTypeLastConnections = ElementType.NoElement;
    [SerializeField] private List<MatchThreeFlexibleElement> arrayObjectsSelected;
    [SerializeField] private float timerValue;

    #endregion Inspector variables

    #region private variables

    private int columnCount = 8;
    private int lineCount = 8;
    private MatchThreeFlexibleElement[,] arrayObjectsInCell;
    private int xFirst = -1, xSecond = -1;
    private int yFirst = -1, ySecond = -1;
    private int gameScore = 0;
    private Action onCombinationSuccess;
    private int gameScoreMod = 1;
    private Coroutine autoScoreCoroutine;

    #endregion private variables

    #region properties

    public int ColumnCount => columnCount;
    public int LineCount => lineCount;
    public ElementType ElementTypeLastConnections => elementTypeLastConnections;
    public GameObject GamePanel => gamePanelGameObject;
    public int GameScore => gameScore;
    public TMP_Text Text => text;

    #endregion properties
    
    #region Unity functions

    private void Awake()
    {
        arrayObjectsInCell = new MatchThreeFlexibleElement[columnCount, lineCount];
        arrayObjectsSelected = new List<MatchThreeFlexibleElement>();
    }

    #endregion Unity functions

    #region public functions

    public void ChangeAutoScoreState()
    {
        if (autoScoreCoroutine == null)
        {
            autoScoreCoroutine = StartCoroutine(AutoScore());
        }
        else
        {
            StopCoroutine(autoScoreCoroutine);
            autoScoreCoroutine = null;
        }
    }

    private IEnumerator AutoScore()
    {
        yield return new WaitForSeconds(timerValue);
        IncreaseGameScore();
        yield return AutoScore();
    }
    
    public void ChangeDoubleScoreState(bool value) => gameScoreMod = value ? 2 : 1;

    public void CheckSlideConnectionBetweenOnBeginDragAndOnEndDrag()
    {
        Debug.Log($"xFirst = {xFirst} | yFirst = {yFirst} | xSecond = {xSecond} | ySecond = {ySecond}");
        if ((xFirst != -1 && yFirst != -1) && (xSecond != -1 && ySecond != -1))
        {
            SwapValues(ref xSecond, ref ySecond, ref xFirst, ref yFirst, true);
            ChangeElementsInArray();

            if (CheckCombinationForElement(xSecond, ySecond))
            {
                IncreaseGameScore();
                ChangeSelectedElements();
                arrayObjectsSelected.Clear();
                onCombinationSuccess?.Invoke();
                return;
            }
            else
            {
                SwapValues(ref xSecond, ref ySecond, ref xFirst, ref yFirst, true);
                ChangeElementsInArray();
                Debug.Log("FALSE check combinations");
            }
        }

    }

    //fall after connect
    private void ChangeSelectedElements()
    {
        foreach(var element in arrayObjectsSelected)
        {
            SetRandomElementToCell(element);
        }
    }

    private void SwapValues(ref int x1, ref int y1, ref int x2, ref int y2, bool needToResetXY = false)
    {
        var xTemp = 0;
        var yTemp = 0;
        xTemp = x1;
        yTemp = y1;
        x1 = x2;
        y1 = y2;
        x2 = xTemp;
        y2 = yTemp;

        if(needToResetXY)
        {
            xTemp = arrayObjectsInCell[x2, y2].X;
            yTemp = arrayObjectsInCell[x2, y2].Y;
            arrayObjectsInCell[x2, y2].SetX(arrayObjectsInCell[x1, y1].X);
            arrayObjectsInCell[x2, y2].SetY(arrayObjectsInCell[x1, y1].Y);
            arrayObjectsInCell[x1, y1].SetX(xTemp);
            arrayObjectsInCell[x1, y1].SetY(yTemp);
        }
    }

    public void SetObjectToPanel(params GameObject[] gameObjects)
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].transform.SetParent(gamePanelGameObject.transform);
        }
        CheckIndexesAndSetRandomElement();
    }

    public void HidePanel()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// point what we will check with first point
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetFirstsXY(int x, int y)
    {
        xFirst = x;
        yFirst = y;
    }

    /// <summary>
    /// point from we checking
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetValuesFromBeginDragPoint(int x, int y)
    {
        //x,ySecond = point OnDrag
        xSecond = x;
        ySecond = y;
    }

    /// <summary>
    /// From beginDrag to EndDrag (from xSecond to xFirst, and then xFirst to xSecond)
    /// </summary>
    public void ChangeElementsInArray()
    {
        ElementType tempType = arrayObjectsInCell[xSecond, ySecond].ElementType;
        Sprite tempSprite = arrayObjectsInCell[xSecond,ySecond].Sprite;
        arrayObjectsInCell[xSecond,ySecond].SetElementType(arrayObjectsInCell[xFirst,yFirst].ElementType);
        arrayObjectsInCell[xFirst,yFirst].SetElementType(tempType);
        arrayObjectsInCell[xSecond,ySecond].SetSprite(arrayObjectsInCell[xFirst,yFirst].Sprite);
        arrayObjectsInCell[xFirst,yFirst].SetSprite(tempSprite);
    }

    public void SetActionsOnSuccessCombination(params Action[] actions)
    {
        foreach(var item in actions)
        {
            onCombinationSuccess += item;
        }
    }

    public void ResetAllCells() => SetRandomElementsToCells();

    #endregion public functions

    #region private functions

    private void IncreaseGameScore() => gameScore += gameScoreMod;
    
    private bool CheckCombinationForElement(int x, int y)
    {
        Debug.Log($"Checking [{x}|{y}], elementType = {arrayObjectsInCell[x,y].ElementType}");
        var maxIndexX = lineCount -1;
        var maxIndexY = columnCount -1;

        var currX = x;
        var currY = y;

        var lineValue = 0;
        var columnValue = 0;
        
        for (int i = 0; i < maxIndexY; i++)
        {
            if (arrayObjectsInCell[currX, i].ElementType == arrayObjectsInCell[currX, currY].ElementType)
            {
                columnValue++;
                arrayObjectsSelected.Add(arrayObjectsInCell[currX, i]);
            }
        }

        if (columnValue < 3)
        {
            arrayObjectsSelected.Clear();
        } 
        else
        {
            Debug.Log($"YEP, u have columnCount [{columnValue}]");
            return true;
        }
        
        for (int i = 0; i < maxIndexX; i++)
        {
            if (arrayObjectsInCell[i, currY].ElementType == arrayObjectsInCell[currX, currY].ElementType)
            {
                lineValue++;
                arrayObjectsSelected.Add(arrayObjectsInCell[i, currY]);
            }
        }
        
        if (lineValue < 3)
        {
            arrayObjectsSelected.Clear();
        }
        else
        {
            Debug.Log($"YEP, u have lineCount [{lineValue}]");
            return true;
        }
        
        return false;
    }

    private void CheckIndexesAndSetRandomElement()
    {
        CheckPanelAndAddAllToList();
        SetRandomElementsToCells();
    }
    
    private void CheckPanelAndAddAllToList()
    {
        MatchThreeFlexibleElement[,] tempObjects = new MatchThreeFlexibleElement[lineCount,columnCount];
        for (int i = 0; i < lineCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                tempObjects[i,j] = gamePanelGameObject.transform.GetChild((lineCount*i) + j).GetChild(0).GetComponent<MatchThreeFlexibleElement>();  
            }
        }

        for (int i = 0; i < lineCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                arrayObjectsInCell[i, j] = tempObjects[i,j];
                arrayObjectsInCell[i, j].SetX(i);
                arrayObjectsInCell[i, j].SetY(j);
            }
        }
    }

    private void SetRandomElementsToCells()
    {
        for (int i = 0; i < lineCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                var tempRandomElementType = (ElementType) Random.Range(1,System.Enum.GetValues(typeof(ElementType)).Length);
                arrayObjectsInCell[i,j].SetElementType(tempRandomElementType);
                switch (tempRandomElementType)
                {
                    case ElementType.Fire: arrayObjectsInCell[i,j].SetSprite(spriteFire); break;
                    case ElementType.Water: arrayObjectsInCell[i,j].SetSprite(spriteWater); break;
                    case ElementType.Nature: arrayObjectsInCell[i,j].SetSprite(spriteNature); break;
                    case ElementType.Energy: arrayObjectsInCell[i,j].SetSprite(spriteEnergy); break;
                    case ElementType.Magic: arrayObjectsInCell[i,j].SetSprite(spriteMagic); break;
                    default: arrayObjectsInCell[i,j].SetSprite(spriteClear); break;
                }
            }
        }
    }

    private void SetRandomElementToCell(MatchThreeFlexibleElement element)
    {
        var tempRandomElementType = (ElementType)Random.Range(1, System.Enum.GetValues(typeof(ElementType)).Length);
        element.SetElementType(tempRandomElementType);
        switch (tempRandomElementType)
        {
            case ElementType.Fire: element.SetSprite(spriteFire); break;
            case ElementType.Water: element.SetSprite(spriteWater); break;
            case ElementType.Nature: element.SetSprite(spriteNature); break;
            case ElementType.Energy: element.SetSprite(spriteEnergy); break;
            case ElementType.Magic: element.SetSprite(spriteMagic); break;
            default: element.SetSprite(spriteClear); break;
        }
    }
    
    #endregion private functions   
}
