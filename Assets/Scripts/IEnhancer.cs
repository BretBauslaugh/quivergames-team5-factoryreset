using UnityEngine;

public interface IEnhancer
{
    float Multiplier { get; }
}

public interface IFlatEnhancer 
{ 
    int FlatBonus { get; } 
}

public interface IOutcomeListener
{
    void OnNeighborSuccess();
    void OnNeighborFailure();
}