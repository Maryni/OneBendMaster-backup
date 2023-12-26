using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBulletsChecker : MonoBehaviour
{

    #region Unity functions

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BaseBullet>() && other.gameObject.GetComponent<BaseBullet>().TriggeredByWall)
        {
            other.gameObject.GetComponent<BaseBullet>().ChangeTriggerByWallState();
            other.gameObject.GetComponent<BulletBounce>().StopVelocity();
            other.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<BaseBullet>())
        {
            other.gameObject.GetComponent<BaseBullet>().ChangeTriggerByWallState();
        }
    }

    #endregion Unity functions
    
}
