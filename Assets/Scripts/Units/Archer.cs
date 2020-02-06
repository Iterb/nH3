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

    void ReleaseArrow()
    {
        Shoot();
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
    void Shoot()
    {
        float gravity = 30;
        if (!target) return;
        Vector3 direction = transform.forward;
        float distance = Vector3.Magnitude(target.position - transform.position);
        float velocity;
        velocity = Mathf.Sqrt(distance * gravity);
        flyTime = (2 * velocity * 0.707f) / gravity;
        arrow = Instantiate(arrowPrefab, transform).GetComponentInChildren<Arrow>();
        arrow.transform.localPosition += new Vector3(-0.6f, 1.37f, 0f);
        arrow.transform.eulerAngles += new Vector3(-15.66f, -90f, 0f);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        float force = rb.mass * velocity / Time.fixedDeltaTime;
        rb.AddForce(-(transform.right.x * force * 0.707f), force * 0.707f, -(transform.right.z * force * 0.707f));
        
    }

    public override void ReciveDamage(float damage, Vector3 damageDealerPosition)
    {
        base.ReciveDamage(damage, damageDealerPosition);
        animator.SetTrigger("Get Hit");
    }
}
