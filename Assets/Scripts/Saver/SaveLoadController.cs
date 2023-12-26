using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveLoadController : MonoBehaviour
{
    #region Inspector variables

    [SerializeField] private DefaultLevelData defaultLevelData;
    [SerializeField] private List<LevelData> levelDatas;
    [SerializeField] private int lastCompleteLevel;

    #endregion Inspector variables

    #region properties

    public int LastCompleteLevel => lastCompleteLevel;

    #endregion properties
    
    #region Unity functions

    private void Start()
    {
        SetLastCompleteLevel();
    }

    #endregion Unity functions
    
    #region public functions

    public LevelData GetFullLevelData(int index)
    {
        return levelDatas[index];
    }

    public int GetCountWavesOnLevelData(int indexLevel)
    {
        return levelDatas[indexLevel].GetWaveCount();
    }

    public Data GetWaveData(int indexWave, int indexLevel)
    {
        return levelDatas[indexLevel - 1].GetDataWaveByIndex(indexWave);
    }

    public Data GetDataFromDefaultData()
    {
        return defaultLevelData.GetDefaultData();
    }

    public DefaultLevelData GetDefaultData()
    {
        return defaultLevelData;
    }


    #endregion public functions

    #region private functions

    private void SetLastCompleteLevel()
    {
        if (lastCompleteLevel == 0)
        {
            lastCompleteLevel = levelDatas.FirstOrDefault(x => !x.IsLevelComplete)!.IndexLevel; 
        }
    }

    #endregion private functions

}
