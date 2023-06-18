using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPDropController : DropController
{
    //References

    //Internal Variables

    //User defined objects

    //Delegates

    public delegate void XPPickedUpHandler(double experience);


    //Events

    public static event XPPickedUpHandler OnXPPickedUp;


    //Unity Methods

    //User-defined methods

    protected override void BroadcastAmount()
    {
        OnXPPickedUp?.Invoke((double)amount);
    }
}
