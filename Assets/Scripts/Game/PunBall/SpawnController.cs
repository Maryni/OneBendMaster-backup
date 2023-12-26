using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class SpawnController : MonoBehaviour
{
    #region private variables

    private GameController gameController;
    private List<PunBallCellsIndex> listCells;
    [SerializeField] private List<GameObject> listAllSpawned = new List<GameObject>();
    [SerializeField] private List<GameObject> listLastWaveSpawned = new List<GameObject>();
    private UnityAction actionAfterSpawn;

    #endregion private variables

    #region properties

    public List<GameObject> LastWaveSpawnedList => listLastWaveSpawned;

    #endregion properties

    #region Unity functions

    private void Start()
    {
        SetVariables();
        Spawn();
    }

    #endregion Unity functions

    #region public functions

    public void Spawn()
    {
        Debug.Log($"[Spawn] WaveIndex = {gameController.WaveIndex}");
        int countEnemiesByFire = gameController.WaveData.GetCountEnemiesByIndex(ElementType.Fire);
        int countEnemiesByWater = gameController.WaveData.GetCountEnemiesByIndex(ElementType.Water);
        int countEnemiesByEnergy = gameController.WaveData.GetCountEnemiesByIndex(ElementType.Energy);
        int countEnemiesByNature = gameController.WaveData.GetCountEnemiesByIndex(ElementType.Nature);
        int countEnemiesByMagic = gameController.WaveData.GetCountEnemiesByIndex(ElementType.Magic);
        
        SpawnByTypeAndCount(ElementType.Fire,countEnemiesByFire);
        SpawnByTypeAndCount(ElementType.Water,countEnemiesByWater);
        SpawnByTypeAndCount(ElementType.Energy,countEnemiesByEnergy);
        SpawnByTypeAndCount(ElementType.Nature,countEnemiesByNature);
        SpawnByTypeAndCount(ElementType.Magic,countEnemiesByMagic);
        actionAfterSpawn?.Invoke();
    }

    public void SetActionAfterSpawn(params UnityAction[] actions)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actionAfterSpawn += actions[i];
        }
    }

    public void MovePreviousEnemyForward()
    {
        var fullList = gameController.PunBallPoolCells.GetCellsList(true);

        for (int i = 0; i < listAllSpawned.Count; i++)
        {
            var currentCell = fullList.LastOrDefault(x=>x.IsOcupied && !x.MovedInThisWave);
            var currentX = currentCell.X;
            var currentY = currentCell.Y;
            Debug.Log($"currentX = {currentX} | currentY = {currentY}");
            if (currentY != gameController.PunBallPoolCells.countY - 1)
            {
                var tempObjectNew = fullList.FirstOrDefault(x => x.X == currentX && x.Y == currentY + 1  && !x.MovedInThisWave);
                var tempObjectCurrent = fullList.FirstOrDefault(x => x.X == currentX && x.Y == currentY);

                listAllSpawned[i].transform.SetParent(tempObjectNew.gameObject.transform);
                listAllSpawned[i].transform.localPosition = new Vector3(0, 1.5f, 0f);
                tempObjectCurrent.ChangeOcupiedState();
                tempObjectNew.ChangeOcupiedState();
                tempObjectNew.ChangeMovedState();
                Debug.Log($"newObject X = {tempObjectNew.X} | newObject Y = {tempObjectNew.Y} ");
            }
        }
        
        for (int i = 0; i < fullList.Count; i++)
        {
            if (fullList[i].GetComponent<PunBallCellsIndex>().MovedInThisWave)
            {
                fullList[i].GetComponent<PunBallCellsIndex>().ChangeMovedState();  
            }
        }
        
    }

    #endregion public functions
    
    #region private functions

    private void SetVariables()
    {
        if (gameController == null)
        {
            gameController = GetComponent<GameController>();
        }
        listCells = gameController.PunBallPoolCells.GetCellsList(false);
    }

    private void SpawnByTypeAndCount(ElementType elementType, int count)
    {
        if (count > 0)
        {
            List<GameObject> tempList = new List<GameObject>();
            for (int i = 0; i < count; i++)
            {
                tempList.Add(gameController.ObjectPool.GetObjectByType(ObjectType.Enemy, elementType));
                tempList[i].SetActive(true);
                
                int index = GetRandomInt();
                PunBallCellsIndex tempObject;
        
                if (!listCells[index].IsOcupied)
                {
                    tempObject = listCells[index];
                    tempObject.ChangeOcupiedState();
                }
                else
                {
                    tempObject =  listCells.FirstOrDefault(x => !x.IsOcupied);
                    tempObject.ChangeOcupiedState();
                }
                tempList[i].transform.SetParent(tempObject.gameObject.transform);
                tempList[i].transform.localPosition = new Vector3(0,1.5f,0f);
                listLastWaveSpawned.Add(tempList[i]);
            }
            if (listLastWaveSpawned.Count >= 0)
            {
                SetObjectFromFirstListToSecond(listLastWaveSpawned,listAllSpawned,true);
            }
        }
        else
        {
            Debug.LogWarning($"[SpawnWarning] ElemType [{elementType}] count [{count}]");
        }
    }
    
    private int GetRandomInt()
    {
        return Random.Range(0,listCells.Count-1);
    }

    private void SetObjectFromFirstListToSecond(List<GameObject> a, List<GameObject> b, bool clearFirstList = false)
    {
        //Debug.Log($"a.count = {a.Count} | b.count = {b.Count}");
        for (int i = 0; i < a.Count; i++)
        {
            b.Add(a[i]);
        }
        if (clearFirstList)
        {
            a.Clear();
        }
        //Debug.Log($"a.count = {a.Count} | b.count = {b.Count}");
    }
    
    #endregion private functions
}
