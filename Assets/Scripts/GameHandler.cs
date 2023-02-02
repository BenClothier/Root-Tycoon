using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static event Action OnRootPickup;

    public static int NumberOfRoots { private set; get; }

    public static void PickupRoot () {
        NumberOfRoots++;
        OnRootPickup.Invoke();
    }
}
