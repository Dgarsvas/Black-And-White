using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Signs
{
    Searching,
    Spotted,
    Surrender
}

public class BaseEnemy : BaseEntity
{
    public virtual void ShowSign(Signs sign)
    {
        throw new NotImplementedException();
    }
}
