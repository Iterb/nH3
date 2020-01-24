using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Unit, ISelectable
{
    [SerializeField]
    GameObject arrowPrefab;

    protected Arrow arrow;
    public void SetSelected(bool selected)
    {
        //arrowPrefab = Resources.Load("arrow") as GameObject;
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

    void ReleaseArrow()
    {
        if (Shoot())
        {

        }
    }
    public override void DealDamage()
    {
        if (Shoot())
        {
            base.DealDamage();
        }

        
    }
    protected override void Attacking()
    {
        if (target)
        {
            nav.velocity = Vector3.zero;
            transform.LookAt(target);
            transform.Rotate(0,90,0);
                //obracanie w strone targetu
            float distance = Vector3.Magnitude(target.position - transform.position);
            if (distance <= attackDistance)
            {
                if ((attackTimer -= Time.deltaTime) <= 0) Attack();
            }

            else
            {
                task = Task.chase;
            }
        }
        else
        {
            task = Task.idle;
        }
    }
    bool Shoot()
    {
        //Vector3 start = muzzleEffect.transform.position;
        Vector3 direction = transform.forward;
        arrow = Instantiate(arrowPrefab, transform).GetComponent<Arrow>();
        arrow.transform.position += new Vector3(0f, 1.37f, 0.6f);
        arrow.transform.eulerAngles += new Vector3(-10.66f, -90, 0f);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.velocity = -(transform.right * 30);


        //RaycastHit hit;
        //if (Physics.Raycast(start, direction, out hit, attackDistance, shootingLayerMask))
        //{
        //    StartShootEffect(start, hit.point, true);
        //    var unit = hit.collider.gameObject.GetComponent<Unit>();
        //    return unit; //true jezeli trafiono w gameobject unit
        //}
        //StartShootEffect(start, start + direction * attackDistance, false);
        return false;
    }

    public override void ReciveDamage(float damage, Vector3 damageDealerPosition)
    {
        base.ReciveDamage(damage, damageDealerPosition);
        animator.SetTrigger("Get Hit");
    }
}
