using UnityEngine;

public interface IRowEnhancer { float RowMultiplier { get; } }

public class LineModule : MonoBehaviour, IRowEnhancer
{
    [SerializeField] private float _multiplier = 1.5f;
    public float RowMultiplier => _multiplier;
}