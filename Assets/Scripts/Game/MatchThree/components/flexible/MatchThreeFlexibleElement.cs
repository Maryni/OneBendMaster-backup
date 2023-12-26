using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class MatchThreeFlexibleElement : BaseMatchThree
{
    #region Inspector variables

    [SerializeField] private int x;
    [SerializeField] private int y;

    #endregion Inspector variables

    #region properties

    public int X => x;
    public int Y => y;

    #endregion properties

    #region public functions

    public void SetX(int value)
    {
        x = value;
    }
    
    public void SetY(int value)
    {
        y = value;
    }

    #endregion public functions
}
