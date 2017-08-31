using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]

public class FRobotAI : EnemyAI {

    [SerializeField] protected  AudioMaster AudioM;
    private Seeker seeker;
    private Rigidbody2D RBody;
    private Transform OuterRing;
    public float speed = 300f;
    public ForceMode2D fMode;

    // How many times each second we will update our path
    public float updateRate = 2f;

    // The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3;

    // The waypoint we are currently moving towards
    private int currentWaypoint = 0;

    //The calculated path
    public Path path;

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
        OuterRing = transform.Find("Center/OuterRing");
        FirePoint = transform.Find("Center/OuterRing/FirePoint");
        AimPosition = transform.Find("Center/OuterRing/FirePoint/AimPoint");

        seeker = GetComponent<Seeker>();
        RBody = GetComponent<Rigidbody2D>();
        GetTarget();

        // Start a new path to the target position, return the result to the OnPathComplete method
        seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
        StartCoroutine(UpdatePath());
    }

    // Update is called once per frame
    protected override void Update ()
    {
        base.Update();
        Debug.Log(currentWaypoint);
        Aim(OuterRing);
        if (path == null)
            return;
        if ((GetTheSqrDistance(transform.position, target.transform.position)) < (MaxDistanceToTarget * MaxDistanceToTarget))
        {
            if (pathIsEnded)
            {
                 if (!StartedShooting)
                 {
                     StartCoroutine(Shooting());
                     StartedShooting = true;
                  }
                //Shooting();
                return;
            }
            if (((GetTheSqrDistance(transform.position, target.transform.position)) < (MinDistanceToTarget * MinDistanceToTarget)) && InSight())
            {
                if (!pathIsEnded)
                    RBody.velocity = new Vector2(0, 0);
                Debug.Log("End of path reached.");
                pathIsEnded = true;
                return;
            }
        }
        pathIsEnded = false;
        //Direction to the next waypoint
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;
        //Move the AI
        RBody.AddForce(dir, fMode);
        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (dist < nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }
    }

    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            GetTarget();

        }
        else
        {
            //Create new Path
            seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
            //yield for a period of time
            yield return new WaitForSeconds(1f / updateRate);
            //Start Again
            StartCoroutine(UpdatePath());
        }

    }

    protected override void GetTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        if (target == null)
            return;
        seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
        StartCoroutine(UpdatePath());
    }

    public void OnPathComplete(Path p)
    {
        //Debug.Log("We got a path. Did it have an error? " + p.error);
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

     IEnumerator Shooting()
     {
         yield return new WaitForSeconds(TimeBetweenShots);
         StartedShooting = false;
         if (pathIsEnded)
         {
             Shoot();
             AudioM.PlaySound("Shot");
         }

     }

   
}
