using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootAttributes
{
    public Color UpperColor { get; set; }
    public Color LowerColor { get; set; }
    public float Length { get; set; }
    public float OverallGirth { get; set; }
    public float LowerGirth { get; set; }

    public float[] AttributeVector => new float[] { 
        UpperColor.r,
        UpperColor.g,
        UpperColor.b,
        LowerColor.r,
        LowerColor.g,
        LowerColor.b,
        (Length - 1) / 2,
        (OverallGirth - 1) / 4,
        (LowerGirth - .5f) / 1.5f
        };

    public static RootAttributes Default() => new RootAttributes()
    {
        UpperColor = new Color(.75f, .72f, .56f, 1),
        LowerColor = new Color(.75f, .72f, .56f, 1),
        Length = 1,
        OverallGirth = 1,
        LowerGirth = 1,
    };

    public static RootAttributes LowerBound() => new RootAttributes()
    {
        UpperColor = new Color(.2f, .2f, .2f, 1),
        LowerColor = new Color(.2f, .2f, .2f, 1),
        Length = 1,
        OverallGirth = 1,
        LowerGirth = .2f,
    };

    public static RootAttributes UpperBound() => new RootAttributes()
    {
        UpperColor = new Color(.8f, .8f, .8f, 1),
        LowerColor = new Color(.8f, .8f, .8f, 1),
        Length = 3,
        OverallGirth = 5,
        LowerGirth = 2.5f,
    };

    public static RootAttributes MakeMutatedCopy(RootAttributes current){
        RootAttributes lower = RootAttributes.LowerBound();
        RootAttributes upper = RootAttributes.UpperBound();

        Color newUpperColour = new Color(
            Mathf.Clamp(current.UpperColor.r + (upper.UpperColor.r - lower.UpperColor.r) * GameHandler.NormalDistribution.Evaluate(UnityEngine.Random.value) * 1.5f, lower.UpperColor.r, upper.UpperColor.r),
            Mathf.Clamp(current.UpperColor.g + (upper.UpperColor.g - lower.UpperColor.g) * GameHandler.NormalDistribution.Evaluate(UnityEngine.Random.value) * 1.5f, lower.UpperColor.g, upper.UpperColor.g),
            Mathf.Clamp(current.UpperColor.b + (upper.UpperColor.b - lower.UpperColor.b) * GameHandler.NormalDistribution.Evaluate(UnityEngine.Random.value) * 1.5f, lower.UpperColor.b, upper.UpperColor.b),
            1
        );

        Color newLowerColour = new Color(
            Mathf.Clamp(current.LowerColor.r + (upper.LowerColor.r - lower.LowerColor.r) * GameHandler.NormalDistribution.Evaluate(UnityEngine.Random.value) * 1.5f, lower.LowerColor.r, upper.LowerColor.r),
            Mathf.Clamp(current.LowerColor.g + (upper.LowerColor.g - lower.LowerColor.g) * GameHandler.NormalDistribution.Evaluate(UnityEngine.Random.value) * 1.5f, lower.LowerColor.g, upper.LowerColor.g),
            Mathf.Clamp(current.LowerColor.b + (upper.LowerColor.b - lower.LowerColor.b) * GameHandler.NormalDistribution.Evaluate(UnityEngine.Random.value) * 1.5f, lower.LowerColor.b, upper.LowerColor.b),
            1
        );

        return new(){
            UpperColor = newUpperColour,
            LowerColor = newLowerColour,
            Length = current.Length + (upper.Length - lower.Length) * GameHandler.NormalDistribution.Evaluate(UnityEngine.Random.value),
            OverallGirth = current.OverallGirth + (upper.OverallGirth - lower.OverallGirth) * GameHandler.NormalDistribution.Evaluate(UnityEngine.Random.value) * .5f,
            LowerGirth = current.LowerGirth + (upper.LowerGirth - lower.LowerGirth) * GameHandler.NormalDistribution.Evaluate(UnityEngine.Random.value) * .75f,
        };
    }
}
