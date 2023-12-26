using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using  System.Linq;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

public class  BulletsController : MonoBehaviour
{
    #region Inspector variables

    [SerializeField] private List<GameObject> spriteBulletsList;
    [SerializeField] private GameObject exampleBulletSprite;
    [SerializeField] private Transform parentBulletSpritePanel;
    [Header("Elemental Sprites"),SerializeField] private Sprite colorFireElemental;
    [SerializeField] private Sprite colorWaterElemental;
    [SerializeField] private Sprite colorEnergyElemental;
    [SerializeField] private Sprite colorNatureElemental;
    [SerializeField] private Sprite colorMagicElemental;
    [SerializeField] private Sprite colorDefaultElemental;

    #endregion Inspector variables

    #region private variables

    private GameObject poolBullets;
    [SerializeField] private int countAvaliablePlaceForBullet;
    private UnityAction actionWhenAllBulletsAreColored;

    #endregion private variables

    #region properties

    public int CountCurrentBullet => spriteBulletsList.Count;

    #endregion properties

    #region Unity functions

    private void Awake()
    {
        if (poolBullets == null)
        {
            poolBullets = transform.GetChild(0).gameObject;
        }

        AddAllBulletsToList();
        gameObject.SetActive(false); 
    }

    #endregion Unity functions

    #region public functions

    public ElementType GetFirstBulletElementType()
    {
        var tempObject = spriteBulletsList.FirstOrDefault(x=>x.GetComponent<BulletSprite>().ElementType != ElementType.NoElement)?.GetComponent<BulletSprite>();
        if (tempObject != null)
        {
            var tempType = tempObject.ElementType;
            //Debug.Log($"TempType returned, tempType = {tempType}");
            return tempType;
        }
        else
        {
            Debug.Log($"[GetLastBulletElementType] no sprite in list where element != no element");
            return ElementType.NoElement;
        }
 }

    public void SetLastBulletColorByType(ElementType elementType)
    {
        SetBulletColorByType(elementType,spriteBulletsList.Count-1);
    }

    public void SetBulletColorByTypeAndIndex(ElementType elementType, int index)
    {
        SetBulletColorByType(elementType, index);
    }

    public string GetBulletTextWhichLastUnzero()
    {
        string temp = "0";
        if(countAvaliablePlaceForBullet > 0)
        {
            for (int i = spriteBulletsList.Count - 1; i >= 0; i--)
            {
                var currentElement = spriteBulletsList[i].GetComponent<BulletSprite>();
                if (currentElement.TextOnBullet != "0")
                {
                    temp = currentElement.TextOnBullet;
                    break;
                }
            }

            if (temp == "0")
            {
                //Debug.LogWarning($"There is no bullets without 0 in text");
            }
        }

        return temp;
    }
    
    public void SetBulletTextLesserByOneForFirstUnzero()
    {
        for (int i = 0; i < spriteBulletsList.Count; i++)
        {
            var currentElement = spriteBulletsList[i].GetComponent<BulletSprite>();
            if (currentElement.TextOnBullet != "0" && currentElement.ElementType != ElementType.NoElement)
            {
                int currentText = int.Parse(currentElement.TextOnBullet);
                currentText -= 1;
                currentElement.SetText(currentText.ToString());
                if (currentText == 0)
                {
                    currentElement.SetElementType(ElementType.NoElement);
                }
                break;
            }
        }
    }

    public void CheckBulletsForZero()
    {
        for (int i = 0; i < spriteBulletsList.Count; i++)
        {
            var currentElement = spriteBulletsList[i].GetComponent<BulletSprite>();
            if (currentElement.TextOnBullet == "0")
            {
                currentElement.SetElementType(ElementType.NoElement);
                currentElement.SetImageSprite(colorDefaultElemental);
            }
        }
    }
    
    public void SetBulletTextByIndex(int index, string value)
    {
        SetTextByIndex(index, value);
    }

    public void SetBulletTextForLastBullet(string value)
    {
        SetTextByIndex(spriteBulletsList.Count - 1, value);
    }

    public void SetBulletTextForFirstNonElementBullet(string value)
    {
        if (countAvaliablePlaceForBullet > 0)
        {
            var temp = spriteBulletsList.FirstOrDefault(x =>
                x.GetComponent<BulletSprite>().ElementType == ElementType.NoElement);
            temp.GetComponent<BulletSprite>().SetText(value);
        }
    }

