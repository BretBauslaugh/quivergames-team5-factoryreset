using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Structure : ScriptableObject
{    
    public string DisplayName;
    public string Description;
    public Sprite structureIcon;
    public ResourceAmount[] Cost;
    //public ResourceAmount[] ClickReward;
    public GameObject prefab;    

    public EventSettings Event;
    public UISettings UI;
    public SfxSettings Sfx;
}
