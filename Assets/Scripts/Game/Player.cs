using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    #region Inspector variables

    [SerializeField] private float shootRate;
    [SerializeField] private List<int> maxBulletTypeCount;
    [SerializeField] private List<ElementType> maxBulletTypeElementType;
    [SerializeField] private int maxHp;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject activeBullet;
    [SerializeField] private bool canShoot = false;
    [SerializeField] private LayerMask layerMaskForShoot;

    #endregion Inspector variables

    #region private variables

    private int currentHp;
    private int currentBulletsCount;
    private UnityAction actionOnShoot;
    private UnityAction actionAfterShootingWhenBulletsZero;
    private UnityAction actionAfterShootAllBullets;
    private Camera cam;
    private Coroutine shootCoroutine;
    [SerializeField]private bool panelClosed;
    
    #endregion private variables

    #region properties

    public List<int> MaxBulletTypeCount => maxBulletTypeCount;
    public int CurrentBulletsCount => currentBulletsCount;
    public int CurrentHp => currentHp;
    public int MaxHp => maxHp;

    #endregion properties

    #region Unity functions

    private void Start()
    {
        SetCurrentHp();
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Shoot();
        }
    }

    #endregion Unity functions
    
    #region public functions

    public void SetActionsOnShoot(params UnityAction[] actions)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actionOnShoot += actions[i];
        }
    }
    
    public void SetActionAfterShootingWhenBulletsZero(params UnityAction[] actions)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actionAfterShootingWhenBulletsZero += actions[i];
        }
    }

    public void SetActionAfterShootAllBullets(UnityAction action)
    {
        actionAfterShootAllBullets += action;
    }
    
    public void SetCurrentBulletsForFirstBullet(string value)
    {
        if (value != "0")
        {
            for (int i = 0; i < maxBulletTypeCount.Count; i++)
            {
                if (maxBulletTypeCount[i] == 0)
                {
                    maxBulletTypeCount[i] = int.Parse(value);
                    break;
                }
            } 
        }
    }
    
    public void SetCurrentHp()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(int value)
    {
        currentHp -= value;
    }

    public void SetCountCurrentBullets()
    {
        currentBulletsCount = maxBulletTypeCount.FirstOrDefault(x => x > 0);
    }

    public void ChangeCanShootState()
    {
        canShoot = !canShoot;
    }

    public void ChangePanelClosedState()
    {
        panelClosed = !panelClosed;
    }
    
    #endregion public functions

    #region private functions

    private void Shoot()
    {
        if (panelClosed)
        {
            SetCountCurrentBullets();
            if (canShoot && currentBulletsCount > 0)
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;
                if (currentBulletsCount >= 1)
                {
                    for (int i = 0; i < maxBulletTypeCount.Count; i++)
                    {
                        if (maxBulletTypeCount[i] > 0)
                        {
                            maxBulletTypeCount[i] = 0;
                            break;
                        }
                    } 
                }

                Debug.Log("[Shoot]");
                ShootActiveBullet(currentBulletsCount);
            
            
                if (!IsHaveNotAvalibleBullets())
                {
                    for (int i = 0; i < maxBulletTypeCount.Count; i++)
                    {
                        if (maxBulletTypeCount[i] > 0)
                        {
                            currentBulletsCount = maxBulletTypeCount[i];
                            break;
                        }
                    }
                }
            }
        }
    }
    
    private bool IsHaveNotAvalibleBullets()
    {
        var allElementsZero = maxBulletTypeCount.All(x => x == 0);
        return allElementsZero;
    }

    private void ShootActiveBullet(int countBullet)
    {
        shootCoroutine = StartCoroutine(ShootActiveBulletCoroutine(countBullet));
    }
    
    private IEnumerator ShootActiveBulletCoroutine(int countCycles)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        var currentHitPointRay = ray; 
        for(int i=0; i< countCycles; i++)
        {
            if (activeBullet == null)
            {
                actionOnShoot?.Invoke();
            }
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(shootRate);
                
            activeBullet.transform.position = transform.position;
            activeBullet.transform.position = new Vector3( activeBullet.transform.position.x, activeBullet.transform.position.y - 0.5f,activeBullet.transform.position.z + 2f);
            if (Physics.Raycast(ray, out hit, 100, layerMaskForShoot))
            {
                
                
                Vector3 endPoint = hit.point;
                Vector3 direction = endPoint - transform.position;
                activeBullet.transform.LookAt(direction);
                var rig = activeBullet.GetComponent<Rigidbody>();
                rig.velocity = (direction * bulletSpeed);
                activeBullet = null;
            }
            else
            {
                Debug.LogWarning($"No hit point");
            }
        }
        StopCoroutine(ShootActiveBulletCoroutine(countCycles));
        shootCoroutine = null;
        actionAfterShootAllBullets?.Invoke();
        if (IsHaveNotAvalibleBullets())
        {
            actionAfterShootingWhenBulletsZero?.Invoke();
        }
    }

    #endregion private functions
}
