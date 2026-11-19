using UnityEngine;

public class TimeSkipper : MonoBehaviour, ITimeSkip
{
    [SerializeField] private float _timeMuliplier = 0.75f;

    public float TimeMultiplier => _timeMuliplier;
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
}
