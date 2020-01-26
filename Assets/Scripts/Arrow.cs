using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Archer
{
    Rigidbody rb;
    Vector3 arrowOrientation;
    Archer archer;
    protected override void Start()
    {
        //var rotation = transform.localEulerAngles;
        //rotation.y = 30;
        //transform.localEulerAngles = rotation;

        //arrowOrientation = archer.transform.localEulerAngles;

        transform.SetParent(null);
    }
    protected override void Awake()
    {
    }
    public override void DealDamage()
    {
        base.DealDamage();
    }
    protected override void Update()
    {
        //transform.rotation = Quaternion.LookRotation(rb.velocity);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision != null)
        {
            var unit = collision.gameObject.GetComponent<Unit>();

            if (unit)
            {
                Debug.Log("Trafiono w : " + unit.ToString());
                if (unit.unitStatus == UnitStatus.enemy || unit.unitStatus == UnitStatus.neutral)
                {
                    DealDamage(unit);
                }
            }
            //Debug.Log(collision.ToString());
            //Destroy(this);
        }
    }
}


    //protected override void OnTriggerEnter(Collider other)
    //{
    //
    //}

    // Update is called once per frame
