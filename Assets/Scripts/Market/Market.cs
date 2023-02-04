using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Market
{
    private const int MIN_NEW_DEMAND_EPOCHS = 5;
    private const int MAX_NEW_DEMAND_EPOCHS = 5;

    private const float DIST_TO_DEMAND_TO_REVEAL = 1;

    private const int START_VALUE = 10;
    private const float INCREASE_MULTIPLIER = 2;
 
    public List<Demand> RevealedDemand { get; private set; } = new();
    public Demand NextHiddenDemand { get; private set; }

    private List<Demand> AllDemand => RevealedDemand.Append(NextHiddenDemand).ToList();

    private int currentPrice = START_VALUE; 

    public Market(){
        var initialDemand = new Demand(){
            demandedRootAttributes =  RootAttributes.Default(),
            BaseSalePrice = 10,
        };
        NextHiddenDemand = initialDemand;
        RevealHiddenDemand();
    }

    public void RevealHiddenDemand(){
        RevealedDemand.Add(NextHiddenDemand);
        MakeNewDemand();
    }

    public int GetSalePriceOfRoot(RootAttributes root){
        return AllDemand.Max(demand => GetSalePriceOfRootAccordingToDemand(root, demand));
    }

    private int GetSalePriceOfRootAccordingToDemand(RootAttributes root, Demand demand){
        var actualAttributes = root.AttributeVector;
        var demandAttributes = demand.demandedRootAttributes.AttributeVector;

        float squaredSum = 0;
        for (int i = 0; i < actualAttributes.Count(); i++){
            squaredSum += Mathf.Pow(actualAttributes[i] - demandAttributes[i], 2);
        }

        float likenessToDemand = Mathf.Sqrt(squaredSum);

        if (likenessToDemand <= DIST_TO_DEMAND_TO_REVEAL){
            RevealHiddenDemand();
        }

        return Mathf.FloorToInt(Mathf.Max(0, demand.BaseSalePrice - GameHandler.PriceDifferenceCurve.Evaluate(likenessToDemand)));
    }

    private void MakeNewDemand(){
        currentPrice = Mathf.FloorToInt(currentPrice * INCREASE_MULTIPLIER);
        NextHiddenDemand = new Demand(){
            demandedRootAttributes = SimulateRootGeneticChange(NextHiddenDemand.demandedRootAttributes, UnityEngine.Random.Range(MIN_NEW_DEMAND_EPOCHS, MAX_NEW_DEMAND_EPOCHS)),
            BaseSalePrice = currentPrice,
        };
    }

    private RootAttributes SimulateRootGeneticChange(RootAttributes startAttributes, int epochs){
        RootAttributes current = startAttributes;
        for (int i = 0; i < epochs; i++){
            current = RootAttributes.MakeMutatedCopy(current);
        }
        return current;
    }

    public struct Demand {
        public RootAttributes demandedRootAttributes;
        public int BaseSalePrice;
    }
}
