using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunBallPoolCells : MonoBehaviour
{
    #region Inspector variables

    [SerializeField] private GameObject ground;
    [SerializeField] private GameObject cellExample;
    [SerializeField] private List<GameObject> cellsList;
    [SerializeField] private int countColumn;
    [SerializeField] private int countLine;
    [Header("Offsets"),SerializeField] private float offsetX;
    [SerializeField] private float offsetZ;
    [SerializeField] private float offsetXCell;
    [SerializeField] private float offsetZCell;
    [SerializeField] private float leftUpperAngle;

    #endregion Inspector variables

    #region private variables

    private float xCellScale;
    private float zCellScale;

    #endregion private variables

    #region propterties

    public int countX => countColumn;
    public int countY => countLine;

    #endregion propterties

    #region Unity functions

    private void Start()
    {
        InstanceCell();
        SetXSizeForCells();
        SetCellsToList();
        SetCellsToGround();
        
    }

    #endregion Unity functions

    #region public functions

    public List<PunBallCellsIndex> GetCellsList(bool needFull = false)
    {
        List<PunBallCellsIndex> temp = new List<PunBallCellsIndex>();
        if (needFull)
        {
            for (int i = 0; i < cellsList.Count; i++)
            {
                temp.Add(cellsList[i].GetComponent<PunBallCellsIndex>()); 
            }
            
        }
        else
        {
            for (int i = 0; i < countColumn; i++)
            {
                temp.Add(cellsList[i].GetComponent<PunBallCellsIndex>());
            } 
        }
        return temp;
    }

    #endregion public functions
    
    #region private functions

    private void SetCellsToList()
    {
        var countCells = transform.childCount;
        var x = 0;
        var y = 0;
        for (int i = 0; i < countCells; i++)
        {
            var tempChild = transform.GetChild(i).gameObject;
            tempChild.AddComponent<PunBallCellsIndex>();
            var indexScript = tempChild.GetComponent<PunBallCellsIndex>();
            indexScript.SetX(x);
            indexScript.SetY(y);
            cellsList.Add(tempChild);
            
            x++;
            if (x >= countColumn)
            {
                x = 0;
                y++;
            }
        }
    }

    private void SetXSizeForCells()
    {
      var xSizeGround = ground.GetComponent<Collider>().bounds.size.x - offsetX;
      var zSizeGround = ground.GetComponent<Collider>().bounds.size.z - offsetZ;
      var groundScale = ground.transform.localScale.x;
      var groundScaleDifferentBetweenXandZ = ground.transform.localScale.z / ground.transform.localScale.x;
      xCellScale = (xSizeGround / countColumn) / groundScale;
      zCellScale = (xSizeGround/countColumn) / groundScale / groundScaleDifferentBetweenXandZ;
      cellExample.transform.localScale = new Vector3(
          xCellScale, 
          cellExample.transform.localScale.y,
          zCellScale);
    }

    private void SetCellsToGround()
    {
        for (int i = 0; i < cellsList.Count; i++)
        {
            var tempObject = cellsList[i];
            var positionPerCell = ((float)1 / (countColumn + (offsetXCell/countColumn)));
            tempObject.transform.localScale = new Vector3(
                xCellScale, 
                cellExample.transform.localScale.y,
                zCellScale);
           
                tempObject.transform.localPosition = new Vector3(
                    leftUpperAngle + 
                    //offsetXCell + 
                    positionPerCell * 
                    tempObject.GetComponent<PunBallCellsIndex>().X,
                    tempObject.transform.localPosition.y,
                    -leftUpperAngle + 
                    //offsetZCell + 
                    positionPerCell/2 * 
                    -tempObject.GetComponent<PunBallCellsIndex>().Y);
        }
    }

    private void InstanceCell()
    {
        var countToSpawn = countColumn * countLine;
        for (int i = 0; i < countToSpawn; i++)
        {
            var tempObject = Instantiate(cellExample, transform);
        }
    }
    
    #endregion private functions
}
