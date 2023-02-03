using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootAttributes
{
    public Color UpperColor { get; set; } = new Color(192, 184, 144, 255);
    public Color LowerColor { get; set; } = new Color(192, 184, 144, 255);
    public float Length { get; set; } = 1;
    public float OverallGirth { get; set; } = 1;
    public float LowerGirth { get; set; } = 1;

    public static RootAttributes LowerBound() => new()
    {
        UpperColor = new Color(50, 50, 50, 255),
        LowerColor = new Color(50, 50, 50, 255),
        Length = 1,
        OverallGirth = 1,
        LowerGirth = .5f,
    };

    public static RootAttributes UpperBound() => new()
    {
        UpperColor = new Color(205, 205, 205, 255),
        LowerColor = new Color(205, 205, 205, 255),
        Length = 2,
        OverallGirth = 3,
        LowerGirth = 1.5f,
    };

    public static RootAttributes MakeMutatedCopy(RootAttributes current, float sigma){
        RootAttributes lower = RootAttributes.LowerBound();
        RootAttributes upper = RootAttributes.UpperBound();

        return new(){
            UpperColor = new Color(
                Mathf.Clamp(current.UpperColor.r + (upper.UpperColor.r - lower.UpperColor.r) * GameHandler.NormalDistribution.Evaluate(UnityEngine.Random.value), lower.UpperColor.r, upper.UpperColor.r),
                Mathf.Clamp(current.UpperColor.g + (upper.UpperColor.g - lower.UpperColor.g) * GameHandler.NormalDistribution.Evaluate(UnityEngine.Random.value), lower.UpperColor.g, upper.UpperColor.g),
                Mathf.Clamp(current.UpperColor.b + (upper.UpperColor.b - lower.UpperColor.b) * GameHandler.NormalDistribution.Evaluate(UnityEngine.Random.value), lower.UpperColor.b, upper.UpperColor.b),
                255
            ),
            
            LowerColor = new Color(
                Mathf.Clamp(current.LowerColor.r + (upper.LowerColor.r - lower.LowerColor.r) * GameHandler.NormalDistribution.Evaluate(UnityEngine.Random.value), lower.LowerColor.r, upper.LowerColor.r),
                Mathf.Clamp(current.LowerColor.g + (upper.LowerColor.g - lower.LowerColor.g) * GameHandler.NormalDistribution.Evaluate(UnityEngine.Random.value), lower.LowerColor.g, upper.LowerColor.g),
                Mathf.Clamp(current.LowerColor.b + (upper.LowerColor.b - lower.LowerColor.b) * GameHandler.NormalDistribution.Evaluate(UnityEngine.Random.value), lower.LowerColor.b, upper.LowerColor.b),
                255
            ),
            
            Length = current.Length + (upper.Length - lower.Length) * GameHandler.NormalDistribution.Evaluate(UnityEngine.Random.value),
            OverallGirth = current.OverallGirth + (upper.OverallGirth - lower.OverallGirth) * GameHandler.NormalDistribution.Evaluate(UnityEngine.Random.value),
            LowerGirth = current.LowerGirth + (upper.LowerGirth - lower.LowerGirth) * GameHandler.NormalDistribution.Evaluate(UnityEngine.Random.value),
        };
    }
}
