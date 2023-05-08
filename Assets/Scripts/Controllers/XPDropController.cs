using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPDropController : DropController
{
    public delegate void XPPickedUpHandler(double experience);
    public static event XPPickedUpHandler OnXPPickedUp;

    protected override void BroadcastAmount()
    {
        OnXPPickedUp?.Invoke((double)amount);
    }
}
