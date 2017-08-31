using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShoot : MonoBehaviour {

    public enum ShootMode { AUTOMATIC, SINGULAR };


    public float firerate = 0;
    public int Damage = 10;
    public GameObject Projectile;
    public GameObject MuzzleFlash;
    public float BulSpeed = 60;
    [SerializeField] private int Ammo;
    //[Range(0, 1)]    [SerializeField]    private float ShotLoudness;
    public ShootMode Mode = ShootMode.SINGULAR;
    private float timeToFire;
    public Transform FirePoint;
    public Transform[] AimPosition;
    private PlayerUIScript UIScript;
   
    // Use this for initialization
    protected virtual void Start ()
    {
        FindUIScript();
        UIScript.SetAmmo(Ammo);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FindUIScript()
    {
        UIScript = GameObject.Find("PlayerUI").GetComponent<PlayerUIScript>();
    }

    private void OnEnable()
    {
        FindUIScript();
        UIScript.SetAmmo(Ammo);
    }


    public virtual void Shoot()
    {
        //Check if we have ammo
        if (Ammo > 0)
        {
            Vector2 FirePointPosition = new Vector2(FirePoint.position.x, FirePoint.position.y);
            //if we have multiple aimpoints, shoot in direction of each
            for (int i = 0; i < AimPosition.Length; i++)
            {
                Vector2 Aim = AimPosition[i].position;
                Vector2 direction = Aim - FirePointPosition;
                direction.Normalize();
               
                FireBullet(direction);
            }
            //if more ammo than 10000, then it's unlimited
            if (Ammo < 10000)
                Ammo--;
            UIScript.SetAmmo(Ammo);
        }
    }

    //add Ammo
    public void AddAmmo(int AmmoDelta)
    {
        Ammo = Ammo + AmmoDelta;
        UIScript.SetAmmo(Ammo);
    }

    public int GetAmmo()
    {
        return Ammo;
    }
    
    //get fire and aim points
    public virtual void GetPoints()
    {
        FirePoint = transform.Find("FirePoint");
        AimPosition =new Transform[] { transform.Find("FirePoint/AimPoint") };
    }

    public virtual void ShootCheck()
    {
        bool firecheck;
        if (Mode == ShootMode.SINGULAR)
        {
            firecheck = Input.GetButtonDown("Fire1");
        }
        else
        {
            firecheck = Input.GetButton("Fire1");
        }
        if (firerate == 0)
        {
                if (firecheck)
                {
                    Shoot();
                }
        }
        else
        {
                if (firecheck && (Time.time > timeToFire))
                {
                    timeToFire = Time.time + 1 / firerate;
                    Shoot();
                }
        }
    }

    private void SpawnMuzzleFlash()
    {
        GameObject MuzzleClone;
        MuzzleClone = Instantiate(MuzzleFlash, FirePoint.position, FirePoint.rotation);
        MuzzleClone.transform.parent = FirePoint;
        MuzzleClone.transform.localScale = new Vector2(0.5f, 0.5f);
        Object.Destroy(MuzzleClone, 0.1f);
    }

    private void FireBullet(Vector2 dir)
    {
        GameObject BulletClone;
        Rigidbody2D Bullet;
        //Spawn Muzzle Flash
        SpawnMuzzleFlash();
        //spawn bullet
        BulletClone = (Instantiate(Projectile, FirePoint.position, FirePoint.rotation));
        //set the same layer as weapon (so it wouldn't collide)
        BulletClone.layer = gameObject.layer;
        BulletProperties Prop = BulletClone.GetComponent<BulletProperties>();
        //set bullet damage
        Prop.SetDamage(Damage);
        Bullet = BulletClone.GetComponent<Rigidbody2D>();
        Bullet.velocity = dir * BulSpeed;
        //set destruction timer, just in case
        Object.Destroy(BulletClone, 2.0f);
     }
}
