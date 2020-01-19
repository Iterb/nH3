using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : Unit 
{

    [SerializeField]
    float chasingSpeed = 5;
    [SerializeField]
    float patrolRadius = 5;
    [SerializeField]
    float idlingCooldown = 2;
    [SerializeField]
    uint reward = 100;
    float normalSpeed;
    Vector3 startPoint;
    float idlingTimer;
    //[SerializeField]
    List<Solider> seenSoliders = new List<Solider>();
    Solider ClosestSolider
    {
        get
        {
            if (seenSoliders == null || seenSoliders.Count <= 0) return null;
            float minDistance = float.MaxValue;
            Solider closestSolider = null;
            foreach (Solider solider in seenSoliders)
            {
                if (!solider || !solider.IsAlive) continue;
                float distance = Vector3.Magnitude(solider.transform.position - transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestSolider = solider;
                }
            }
            return closestSolider;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        normalSpeed = nav.speed;
        startPoint = transform.position;
    }
    protected override void Start()
    {
        base.Start();
        GameController.DragonList.Add(this);
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        var solider = other.gameObject.GetComponent<Solider>();
        if (solider && !seenSoliders.Contains(solider))
        {
            seenSoliders.Add(solider);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        var solider = other.gameObject.GetComponent<Solider>();
        if (solider)
        {
            seenSoliders.Remove(solider);
        }
    }
    protected override void Idling()
    {
        base.Idling();
        UpdateSight();
        if ((idlingTimer -= Time.deltaTime) <= 0)
        {
            idlingTimer = idlingCooldown;
            task = Task.move;
            SetRandomRoamingPosition();
        }
    }

    protected override void Moving()
    {
        base.Moving();
        nav.speed = normalSpeed;
        UpdateSight();
    }

    protected override void Chasing()
    {
        base.Chasing();
        nav.speed = chasingSpeed;
    }

    void UpdateSight()
    {
        var solider = ClosestSolider;
        if (solider)
        {
            target = solider.transform;
            task = Task.chase;
        }
    }
    void SetRandomRoamingPosition()
    {
        Vector3 delta = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        delta.Normalize();
        delta *= patrolRadius;
        nav.SetDestination(startPoint + delta);
    }

    public override void ReciveDamage(float damage, Vector3 damageDealerPosition)
    {
        base.ReciveDamage(damage, damageDealerPosition);
        if (!target && IsAlive)
        {
            task = Task.move;
            nav.SetDestination(damageDealerPosition);
        }
        if (HealthPercent > .5f)
        {
            animator.SetTrigger("Get Hit");
            //nav.velocity = Vector3.zero;
        }

        if (!IsAlive && Money.TryAddMoney(reward) && reward > 0)
        {
            MoneyEarner.ShowMoneyText(transform.position, (int)reward);
            reward = 0;
        }

    }
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        if (!Application.isPlaying) startPoint = transform.position;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(startPoint, patrolRadius);
    }
}
