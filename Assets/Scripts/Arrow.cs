using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Archer
{
    Collider[] colliders;
    Rigidbody rb;
    Vector3 arrowOrientation;
    Archer archer;
    protected override void Start()
    {
        //var rotation = transform.localEulerAngles;
        //rotation.y = 30;
        //transform.localEulerAngles = rotation;
        colliders = GetComponents<Collider>();
        rb = transform.GetComponent<Rigidbody>();
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
        SpinArrow();
    }
    //animacja lotu strzały
    void SpinArrow()
    {
        float yVelocity = rb.velocity.y;
        float zVelocity = rb.velocity.z;
        float xVelocity = rb.velocity.x;
        float combinedVelocity = Mathf.Sqrt(xVelocity * xVelocity + zVelocity * zVelocity);
        float fallAngle = -1*Mathf.Atan2(yVelocity, combinedVelocity) * 180/Mathf.PI;

        transform.eulerAngles = new Vector3(fallAngle, transform.eulerAngles.y, transform.eulerAngles.z);

    }

    private void OnCollisionEnter(Collision collision)
        //collider strzały
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
                    transform.SetParent(unit.transform);
                    


                }
            }
            
            //Debug.Log(collision.ToString());
            //
        }
    }

    protected override void OnTriggerEnter(Collider other)
        //mały trigger na poczatku prefaba strzały
    {
        if (other is SphereCollider) return;
        Embed();
    }
    void Embed()
        //zachowanie strzały po wbicu w ziemię
    {
        transform.GetComponent<Arrow>().enabled = false;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        foreach(Collider collider in colliders)
        {
            collider.enabled = false;
        }
        Invoke("DestroyArrow", 3);
    }

    void DestroyArrow()
    {
        Destroy(this.gameObject);
    }
}


