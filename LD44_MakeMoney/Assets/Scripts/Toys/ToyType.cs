using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Make Money/Toy Type")]
public class ToyType : ScriptableObject
{
    public Color color;
    public Sprite sprite;
    public float moneyValue;
    public float approvalValue;
    public float penaltyMultiplier;
}
