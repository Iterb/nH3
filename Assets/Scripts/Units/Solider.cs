using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solider : Unit, ISelectable
{

    [Header("Solider")]
    [Range(0, .3f), SerializeField]
    float shootDuration = 0;
    [SerializeField]
    ParticleSystem muzzleEffect, impactEffect;
    [SerializeField]
    LayerMask shootingLayerMask;

    LineRenderer lineEffect;
    Light lightEffect;
    const string EFFECTS_TAG = "Effects";
    public void SetSelected(bool selected)
    {
        healthBar.gameObject.SetActive(selected);
        selectionIndicator.gameObject.SetActive(selected);
    }

    protected override void Awake()
    {
        base.Awake();
        lineEffect = muzzleEffect.GetComponent<LineRenderer>();
        lightEffect = muzzleEffect.GetComponent<Light>();
        impactEffect.transform.SetParent(null); // zeby ruch solidera podczas strzalu nie wplywal na miejsce impactu
        var effects = GameObject.FindGameObjectWithTag(EFFECTS_TAG);
        if (effects) impactEffect.transform.SetParent(effects.transform); //zmiana rodzica, ponieważ elementy UI są widoczne tylko na canvasie
        EndShootEffect();

    }
 
    protected override void Start()
    {
        base.Start();
        GameController.SoliderList.Add(this);
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
        if (Shoot())
        {
            base.DealDamage();
        }

    }

    bool Shoot()
    {
        Vector3 start = muzzleEffect.transform.position;
        Vector3 direction = transform.forward;


        RaycastHit hit;
        if (Physics.Raycast(start, direction, out hit, attackDistance, shootingLayerMask))
        {
            StartShootEffect(start, hit.point, true);
            var unit = hit.collider.gameObject.GetComponent<Unit>();
            return unit; //true jezeli trafiono w gameobject unit
        }
        StartShootEffect(start, start + direction * attackDistance, false);
        return false;
    }

    void StartShootEffect(Vector3 lineStart, Vector3 lineEnd, bool hitSomething)
    {
        if (hitSomething)
        {
            impactEffect.transform.position = lineEnd;
            impactEffect.Play();
        }
        lineEffect.SetPositions(new Vector3[] { lineStart, lineEnd });
        lightEffect.enabled = true;
        lineEffect.enabled = true;
        muzzleEffect.Play();
        Invoke("EndShootEffect", shootDuration);//wywołanie metody po pewnym czasie 
    }
    void EndShootEffect()
    {
        lightEffect.enabled = false;
        lineEffect.enabled = false;
    }

    public override void ReciveDamage(float damage, Vector3 damageDealerPosition)
    {
        base.ReciveDamage(damage, damageDealerPosition);
        animator.SetTrigger("Get Hit");
    }
}
