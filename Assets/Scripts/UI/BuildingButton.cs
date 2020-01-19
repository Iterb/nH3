using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : UnitButton
{
    public void SpawnBuilding()
    {
        MouseBehaviour.SpawnBuilding(spawnPrefab);
    }
    public override void SpawnUnit()
    {
        //base.SpawnUnit();
    }
}
