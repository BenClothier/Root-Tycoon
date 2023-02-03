using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static event Action OnGameStart;

    public static AnimationCurve NormalDistribution;

    public static event Action OnRootPickup;

    public static int NumberOfRoots { private set; get; }

    public static void PickupRoot () {
        NumberOfRoots++;
        OnRootPickup.Invoke();
    }

    [SerializeField] private AnimationCurve normalDistr;

    private void Awake() {
        NormalDistribution = normalDistr;
    }

    private void Start() {
        OnGameStart?.Invoke();
    }
}
