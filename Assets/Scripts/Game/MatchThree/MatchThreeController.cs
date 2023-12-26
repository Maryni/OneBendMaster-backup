using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MatchThreeController : MonoBehaviour
{
   #region Inspector variables

   [SerializeField] private float valueLocalScaleConnectedElement;
   [SerializeField] private GameObject gamePanelGameObject;
   [SerializeField] private GameObject panelForHideObjects;

   [Header("Sprites in cells"),SerializeField] private Sprite spriteFire;
   [SerializeField] private Sprite spriteWater;
   [SerializeField] private Sprite spriteEnergy;
   [SerializeField] private Sprite spriteNature;
   [SerializeField] private Sprite spriteMagic;
   [SerializeField] private Sprite spriteClear;

   [SerializeField] private int countConnectedCells;
   [SerializeField] private ElementType elementTypeLastConnections = ElementType.NoElement;
   
   [SerializeField] private GridLayoutGroup group;
   
   #endregion Inspector variables

   #region private variables

   private int columnCount = 6;
   private int lineCount = 6;
   private MatchThreeFlexibleElement[,] arrayObjectsInCell;
   [SerializeField] private List<MatchThreeFlexibleElement> arrayObjectsConnected;
   private int xFirst = -1, xSecond = -1;
   private int yFirst = -1, ySecond = -1;

   #endregion private variables

   #region properties

   public int ColumnCount => columnCount;
   public int LineCount => lineCount;
   public ElementType ElementTypeLastConnections => elementTypeLastConnections;
   public GameObject GamePanel => gamePanelGameObject;
   //public int CountConnectedCellsLastConnection => countConnectedCells + 1;

   #endregion properties

   #region Unity functions

   private void Awake()
   {
      arrayObjectsInCell = new MatchThreeFlexibleElement[columnCount, lineCount];
      arrayObjectsConnected = new List<MatchThreeFlexibleElement>();
   }

   #endregion Unity functions

   #region public functions
   
   public void CheckElementsIfWasRemoved()
   {
      if (arrayObjectsConnected.Count == 1)
      {
         elementTypeLastConnections = ElementType.NoElement;
         arrayObjectsConnected[0].transform.localScale = Vector3.one;
         arrayObjectsConnected.Clear();
      }
   }
   
   public string GetCountConnectedCellsLastConnection()
   {
      int count = 0;
      switch (countConnectedCells + 1)
      {
         case 2: count = 1; break;
         case 3: count = 3; break;
         case 4: count = 5; break;
         case 5: count = 7; break;
         case 6: count = 9; break;
         default: count = countConnectedCells; break;
      }
      return count.ToString();
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
   /// point what we will check with first point (it's second point)
   /// </summary>
   /// <param name="x"></param>
   /// <param name="y"></param>
   public void SetFirstsXY(int x, int y)
   {
      bool haveThisXY = false;
      for (int i = 0; i < arrayObjectsConnected.Count; i++)
      {
         if (arrayObjectsConnected[i].X == x && arrayObjectsConnected[i].Y == y)
         {
            haveThisXY = true;
         }
      }

      if (!haveThisXY)
      {
         xFirst = x;
         yFirst = y; 
      }
   }

   /// <summary>
   /// point from we checking
   /// </summary>
   /// <param name="x"></param>
   /// <param name="y"></param>
   public void SetValuesFromBeginDragPoint(int x, int y)
   {
      //xSecond,ySecond = point OnBeginDrag
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
   
   public void CheckSlideConnectionBetweenOnBeginDragAndOnEndDrag()
   {
      //Debug.Log($"xFirst = {xFirst} | yFirst = {yFirst} | xSecond = {xSecond} | ySecond = {ySecond}");
      if (arrayObjectsInCell[xSecond, ySecond].ElementType == arrayObjectsInCell[xFirst, yFirst].ElementType)
      {
         if (CheckConnectionBetweenPoints(xSecond, ySecond, xFirst, yFirst))
         {
            if (!arrayObjectsConnected.Contains(arrayObjectsInCell[xSecond, ySecond]))
            {
               SetElementToLastConnectionList(xSecond, ySecond);
               SetLastConnectedElementScale(valueLocalScaleConnectedElement);
            }
            
            if (!arrayObjectsConnected.Contains(arrayObjectsInCell[xFirst, yFirst]))
            {
               SetElementToLastConnectionList(xFirst, yFirst);
               SetLastConnectedElementScale(valueLocalScaleConnectedElement);
               
               countConnectedCells++;
               xSecond = xFirst;
               ySecond = yFirst;
            }
            
            if (elementTypeLastConnections != arrayObjectsInCell[xFirst, yFirst].ElementType)
            {
               elementTypeLastConnections = arrayObjectsInCell[xFirst, yFirst].ElementType;
            }
            Debug.Log($"connection YES | countConnectedCells = [{countConnectedCells}] ");
         }
         else
         {
            Debug.Log($"connection NO | countConnectedCells = [{countConnectedCells}] ");
            //Debug.LogWarning($"xFirst = {xFirst} | yFirst = {yFirst} | xSecond = {xSecond} | ySecond = {ySecond}");
         }
      }
      else
      {
         Debug.Log("ElementType not the same");
         Debug.LogWarning($"arrayObjectsInCell[{xSecond}, {ySecond}].ElementType = {arrayObjectsInCell[xSecond, ySecond].ElementType} | arrayObjectsInCell[{xFirst}, {yFirst}].ElementType = {arrayObjectsInCell[xFirst, yFirst].ElementType} ");
      }
   }

   public void ClearCountConnected()
   {
      countConnectedCells = 0;
      arrayObjectsConnected.Clear();
      xFirst = yFirst = xSecond = ySecond = -1;
   }
   
   public void SetConnectedCellsToOtherRandomElement()
   {
      for (int i = 0; i < arrayObjectsConnected.Count; i++)
      {
         arrayObjectsConnected[i].gameObject.transform.localScale = Vector3.one;
         ElementType tempType = GetRandomElementType();
         arrayObjectsConnected[i].SetElementType(tempType);
         arrayObjectsInCell[arrayObjectsConnected[i].X,arrayObjectsConnected[i].Y].SetElementType(tempType);
      }
      SetSpriteForElementType();
      elementTypeLastConnections = ElementType.NoElement;
   }
   
   public void CheckAndRemoveLastElementInConnectedList(int x, int y)
   {
      if(arrayObjectsConnected.Count > 1)
      {
         if (x == arrayObjectsConnected[arrayObjectsConnected.Count - 2].X &&
             y == arrayObjectsConnected[arrayObjectsConnected.Count - 2].Y)
         {
            var temp = arrayObjectsConnected[arrayObjectsConnected.Count - 1];
            temp.gameObject.transform.localScale = Vector3.one;
            arrayObjectsConnected.Remove(temp);
            countConnectedCells -= 1;
            xSecond = arrayObjectsConnected[arrayObjectsConnected.Count - 1].X;
            ySecond = arrayObjectsConnected[arrayObjectsConnected.Count - 1].Y;
         }
      }
   }

   #endregion public functions

   #region private functions
   
   private void SetLastConnectedElementScale(float value)
   {
      arrayObjectsConnected[arrayObjectsConnected.Count - 1].gameObject.transform.localScale =
         new Vector3(value, value, value);
   }
   
   private void SetElementToLastConnectionList(int xValue, int yValue)
   {
      var tempObject = arrayObjectsInCell[xValue, yValue];
      if (!arrayObjectsConnected.Contains(tempObject))
      {
         arrayObjectsConnected.Add(tempObject);
         //Debug.Log($"[LastElementAdded] arrayObjectsConnected[{arrayObjectsConnected.Count - 1}].X = {arrayObjectsConnected[arrayObjectsConnected.Count - 1].X} | arrayObjectsConnected[0].Y = {arrayObjectsConnected[arrayObjectsConnected.Count - 1].Y} ");
      }
      else
      {
         Debug.Log($"[LastElementAdded] arrayObjectsConnected have object");
      }
   }

   private bool CheckConnectionBetweenPoints(int x1, int y1, int x2, int y2)
   {
      if (x1 + 1 == x2)
      {
         if (y1 + 1 == y2)
         {
            return true;
         }
         else if (y1 - 1 == y2)
         {
            return true;
         }
         else if (y1 == y2)
         {
            return true;
         }
         else
         {
            return false;
         }
      }

      if (x1 == x2)
      {
         if (y1 + 1 == y2)
         {
            return true;
         }
         else if (y1 - 1 == y2)
         {
            return true;
         }
         else
         {
            return false;
         }
      }
      
      if (x1 - 1 == x2)
      {
         if (y1 + 1 == y2)
         {
            return true;
         }
         else if (y1 - 1 == y2)
         {
            return true;
         }
         else if (y1 == y2)
         {
            return true;
         }
         else
         {
            return false;
         }
      }
      return false;
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

   private void SetRandomElementsToCells(bool needToChangeAllCells = true)
   {
      for (int i = 0; i < lineCount; i++)
      {
         for (int j = 0; j < columnCount; j++)
         {
            var tempRandomElementType = GetRandomElementType();
            if (needToChangeAllCells)
            {
               arrayObjectsInCell[i,j].SetElementType(tempRandomElementType);
            }
            else
            {
               if (arrayObjectsInCell[i, j].ElementType == ElementType.NoElement)
               {
                  tempRandomElementType = arrayObjectsInCell[i, j].ElementType;
                  arrayObjectsInCell[i,j].SetElementType(tempRandomElementType);
               }
            }
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

   private void CheckIndexesAndSetRandomElement()
   {
      CheckPanelAndAddAllToList();
      SetRandomElementsToCells();
   }

   private ElementType GetRandomElementType()
   {
      return (ElementType) Random.Range(1, System.Enum.GetValues(typeof(ElementType)).Length);
   }

   private void SetSpriteForElementType()
   {
      for (int i = 0; i < lineCount; i++)
      {
         for (int j = 0; j < columnCount; j++)
         {
            switch (arrayObjectsInCell[i,j].ElementType)
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

   #endregion private functions

}
