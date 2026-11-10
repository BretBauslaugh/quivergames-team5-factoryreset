using UnityEngine;

public class Building02 : BuildingInstance
{
    [SerializeField] private Building02_SO _buildingData;

    protected override Structure StrucData => _buildingData;

    protected override string ExtraInfo => $"Reqired {_buildingData.reqiredY} unit of Y, " + 
        $"Generating {_buildingData.generatedX} unit of X per second";
    protected override void Start()
    {
        base.Start();

    }

    void Update()
    {

    }
}
