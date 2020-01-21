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
    void Command(Vector3 destination)
    {
        if (!IsAlive) return;
        nav.SetDestination(destination);
        task = Task.move;
    }
    void CommandFollow(Unit unitToFollow)
    {
        if (!IsAlive) return;
        target = unitToFollow.transform;
        task = Task.follow;
    }
    void CommandAttack(Unit enemyToAttack)
    {
        if (!IsAlive) return;
        target = enemyToAttack.transform;
        task = Task.chase;
    }

    public override void DealDamage()
    {
        base.DealDamage();
    }

    bool Shoot()
    {
        Vector3 direction = transform.forward;
        return false;

    }


    public override void ReciveDamage(float damage, Vector3 damageDealerPosition)
    {
        base.ReciveDamage(damage, damageDealerPosition);
        animator.SetTrigger("Get Hit");
    }
}
