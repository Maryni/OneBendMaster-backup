using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyEnemy : BaseEnemy
{
    private void Start()
    {
        Init(ElementType.Energy);
    }
}
