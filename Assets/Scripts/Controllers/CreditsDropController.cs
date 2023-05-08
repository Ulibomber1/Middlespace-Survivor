using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsDropController : DropController
{
    public delegate void CreditsPickedUpHandler(int credits);
    public static event CreditsPickedUpHandler OnCreditsPickedUp;

    protected override void BroadcastAmount()
    {
        OnCreditsPickedUp?.Invoke((int)amount);
    }
}