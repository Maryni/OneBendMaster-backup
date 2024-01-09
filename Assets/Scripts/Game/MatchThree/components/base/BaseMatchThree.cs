using UnityEngine;
using UnityEngine.UI;

public abstract class BaseMatchThree : MonoBehaviour
{
    #region private variables

    #endregion private variables

    #region Inspector variables
    
    [SerializeField] private ElementType elementType;
    [SerializeField] private Sprite sprite;
    [SerializeField] private Image image;

    #endregion Inspector variables
    
    #region properties
    
    public ElementType ElementType => elementType;
    public Sprite Sprite => sprite;
    public Image Image => image;
    
    #endregion properties

    #region Unity functions
    

    #endregion Unity functions
    
    #region public functions

    public void SetElementType(ElementType elementType)
    {
        this.elementType = elementType;
    }

    public void SetSprite(Sprite sprite)
    {
        this.sprite = sprite;
        image.sprite = sprite;
    }
    


    #endregion public functions 
}

