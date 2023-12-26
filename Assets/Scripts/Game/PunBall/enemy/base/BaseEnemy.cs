using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseEnemy : MonoBehaviour
{
    #region private variables
    
    protected UnityAction actionOnShooted;
    protected float currentHp;
    protected float currentDamage;
    protected float maxHp;
    
    #endregion private variables

    #region Inspector variables
    
    [SerializeField] protected ElementType elementType;
    [Header("Stats, mod 1 = 1%"),SerializeField] private float baseHp;
    [SerializeField] private float modHp;
    [SerializeField] private float baseDamage;
    [SerializeField] private float modDamage;

    #endregion Inspector variables
    
    #region properties
    public float CurrentHP => currentHp;
    public float CurrentDamage => currentDamage;
    public ElementType ElementType => elementType;
    
    #endregion properties

    #region Unity functions

    private void Awake()
    {
        CalculateStatsWithMods();
    }

    #endregion Unity functions
    
    #region public functions

    protected virtual void Shooted()
    {
        actionOnShooted?.Invoke();
    }

    public void SetActionOnShooted(params UnityAction[] actions)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actionOnShooted += actions[i];
        }
    }

    #endregion public functions

    #region private functions

    public void SetStats(float baseHp, float modHp, float baseDamage, float modDamage)
    {
        this.baseHp = baseHp;
        this.modHp = modHp;
        this.baseDamage = baseDamage;
        this.modDamage = modDamage;
        
        CalculateStatsWithMods();
    }
    
    protected void CalculateStatsWithMods()
    {
        maxHp = baseHp + (baseHp * modHp);
        currentDamage = baseDamage + (baseDamage * modDamage);
    }

    protected void Init(ElementType elementType)
    {
        this.elementType = elementType;
    }
    
    #endregion private functions
}
