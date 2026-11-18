using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Structure : ScriptableObject
{
    public string Id;
    public string DisplayName;
    public Sprite structureIcon;
    public ResourceAmount[] Cost;
    public ResourceAmount[] ClickReward;
    public GameObject prefab;
    public int currencyCost;
}
