using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolScript : BaseShoot {
    private Animator animator;
    public AudioClip Shot;
    private AudioSource source;
    [Range(0, 1)]    [SerializeField]    private float ShotLoudness;

    // Use this for initialization
    protected override void Start ()
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
    // Update is called once per frame
    void Update ()
    {
        if ((animator.GetBool("IsAiming") == true))
        {
            ShootCheck();
        }
   }
}
