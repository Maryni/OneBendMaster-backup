using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatsController : MonoBehaviour
{
   #region private variables

   private SaveLoadController saveLoadController;
   private ObjectPool objectPool;
   private List<GameObject> lastWaveSpawned;
   private GameController gameController;

   #endregion private variables

   #region Unity functions

   private void Start()
   {
      SetVariables();
      GetAndSetStatsFromDefaultData();
   }

   #endregion Unity functions

   #region public functions

   public void SetLastWaveSpawnedList(List<GameObject> gameObjectsList)
   {
      lastWaveSpawned = gameObjectsList;
   }

   public void SetStatsToSpawnedEnemy()
   {
      GetAndSetStatsFromWaveData();
   }

   public void SetGameController(GameController gameController)
   {
      this.gameController = gameController;
   }

   #endregion public functions
   
   #region private functions

   private void SetVariables()
   {
      if (objectPool == null)
      {
         objectPool = FindObjectOfType<ObjectPool>();
      }

      if (saveLoadController == null)
      {
         saveLoadController = GetComponent<SaveLoadController>();
      }
   }

   private void GetAndSetStatsFromWaveData()
   {
      var tempFire = gameController.WaveData.GetListStats(ElementType.Fire);
      var tempWater = gameController.WaveData.GetListStats(ElementType.Water);
      var tempEnergy = gameController.WaveData.GetListStats(ElementType.Energy);
      var tempNature = gameController.WaveData.GetListStats(ElementType.Nature);
      var tempMagic = gameController.WaveData.GetListStats(ElementType.Magic);
      if (tempFire[0] == tempWater[0] &&
          tempWater[0] == tempNature[0] &&
          tempNature[0] == tempEnergy[0] &&
          tempEnergy[0] == tempMagic[0] &&
          tempMagic[0] == tempFire[0]
          && tempFire[0] == 0)
      {
          tempFire = saveLoadController.GetDefaultData().GetListStats(ElementType.Fire);
          tempWater = saveLoadController.GetDefaultData().GetListStats(ElementType.Water);
          tempEnergy = saveLoadController.GetDefaultData().GetListStats(ElementType.Energy);
          tempNature = saveLoadController.GetDefaultData().GetListStats(ElementType.Nature);
          tempMagic = saveLoadController.GetDefaultData().GetListStats(ElementType.Magic);
      }
      SetStatsToSpawnedEnemy(tempFire,ElementType.Fire);
      SetStatsToSpawnedEnemy(tempWater,ElementType.Water);
      SetStatsToSpawnedEnemy(tempEnergy,ElementType.Energy);
      SetStatsToSpawnedEnemy(tempNature,ElementType.Nature);
      SetStatsToSpawnedEnemy(tempMagic,ElementType.Magic);
   }
   
   private void GetAndSetStatsFromDefaultData()
   {
      var tempFire = saveLoadController.GetDefaultData().GetListStats(ElementType.Fire);
      var tempWater = saveLoadController.GetDefaultData().GetListStats(ElementType.Water);
      var tempEnergy = saveLoadController.GetDefaultData().GetListStats(ElementType.Energy);
      var tempNature = saveLoadController.GetDefaultData().GetListStats(ElementType.Nature);
      var tempMagic = saveLoadController.GetDefaultData().GetListStats(ElementType.Magic);


      SetStatsToPrefabs(tempFire,ElementType.Fire);
      SetStatsToPrefabs(tempWater,ElementType.Water);
      SetStatsToPrefabs(tempEnergy,ElementType.Energy);
      SetStatsToPrefabs(tempNature,ElementType.Nature);
      SetStatsToPrefabs(tempMagic,ElementType.Magic);
      
      objectPool.ChangeStatsLoadState();
   }

   private void SetStatsToPrefabs(List<float> defaultListStats, ElementType elementType)
   {
      GameObject exampleEnemy = new GameObject();
      exampleEnemy.name = "0";
      if (elementType == ElementType.Fire)
      {
        exampleEnemy = objectPool.PrefabsEnemyList[0];
      }
      
      if (elementType == ElementType.Water)
      {
         exampleEnemy = objectPool.PrefabsEnemyList[1];
      }
      
      if (elementType == ElementType.Energy)
      {
         exampleEnemy = objectPool.PrefabsEnemyList[2];
      }
      
      if (elementType == ElementType.Nature)
      {
         exampleEnemy = objectPool.PrefabsEnemyList[3];
      }
      
      if (elementType == ElementType.Magic)
      {
         exampleEnemy = objectPool.PrefabsEnemyList[4];
      }

      exampleEnemy.GetComponent<BaseEnemy>().SetStats(
         
         defaultListStats[0],
         defaultListStats[1],
         defaultListStats[2],
         defaultListStats[3]);
      exampleEnemy.SetActive(false);
   }

   private void SetStatsToSpawnedEnemy(List<float> defaultListStats, ElementType elementType)
   {
      List<BaseEnemy> tempList = new List<BaseEnemy>();
      for (int i = 0; i < lastWaveSpawned.Count; i++)
      {
         if (lastWaveSpawned[i].GetComponent<BaseEnemy>().ElementType == elementType)
         {
            if (!tempList.Contains(lastWaveSpawned[i].GetComponent<BaseEnemy>()))
            {
               tempList.Add(lastWaveSpawned[i].GetComponent<BaseEnemy>());
            }
         }
      }

      for (int i = 0; i < tempList.Count; i++)
      {
         tempList[i].SetStats(
            defaultListStats[0],
            defaultListStats[1],
            defaultListStats[2],
            defaultListStats[3]
            );
      }
      
   }

   #endregion private functions
}
