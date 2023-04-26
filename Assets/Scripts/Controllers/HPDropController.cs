using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPDropController : DropController
{
    public delegate void HPPickedUpHandler(float HP);
    public static event HPPickedUpHandler OnHPPickedUp;

    protected override void BroadcastAmount()
    {
        //Heal?.Invoke(amount);
    }
}
