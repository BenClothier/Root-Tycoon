using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Market
{
    private const int DEMANDS_TO_SHOW = 3;

    private const int MIN_NEW_DEMAND_EPOCHS = 10;
    private const int MAX_NEW_DEMAND_EPOCHS = 15;

    private const float DIST_TO_DEMAND_TO_REVEAL = 0.05f;

    private const int START_VALUE = 10;
    private const float INCREASE_AMOUNT = 2f;
 
    public List<Demand> RevealedDemand { get; private set; } = new();

    public Market(){
        RevealedDemand.Add(new Demand()
        {
            DemandedRootAttributes = RootAttributes.Default(),
            BaseSalePrice = START_VALUE,
        });
        MakeNewDemand();
    }

    public void SellAll(List<RootAttributes> rootsToSell)
    {
        PlayerStats.AddMoney(rootsToSell.Sum(r => GetSalePriceOfRoot(r)));
        Debug.Log("Money: " + PlayerStats.money);
    }

    public List<Demand> GetMostRelevantDemands()
    {
        return Enumerable.Reverse(RevealedDemand).Take(DEMANDS_TO_SHOW).ToList();
    }

    public int GetSalePriceOfRoot(RootAttributes root, bool shouldCountAsProgress = false){
        bool shouldMakeNewDemand = false;
        int highestWorth = RevealedDemand.Max(demand => GetLikenessOfRootToDemand(root, demand, shouldCountAsProgress, (shouldCountAsProgress && demand == RevealedDemand[RevealedDemand.Count - 1] ? () => shouldMakeNewDemand = true : null)));

        if (shouldMakeNewDemand)
        {
            MakeNewDemand();
        }

        return highestWorth;
    }

    private int GetLikenessOfRootToDemand(RootAttributes root, Demand demand, bool shouldCountAsProgress, Action ifLikenessWithinThreshold){
        var actualAttributes = root.AttributeVector;
        var demandAttributes = demand.DemandedRootAttributes.AttributeVector;

        float squaredSum = 0;
        for (int i = 0; i < actualAttributes.Count(); i++){
            squaredSum += Mathf.Pow(actualAttributes[i] - demandAttributes[i], 2);
        }

        float likenessToDemand = Mathf.Sqrt(squaredSum);

        if (shouldCountAsProgress)
        {
            Debug.Log(likenessToDemand);
        }

        if (likenessToDemand <= DIST_TO_DEMAND_TO_REVEAL)
        {
            ifLikenessWithinThreshold?.Invoke();
        }

        return Mathf.CeilToInt(Mathf.Max(0, demand.BaseSalePrice * GameHandler.PriceDifferenceCurve.Evaluate(likenessToDemand)));
    }

    private void MakeNewDemand(){
        Debug.Log("Made new demand!");
        RevealedDemand.Add(new Demand(){
            DemandedRootAttributes = SimulateRootGeneticChange(RevealedDemand[RevealedDemand.Count - 1].DemandedRootAttributes, UnityEngine.Random.Range(MIN_NEW_DEMAND_EPOCHS, MAX_NEW_DEMAND_EPOCHS)),
            BaseSalePrice = Mathf.FloorToInt(RevealedDemand[RevealedDemand.Count - 1].BaseSalePrice * INCREASE_AMOUNT),
        });
    }

    private RootAttributes SimulateRootGeneticChange(RootAttributes startAttributes, int epochs){
        RootAttributes current = startAttributes;
        for (int i = 0; i < epochs; i++){
            current = RootAttributes.MakeMutatedCopy(current, new float[]{ 3, 2, 1, 0.5f, 0.25f, 0, 0, 0, 0 }.OrderBy(x => UnityEngine.Random.value).ToArray());
        }
        return current;
    }

    public class Demand {
        public RootAttributes DemandedRootAttributes;
        public int BaseSalePrice;
    }
}
