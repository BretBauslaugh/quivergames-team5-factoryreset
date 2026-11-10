using UnityEngine;

[CreateAssetMenu]
public class Resource : ScriptableObject
{
    [SerializeField] private int _startX;
    [SerializeField] private int _startY;

    public int X { get; private set; }
    public int Y { get; private set; }

    private void Awake()
    {
        X = _startX;
        Y = _startY;
    }

    public bool CanAfford(float x) => X >= x;


}
