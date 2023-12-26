using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default Level Data",menuName = "LevelData/Create Default Level Data",order = 53)]
public class DefaultLevelData : ScriptableObject
{
    #region Inspector variables

    [SerializeField] private Data data;

    #endregion Inspector variables

    #region public functions

    public Data GetDefaultData()
    {
        return data;
    }

    public List<float> GetListStats(ElementType elementType)
    {
        return data.GetListStats(elementType);
    }
    
    #endregion public functions
}
