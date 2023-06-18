using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPDropController : DropController
{
    //References

    //Internal Variables

    //User defined objects

    //Delegates

    public delegate void HPPickedUpHandler(float HP);


    //Events

    public static event HPPickedUpHandler OnHPPickedUp;


    //Unity Methods

    //User-defined methods

    protected override void BroadcastAmount()
    {
        OnHPPickedUp?.Invoke(amount);
    }
}
