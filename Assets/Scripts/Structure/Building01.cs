using UnityEngine;

public class Building01 : BuildingInstance
{
    [SerializeField] private Generators_SO _buildingData;

    //protected override Structure StrucData => _buildingData;

    //protected override string ExtraInfo => 
    protected override void Start()
    {
        base.Start();

    }

    void Update()
    {
        
    }
}
