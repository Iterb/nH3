using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Unit, ISelectable
{
    [SerializeField]
    GameObject arrowPrefab;
    

    protected Arrow arrow;
    protected float flyTime;
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
        base.DealDamage();
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
        Vector3 direction = transform.forward;
        float distance = Vector3.Magnitude(target.position - transform.position);
        float velocity;
        velocity = Mathf.Sqrt(distance * 9.81f);
        flyTime = (2 * velocity * 0.707f) / 9.81f;
        

        arrow = Instantiate(arrowPrefab, transform).GetComponentInChildren<Arrow>();
        Debug.Log(arrow.ToString());
        arrow.transform.localPosition += new Vector3(-0.6f, 1.37f, 0f);
        arrow.transform.eulerAngles += new Vector3(-15.66f, -90f, 0f);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        Animator animator = arrow.GetComponentInParent<Animator>();
        float force = rb.mass * velocity / Time.fixedDeltaTime;
        animator.SetFloat("Fly time", (1f / flyTime));
        Debug.Log("ArcherDebug: " + 1f / flyTime);
        rb.AddForce(-(transform.right.x * force * 0.707f), force * 0.707f, -(transform.right.z * force * 0.707f));
        
        return false;
    }

    public override void ReciveDamage(float damage, Vector3 damageDealerPosition)
    {
        base.ReciveDamage(damage, damageDealerPosition);
        animator.SetTrigger("Get Hit");
    }
}
