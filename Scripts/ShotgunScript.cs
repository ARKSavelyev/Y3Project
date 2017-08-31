using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunScript : BaseShoot {
    private Animator animator;
    public AudioClip Shot;
    private AudioSource source;
    [Range(0, 1)]
    [SerializeField]
    private float ShotLoudness;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        GetPoints();
        animator = transform.root.gameObject.GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    public override void Shoot()
    {
        if (GetAmmo() > 0)
            source.PlayOneShot(Shot, ShotLoudness);
        base.Shoot();
    }

    public override void GetPoints()
    {
        FirePoint = transform.Find("FirePoint");
        AimPosition = new Transform[5];
        AimPosition[0] = transform.Find("FirePoint/AimPoint");
        AimPosition[1] = transform.Find("FirePoint/AimPoint2");
        AimPosition[2] = transform.Find("FirePoint/AimPoint3");
        AimPosition[3] = transform.Find("FirePoint/AimPoint4");
        AimPosition[4] = transform.Find("FirePoint/AimPoint5");
    }
    // Update is called once per frame
    void Update()
    {
        if ((animator.GetBool("IsAiming") == true))
        {
            ShootCheck();
        }
    }
}
