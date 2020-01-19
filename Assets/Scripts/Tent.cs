using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tent : Building, ISelectable
{
    public float spawnTimer;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        //spawnTimer = 
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
        if (spawnTimer > 0) return;
        var buyable = prefab.GetComponent<Buyable>();
        if (!buyable || !Money.TrySpendMoney(buyable.cost)) return;
        var unit = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        unit.SendMessage("Command", flag.position, SendMessageOptions.DontRequireReceiver);
        Debug.Log(buyable.creationTime);
        spawnTimer = buyable.creationTime;
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

    protected override void CalculateCreationDelay()
    {
        base.CalculateCreationDelay();
        spawnTimer -= Time.deltaTime;
    }
}


