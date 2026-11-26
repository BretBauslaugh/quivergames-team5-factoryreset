using UnityEngine;

public class DimeModule : MonoBehaviour, IFlatEnhancer
{
    [SerializeField] private int _flatBonus = 10;
    public int FlatBonus => _flatBonus;
}