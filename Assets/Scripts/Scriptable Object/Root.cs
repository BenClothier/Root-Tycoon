using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Root", menuName = "Root")]
public class Root : ScriptableObject
{
    [field: SerializeField] public GameObject Model { private set; get; }
    [field: SerializeField] public float TimeToGrow { private set; get; }

    [field: SerializeField] public Vector3 StartScale { private set; get; }
    [field: SerializeField] public Vector3 EndScale { private set; get; }
}
