using UnityEngine;

public class DoublerModule : MonoBehaviour, IEnhancer
{
    [SerializeField] private float _multiplier = 1.25f;
    public float Multiplier => _multiplier;
}
