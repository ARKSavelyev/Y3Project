using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProperties : MonoBehaviour {
    
    private int Damage;
    private string WhatToHit;
    private AudioMaster AudioM;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BaseStats ColliderStats = collision.collider.gameObject.GetComponent<BaseStats>();
        if (ColliderStats != null)
        {
            if (gameObject.layer != collision.collider.gameObject.layer)
            { 
                ColliderStats.Damage(Damage);
            }
            AudioM.PlaySound("DidHit");
         }
        else
        {
            AudioM.PlaySound("DidNotHit");
        }
        
        Destroy(gameObject);
    }

    public void SetDamage(int newDamage)
    {
        Damage = newDamage;
    }

   
    // Use this for initialization
    void Start ()
    {
        AudioM = GameObject.Find("GM").GetComponent<AudioMaster>();
    }
	


	// Update is called once per frame
	void Update () {
		
	}
}
