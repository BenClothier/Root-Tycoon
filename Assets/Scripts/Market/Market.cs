using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Market
{
    private const int DEMANDS_TO_SHOW = 3;

    private const int MIN_NEW_DEMAND_EPOCHS = 10;
    private const int MAX_NEW_DEMAND_EPOCHS = 15;

    private const float DIST_TO_DEMAND_TO_REVEAL = 0.1f;

    private const int START_VALUE = 10;
    private const int INCREASE_AMOUNT = 15;
 
    public List<Demand> RevealedDemand { get; private set; } = new();
    public Demand NextHiddenDemand { get; private set; }

    private List<Demand> AllDemand => RevealedDemand.Append(NextHiddenDemand).ToList();

    public Market(){
        var initialDemand = new Demand(){
            DemandedRootAttributes =  RootAttributes.Default(),
            BaseSalePrice = START_VALUE,
        };
        NextHiddenDemand = initialDemand;
        RevealHiddenDemand();
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

    public void RevealHiddenDemand(){
        Debug.Log("HIDDEN DEMAND REVEALED!");
        RevealedDemand.Add(NextHiddenDemand);
        MakeNewDemand();
    }

    public int GetSalePriceOfRoot(RootAttributes root){
        return AllDemand.Max(demand => GetSalePriceOfRootAccordingToDemand(root, demand));
    }

    private int GetSalePriceOfRootAccordingToDemand(RootAttributes root, Demand demand){
        var actualAttributes = root.AttributeVector;
        var demandAttributes = demand.DemandedRootAttributes.AttributeVector;

        float squaredSum = 0;
        for (int i = 0; i < actualAttributes.Count(); i++){
            squaredSum += Mathf.Pow(actualAttributes[i] - demandAttributes[i], 2);
        }

        float likenessToDemand = Mathf.Sqrt(squaredSum);

        if (demand == NextHiddenDemand && likenessToDemand <= DIST_TO_DEMAND_TO_REVEAL){
            RevealHiddenDemand();
        }

        return Mathf.FloorToInt(Mathf.Max(0, demand.BaseSalePrice * GameHandler.PriceDifferenceCurve.Evaluate(likenessToDemand)));
    }

    private void MakeNewDemand(){
        NextHiddenDemand = new Demand(){
            DemandedRootAttributes = SimulateRootGeneticChange(NextHiddenDemand.DemandedRootAttributes, UnityEngine.Random.Range(MIN_NEW_DEMAND_EPOCHS, MAX_NEW_DEMAND_EPOCHS)),
            BaseSalePrice = NextHiddenDemand.BaseSalePrice + INCREASE_AMOUNT,
        };
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
