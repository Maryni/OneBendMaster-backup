using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchThreeButtons : MonoBehaviour
{
    #region Inspector variables

    [SerializeField] private GameObject buttonOpenPanel;
    [SerializeField] private GameObject buttonClosePanel;

    #endregion Inspector variables

    #region properties

    public GameObject ButtonClosePanel => buttonClosePanel;
    public GameObject ButtonOpenPanel => buttonOpenPanel;

    #endregion properties
}
