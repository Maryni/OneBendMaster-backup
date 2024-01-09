using UnityEngine;

public class MatchThreeButtons : MonoBehaviour
{
    #region Inspector variables

    [SerializeField] private GameObject buttonOpenPanel;
    [SerializeField] private GameObject buttonClosePanel;
    [SerializeField] private GameObject buttonRecolorPanel;

    #endregion Inspector variables

    #region properties

    public GameObject ButtonClosePanel => buttonClosePanel;
    public GameObject ButtonOpenPanel => buttonOpenPanel;
    public GameObject ButtonRecolorPanel => buttonRecolorPanel;

    #endregion properties
}