    public void SetBulletColorForFirstBulletWithoutColor(ElementType typeColor)
    {
        //Debug.Log($"countAvaliablePlaceForBullet = {countAvaliablePlaceForBullet}");
        if (countAvaliablePlaceForBullet > 0)
        {
            BulletSprite bullet;
            bullet = spriteBulletsList.FirstOrDefault(x => x.GetComponent<BulletSprite>().ElementType == ElementType.NoElement)?.GetComponent<BulletSprite>();
            if (typeColor == ElementType.Fire)
            {
                bullet.SetImageSprite(colorFireElemental);
            }
            
            if (typeColor == ElementType.Water)
            {
                bullet.SetImageSprite(colorWaterElemental);
            }
            
            if (typeColor == ElementType.Energy)
            {
                bullet.SetImageSprite(colorEnergyElemental);
            }
            
            if (typeColor == ElementType.Nature)
            {
                bullet.SetImageSprite(colorNatureElemental);
            }
            
            if (typeColor == ElementType.Magic)
            {
                bullet.SetImageSprite(colorMagicElemental);
            }
            
            bullet.SetElementType(typeColor);
            
            if (countAvaliablePlaceForBullet - 1 == 0)
            {
                actionWhenAllBulletsAreColored?.Invoke();
            }

            if (typeColor != ElementType.NoElement)
            {
                countAvaliablePlaceForBullet--;  
            }
        }
        else
        {
            Debug.LogWarning($"CountAvalibleBullets for create new bullets = 0");
        }
    }

    /// <summary>
    /// for add default bullet dont write #1 parameter in function
    /// </summary>
    /// <param name="elementType"></param>
    /// <param name="value"></param>
    public void AddBulletByType(ElementType elementType = ElementType.NoElement, string value = "1")
    {
        var tempObject = Instantiate(exampleBulletSprite, parentBulletSpritePanel);
        spriteBulletsList.Add(tempObject);
        SetLastBulletColorByType(elementType);
        SetTextByIndex(spriteBulletsList.Count-1,value);
        
    }

    public void SetAvalibleCountBullets()
    {
        for (int i = 0; i < poolBullets.transform.childCount; i++)
        {
            if (poolBullets.transform.GetChild(i).GetComponent<BulletSprite>().ElementType == ElementType.NoElement)
            {
                countAvaliablePlaceForBullet++;
            }
        }
    }

    public void SetActionWhenAllBulletsColored(params UnityAction[] actions)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actionWhenAllBulletsAreColored += actions[i];
        }
    }
    
    #endregion public functions

    #region private functions
    
    private void AddAllBulletsToList()
    {
        for (int i = 0; i < parentBulletSpritePanel.childCount; i++)
        {
            var tempObject = parentBulletSpritePanel.GetChild(i).gameObject;
            if (!spriteBulletsList.Contains(tempObject))
            {
                spriteBulletsList.Add(tempObject);  
            }
        }
    }

    private void SetBulletColorByType(ElementType elementType, int index)
    {
        switch (elementType)
        {
            case ElementType.Fire:  spriteBulletsList[index].GetComponent<BulletSprite>().SetImageSprite(colorFireElemental); break;
            case ElementType.Water:  spriteBulletsList[index].GetComponent<BulletSprite>().SetImageSprite(colorWaterElemental); break;
            case ElementType.Energy:  spriteBulletsList[index].GetComponent<BulletSprite>().SetImageSprite(colorEnergyElemental); break;
            case ElementType.Nature:  spriteBulletsList[index].GetComponent<BulletSprite>().SetImageSprite(colorNatureElemental); break;
            case ElementType.Magic:  spriteBulletsList[index].GetComponent<BulletSprite>().SetImageSprite(colorMagicElemental); break;
            default: spriteBulletsList[index].GetComponent<BulletSprite>().SetImageSprite(colorDefaultElemental); break;
        }
        spriteBulletsList[index].GetComponent<BulletSprite>().SetElementType(elementType);
    }

    private void SetTextByIndex(int index, string value)
    {
        spriteBulletsList[index].GetComponent<BulletSprite>().SetText(value);
    }

    #endregion private functions
}
