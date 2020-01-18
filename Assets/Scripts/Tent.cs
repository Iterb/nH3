using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tent : Building, ISelectable
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    public void SetSelected(bool selected)
    {
        flag.gameObject.SetActive(selected);
        healthBar.gameObject.SetActive(selected);
    }

    void Spawn(GameObject prefab)
    {
        var buyable = prefab.GetComponent<Buyable>();
        if (!buyable || !Money.TrySpendMoney(buyable.cost)) return;
        var unit = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        unit.SendMessage("Command", flag.position, SendMessageOptions.DontRequireReceiver);
        MoneyEarner.ShowMoneyText(unit.transform.position, -(int)buyable.cost);
    }

    void Command(Vector3 flagPosition)
    {
        flag.position = flagPosition;
    }

    void Command(Unit unit)
    {
        Command(unit.transform.position);
    }
}


