using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    public GameObject Projectile;
    public GameObject SpawnParticles;
    public GameObject DestroyParticles;
    public int Damage;
    public float MinDistanceToTarget = 8;
    public float MaxDistanceToTarget = 14;
    public float TimeBetweenShots = 1;
    [SerializeField]
    protected LayerMask WhatIsObstacle;
    protected Transform FirePoint;
    protected Transform AimPosition;
    protected GameObject target;
    protected bool pathIsEnded = false;
    protected bool StartedShooting = false;

    protected float timeToFire = 0f;
    // Use this for initialization


    protected void ResetFireTime()
    {
        timeToFire = Time.time + TimeBetweenShots;
    }


    protected virtual void Start ()
    {
        SpawnEffect(SpawnParticles, 2f);
    }

    //method for using particle effect
    //use
    protected void SpawnEffect(GameObject Particles, float DestructionDelay)
    {
        GameObject PartClone = Instantiate(Particles, transform.position, transform.rotation);
        Destroy(PartClone, DestructionDelay);
    }

    // Update is called once per frame
    protected virtual void Update ()
    {
        if (target == null)
        {
            GetTarget();
            return;
        }
    }
    
    //method for getting reference to player
    protected virtual void GetTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    protected virtual void FireProjectile(Vector2 dir)
    {
        GameObject BulletClone;
        Rigidbody2D Bullet;
        BulletClone = (Instantiate(Projectile, FirePoint.position, FirePoint.rotation));
        BulletClone.layer = gameObject.layer;
        BulletProperties Prop = BulletClone.GetComponent<BulletProperties>();
        Prop.SetDamage(Damage);
        Bullet = BulletClone.GetComponent<Rigidbody2D>();
        Bullet.velocity = dir * 50;
        Object.Destroy(BulletClone, 3.0f);
    }

    protected virtual void Shoot()
    {
        Vector2 FirePointPosition = new Vector2(FirePoint.position.x, FirePoint.position.y);
        Vector2 Aim = new Vector2(AimPosition.position.x, AimPosition.position.y);
        Vector2 direction = Aim - FirePointPosition;
        direction.Normalize();
        FireProjectile(direction);
    }

    protected virtual void Aim(Transform WhatToAim)
    {
        if (target == null)
            return;
        Vector2 difference = target.transform.position - transform.position;
        difference.Normalize();
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        WhatToAim.rotation = Quaternion.Euler(0f, 0f, rotZ);
    }

    protected float GetTheSqrDistance(Vector2 A, Vector2 B)
    {
        Vector2 offset = A - B;
        return offset.sqrMagnitude;
    }

    protected bool InSight()
    {
        Vector2 dir = target.transform.position - FirePoint.position;
        dir.Normalize();
        RaycastHit2D hit = Physics2D.Raycast(FirePoint.position, dir, MaxDistanceToTarget, WhatIsObstacle);//, WhatIsEnemy);
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            return true;
        }
        return false;
    }

    protected virtual void OnDestroy()
    {
        SpawnEffect(DestroyParticles, 2f);
        //need to play destroy sound on global AudioManager, since this object gets destroyed 
        AudioMaster GlobalAudioM = GameObject.Find("GM").GetComponent<AudioMaster>();
        if (GlobalAudioM == null)
            return;
        GlobalAudioM.PlaySound("DestroySound");
    }
}
