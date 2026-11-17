using UnityEngine;

public class DoublerModule : MonoBehaviour, IEnhancer
{
    [SerializeField] private float _multiplier = 2f;
    public float Multiplier => _multiplier;
}
