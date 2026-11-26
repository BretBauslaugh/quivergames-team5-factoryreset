using UnityEngine;
using FMODUnity;

[System.Flags]
public enum EventProp
{
    None = 0,
    Delay =1,
    Time = 2,
    MinMaxValue = 4,
    PressNeeded = 8,
    Reward = 16,
    FailLost = 32,
    Chance = 64,
    LookAtResource = 128,
    Divider = 256,
    DestroyOnFail = 512,
}

[System.Serializable]
public class EventSettings
{
    [HideInInspector] public EventProp Enabled;

    public ResourceType LookAtResource;
    public float Time = 1f;
    public float MinTime = 1f;
    public float MaxTime = 1f;
    public int MinValue = 1;
    public int MaxValue = 1;
    public int PressNeeded = 1;
    public ResourceAmount[] Reward;
    public ResourceAmount[] FailLost;
    [Range(0f, 1f)] public float Chance = 0.1f;
    //public float Multiplier = 1f;
    public float Divider = 1;
    public bool DestroyOnFail;

    public bool Uses(EventProp p) => (Enabled & p) == p;

    public float EffectiveTimeLimit => Uses(EventProp.Time) ? Time : 0f;
    public int EffectivePresses => Uses(EventProp.PressNeeded) ? PressNeeded : 1;
}

[System.Serializable]
public class UISettings
{
    public string Prompt = " ";
    public Vector2 ScreenOffset = new Vector2(0f, 200f);
    public float UIShowTime = 0.5f;
}

[System.Serializable]
public class SfxSettings
{
    public EventReference Appear;
    public EventReference Complete;
    public EventReference Fail;
    public EventReference Press;
}
