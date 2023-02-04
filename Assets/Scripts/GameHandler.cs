using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static event Action OnGameStart;

    public static AnimationCurve NormalDistribution;
    public static AnimationCurve PriceDifferenceCurve;

    [SerializeField] private AnimationCurve normalDistr;
    [SerializeField] private AnimationCurve priceDifferenceCurve;

    public static int CurrentSelection = -1;

    private void Awake() {
        NormalDistribution = normalDistr;
        PriceDifferenceCurve = priceDifferenceCurve;
    }

    private void Start() {
        OnGameStart?.Invoke();
    }
}
