using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum ObjectType
{
    Bullet,
    Enemy,
    MatchThreeSprite
}
public class ObjectPool : MonoBehaviour
{
    #region Inspector variables

    [Header("Prefabs for Init"),SerializeField] private List<GameObject> prefabsBulletList;
    [SerializeField] private List<GameObject> prefabsEnemyList;
    [SerializeField] private List<GameObject> prefabsSpriteList;
    [Header("Inited objects"),SerializeField] private List<GameObject> exampleBulletList;
    [SerializeField] private List<GameObject> exampleEnemyList;
    [SerializeField] private List<GameObject> exampleSpriteList;

    [Header("Transform for pools"),SerializeField] private Transform transformBulletParent;
    [SerializeField] private Transform transformEnemyParent;
    [SerializeField] private Transform transformSpriteParent;

    [Header("Count inited object for each type"),SerializeField] private int countBulletsExampleToInit;
    [SerializeField] private int countEnemyExampleToInit;
    [SerializeField] private int countSpriteExampleToInit;

    #endregion Inspector variables

    #region private variables

    private bool isStatsLoaded = false;

    #endregion private variables

    #region properties

    public List<GameObject> PrefabsBulletList => prefabsBulletList;
    public List<GameObject> PrefabsEnemyList => prefabsEnemyList;
    public List<GameObject> PrefabsSpriteList => prefabsSpriteList;

    #endregion properties
    
    #region Unity functions

    private void Start()
    {
       StartCoroutine(Init());
    }

    #endregion Unity functions
    
    #region public functions

    public GameObject GetObjectByType(ObjectType objectType, ElementType elementType)
    {
        if (objectType == ObjectType.Bullet)
        {
           var findedObject = exampleBulletList.Where(x => x.GetComponent<BaseBullet>().ElementType == elementType)
               .FirstOrDefault(x => !x.activeSelf);
           if (findedObject == null)
           {
               var exampleObject =
                   exampleBulletList.FirstOrDefault(x => x.GetComponent<BaseBullet>().ElementType == elementType);
               var newObject = Instantiate(exampleObject, transformBulletParent);
               exampleBulletList.Add(newObject);
               newObject.SetActive(true);
               return newObject;
           }
           findedObject.SetActive(true);
           return findedObject;
        }
        if (objectType == ObjectType.Enemy)
        {
            var findedObject = exampleEnemyList.Where(x => x.GetComponent<BaseEnemy>().ElementType == elementType)
                .FirstOrDefault(x => !x.activeSelf);
            if (findedObject == null)
            {
                var exampleObject =
                    exampleEnemyList.FirstOrDefault(x => x.GetComponent<BaseEnemy>().ElementType == elementType);
                var newObject = Instantiate(exampleObject, transformEnemyParent);
                exampleEnemyList.Add(newObject);
                newObject.SetActive(true);
                return newObject;
            }
            findedObject.SetActive(true);
            return findedObject;
        }
        if (objectType == ObjectType.MatchThreeSprite)
        {
            var findedObject = exampleSpriteList.Where(x => x.GetComponentInChildren<BaseMatchThree>().ElementType == elementType)
                .FirstOrDefault(x => !x.activeSelf);
            if (findedObject == null)
            {
                var exampleObject =
                    exampleSpriteList.FirstOrDefault(x => x.GetComponentInChildren<BaseMatchThree>().ElementType == elementType);
                var newObject = Instantiate(exampleObject, transformSpriteParent);
                exampleSpriteList.Add(newObject);
                newObject.SetActive(true);
                return newObject;
            }
            findedObject.SetActive(true);
            return findedObject;
        }

        Debug.LogError("Incorrect Function GetObjectByType Work");
        return new GameObject();
    }

    public void ChangeStatsLoadState()
    {
        isStatsLoaded = !isStatsLoaded;
    }

    #endregion public functions

    #region private functions

    private IEnumerator Init()
    {
        if (isStatsLoaded)
        {
            InitDefault(prefabsBulletList,countBulletsExampleToInit,transformBulletParent,exampleBulletList);
            InitDefault(prefabsEnemyList,countEnemyExampleToInit,transformEnemyParent, exampleEnemyList);
            InitDefault(prefabsSpriteList, countSpriteExampleToInit, transformSpriteParent, exampleSpriteList);
        }
        else
        {
            yield return new WaitForEndOfFrame();
            yield return Init();
        }

    }

    private void InitDefault(List<GameObject> list, int countGameObjectToInit, Transform transformParent, List<GameObject> exampleList)
    {
        for (int i = 0; i < countGameObjectToInit; i++)
        {
            for (int j = 0; j < list.Count; j++)
            {
                var obj = Instantiate(list[j], transformParent);
                obj.SetActive(false);
                exampleList.Add(obj);
            }
        } 
    }

    #endregion private functions

}
