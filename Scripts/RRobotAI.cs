using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RRobotAI : EnemyAI {

    [SerializeField]
    protected AudioMaster AudioM;
    public float MaxSpeed = 3f;
    public float TeleportDelay = 2f;
    private Transform Gun;
    private Transform GroundCheck;
    private Transform Ahead;
    private Transform Wheel;
    private Rigidbody2D RBody;
    private bool isOnGround = false;
    private bool isMoving = false;
    private bool WasFacingRight = true;
    private GameMaster OGM;
    private RRobotStats Stats;
    private Transform[] SpawnPoints;
    private float ZoneEnterTime;
    private bool WasSameZone;
    private bool startedTeleporting = false;
    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
        Gun = transform.Find("MainBody/Gun");
        FirePoint = transform.Find("MainBody/Gun/FirePoint");
        AimPosition = transform.Find("MainBody/Gun/FirePoint/AimPoint");
        Wheel = transform.Find("MainBody/Wheel");
        GroundCheck = transform.Find("GroundCheck");
        Ahead = transform.Find("AheadCheck");
        RBody = gameObject.GetComponent<Rigidbody2D>();
        OGM = GameObject.Find("GM").GetComponent<GameMaster>();
        Stats = gameObject.GetComponent<RRobotStats>();
    }
	
	// Update is called once per frame
	protected override void Update ()
    {
        if (target == null)
            GetTarget();
        Aim(Gun);
        isOnGround = Check(GroundCheck);
        Move();
        Shooting();
        Flip();
        RotateWheel();
        if (!CompareZones())
        {
           if (!startedTeleporting)
            {
                StartCoroutine(TeleportRoutine());
                startedTeleporting = true;
            }
       }
        else
        {
            WasSameZone = CompareZones();
        }
    }

    IEnumerator TeleportRoutine()
    {
        yield return new WaitForSeconds(TeleportDelay);
        if (!CompareZones())
        {
            GetSpawnPoints(OGM.GetPlayerZone());
            Teleport(SpawnPoints);
        }
        startedTeleporting = false;
    }

    protected override void Aim(Transform WhatToRotate)
    {
        int mod = 1;
        int i = 0;
        Vector2 difference = target.transform.position - transform.position;
        difference.Normalize();
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        if (!WasFacingRight)
        {
            mod *= -1;
            i = 180;
        }

        WhatToRotate.rotation = Quaternion.Euler(0f, 0f, rotZ + mod*(-7) + i);
    }

    protected void Shooting()
    {
        float diff = target.transform.position.x - transform.position.x;
        if (Mathf.Abs(diff) < MaxDistanceToTarget)
        {
            if ((pathIsEnded) || ((RBody.velocity.x == 0) && (!pathIsEnded)))
            {
                if (Time.time > timeToFire)
                {
                    ResetFireTime();
                    AudioM.PlaySound("Shot");
                    Shoot();
                }
            }
        }
        StartedShooting = false;
    }

    private void RotateWheel()
    {
        if (isMoving)
        {
            float rotZ = -RBody.velocity.x;

            Wheel.Rotate(0f, 0f, rotZ);
        }
    }

    void Move()
    {
        float diff = target.transform.position.x - transform.position.x;
        if (Mathf.Abs(diff) < MaxDistanceToTarget)
        {
            if (pathIsEnded)
            {
                
                return;
            }

            if (Mathf.Abs(diff) < MinDistanceToTarget)
            {
                pathIsEnded = true;
                RBody.velocity = new Vector2(0, RBody.velocity.y);
                isMoving = false;
                return;
            }
        }
        pathIsEnded = false;
        isMoving = true;
        if (isOnGround && Check(Ahead))
        {
            if (diff > 0)
            {
                RBody.velocity = new Vector2(MaxSpeed, RBody.velocity.y);
            }
            else
            {
                RBody.velocity = new Vector2(-MaxSpeed, RBody.velocity.y);
            }
        }
        else
        {
            RBody.velocity = new Vector2(0, RBody.velocity.y);
        }
    }

    //flip sprite depending on where is going (left or right)
    private void Flip()
    {
        float diff = target.transform.position.x - transform.position.x;
        if ((diff <= 0) & (WasFacingRight == true)) 
        {
            FlipBase();
        }
        else if ((diff >= 0) & (WasFacingRight == false))
        {
            FlipBase();
        }
    }

    private void FlipBase()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        WasFacingRight = !WasFacingRight;
    }

    private bool CompareZones()
    {
        return (Stats.CurrentZone == OGM.GetPlayerZone());
    }

    private void GetSpawnPoints(GameObject Zone)
    {
        //fill SpawnPoints array with Player's zone spawn points
        SpawnPoints = new Transform[] { Zone.transform.Find("Right"), Zone.transform.Find("Left") };
    }

    public void Teleport(Transform[] _SP)
    {
        Transform sp = GetClosestPoint(_SP);
        Debug.Log(CheckSpawn(sp, 10f));
        //Make sure spawn point is free from other riding robots 
        if (CheckSpawn(sp, 10f))
        {
            transform.position = sp.position;
            SpawnEffect(SpawnParticles, 2f);
            //change current zone to zone of player
            Stats.CurrentZone = OGM.GetPlayerZone();
            WasSameZone = true;
        }
    }

    private Transform GetClosestPoint(Transform[] _SP)
    {
        Transform closest = _SP[0];
        float CurrentDistance = GetTheSqrDistance(closest.position, target.transform.position);
        float temp = 0f;
        //iterate through array to find spawn point closer than first
        for (int i = 1; i < _SP.Length; i++)
        {
            temp = GetTheSqrDistance(_SP[i].position, target.transform.position);
            if (temp < CurrentDistance)
            {
                CurrentDistance = temp;
                closest = _SP[i];
            }
        }
        return closest;
    }


    public bool CheckSpawn(Transform Pos, float radius)
    {
        //get all colliders in radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(Pos.position, radius);
        for (int i = 0; i < colliders.Length; i++)
        {
            //if any collider is Riding Robot, return false
            if (colliders[i].gameObject.name.Contains("Riding"))
                return false;
        }
        //else return true
        return true;
    }

    private bool Check(Transform Point)
    {
        return Physics2D.Raycast(Point.position, Vector2.down, 0.4f, WhatIsObstacle);
    }
}
