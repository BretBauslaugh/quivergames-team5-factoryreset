using UnityEngine;

public interface IEnhancer
{
    float Multiplier { get; }
}

public interface IFlatEnhancer 
{ 
    int FlatBonus { get; } 
}