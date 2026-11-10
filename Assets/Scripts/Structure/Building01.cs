using UnityEngine;

public class Building01 : BuildingInstance
{
    [SerializeField] private Building01_SO _buildingData;

    protected override Structure StrucData => _buildingData;

    protected override string ExtraInfo => $"Reqired {_buildingData.reqiredX} unit of X, " +
        $"Generating {_buildingData.generatedY} unit of Y per second";
    protected override void Start()
    {
        base.Start();

    }

    void Update()
    {
        
    }
}
