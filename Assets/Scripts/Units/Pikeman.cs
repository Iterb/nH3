using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pikeman : Unit, ISelectable
{
    public void SetSelected(bool selected)
    {
        healthBar.gameObject.SetActive(selected);
        selectionIndicator.gameObject.SetActive(selected);
    }
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        //GameController.SoliderList.Add(this);
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override void DealDamage()
    {
        base.DealDamage();
    }

    public override void ReciveDamage(float damage, Vector3 damageDealerPosition)
    {
        base.ReciveDamage(damage, damageDealerPosition);
        animator.SetTrigger("Get Hit");
    }
}
