using UnityEngine;
using FMODUnity;

[System.Flags]
public enum EventProp
{
    None = 0,
    Delay =1,
    TimeLimit = 2,
    PressNeeded = 4,
    Reward = 8,
    FailLost = 16,
    DestroyOnFail = 32,
}

[System.Serializable]
public class EventSettings
{
    [HideInInspector] public EventProp Enabled;

    public float Time = 0f;
    public float MinTime = 0f;
    public float MaxTime = 0f;
    public int PressNeeded = 1;
    public ResourceAmount[] Reward;
    public ResourceAmount[] FailLost;
    public bool DestroyOnFail;

    public bool Uses(EventProp p) => (Enabled & p) == p;

    public float EffectiveTimeLimit => Uses(EventProp.TimeLimit) ? Time : 0f;
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
