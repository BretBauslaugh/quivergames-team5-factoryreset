using System.Collections.Generic;
using UnityEngine;


public class Generator : TimeModule
{
    protected override void OnTick()
    {
        foreach(var r in Data.Production)
        {
            Building.PayOut(r.Type, r.Amount);
        }
        Building.CountProduction();
    }
}
