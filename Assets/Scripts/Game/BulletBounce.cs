using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBounce : MonoBehaviour
{
    #region private variables

    private Rigidbody rig;
    private Vector3 lastVelocity;

    #endregion private variables

    #region Unity functions

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        lastVelocity = rig.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.GetComponent<BaseBullet>())
        {
            var speed = lastVelocity.magnitude;
            var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

            rig.velocity = direction * speed;
        }
    }

    #endregion Unity functions

    #region public functions

    public void StopVelocity()
    {
        rig.velocity = Vector3.zero;
    }

    #endregion public functions
}
