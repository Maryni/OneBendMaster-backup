using UnityEngine;
using UnityEngine.Events;

public enum ElementType
{
    NoElement,
    Fire,
    Water,
    Energy,
    Nature,
    Magic
}

public abstract class BaseBullet : MonoBehaviour
{
    #region private variables

    protected float baseDamage = 1f;
    protected UnityAction actionOnShoot;
    protected bool triggeredByWall = false;

    #endregion private variables

    #region Inspector variables

    [SerializeField] protected string description;
    [SerializeField] protected float damage;
    [SerializeField] private ElementType elementType;

    #endregion Inspector variables
    
    #region properties
    public ElementType ElementType => elementType;
    public bool TriggeredByWall => triggeredByWall;
    
    #endregion properties

    #region Unity functions

    private void Awake()
    {
        if (damage == 0)
        {
            damage = baseDamage;
        }

        if (description == null)
        {
            Debug.LogError($"No description on Bullet {this.name}");
        }
    }

    #endregion Unity functions
    
    #region public functions

    protected virtual void Shoot()
    {
        actionOnShoot?.Invoke();
    }

    public void SetActionOnShoot(params UnityAction[] actions)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actionOnShoot += actions[i];
        }
    }

    public void SetDescription(string value)
    {
        description = value;
    }

    public void SetElementType(ElementType elementType)
    {
        this.elementType = elementType;
    }

    public void ChangeTriggerByWallState()
    {
        triggeredByWall = !triggeredByWall;
    }

    #endregion public functions
    
}

