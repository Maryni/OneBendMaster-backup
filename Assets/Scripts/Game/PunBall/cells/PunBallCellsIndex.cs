using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunBallCellsIndex : MonoBehaviour
{
    #region Inspector variables

    [SerializeField] private int x;
    [SerializeField] private int y;
    [SerializeField] private bool isOcupied;
    [SerializeField] private bool movedInThisWave;

    #endregion Inspector variables

    #region properties

    public int X => x;
    public int Y => y;
    public bool IsOcupied => isOcupied;
    public bool MovedInThisWave => movedInThisWave;

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

    /// <summary>
    /// Working like on->off, and off->on
    /// </summary>
    public void ChangeOcupiedState()
    {
        isOcupied = !isOcupied;
    }

    public void ChangeMovedState()
    {
        movedInThisWave = !movedInThisWave;
    }

    #endregion public functions

}
