using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : BaseStats
{
    
    private GameMaster OGM;
    public PlayerUIScript UIScript;
    public int RegenDelay = 3;
    public int RegenRate = 1;
    private float lastHitTime;
    private AudioMaster AudioM;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        if (UIScript == null)
            FindUIScript();
        OGM = GameObject.Find("GM").GetComponent<GameMaster>();
        AudioM = GetComponent<AudioMaster>();
        lastHitTime = Time.time;
        StartCoroutine(RegenHealth());
        UIScript.SetHealth(EntityStats.CurHealth, EntityStats.MaxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (UIScript == null)
            FindUIScript();
        if (transform.position.y <= FallBoundary)
            Damage(1000);
      
        UpdateStatusIndicator();
        CheckFallBoundary();
    }

    IEnumerator RegenHealth()
    {
       
        if (Time.time > (lastHitTime + RegenDelay))
        {
            if (EntityStats.CurHealth < EntityStats.MaxHealth)
            { // if health < 100...
                EntityStats.CurHealth += RegenRate; // increase health and wait the specified time
                UIScript.SetHealth(EntityStats.CurHealth, EntityStats.MaxHealth);
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                yield return null;
            }
        }
        else
        { 
            yield return null;
        }
        StartCoroutine(RegenHealth());
    }

    private void FindUIScript()
    {
        UIScript = GameObject.Find("PlayerUI").GetComponent<PlayerUIScript>();
    }

    public override void Damage(int Damage)
    {
        base.Damage(Damage);
        UIScript.SetHealth(EntityStats.CurHealth, EntityStats.MaxHealth);
        lastHitTime = Time.time;
        AudioM.PlaySound("DamageSound");
    
    }
    private void OnDestroy()
    {
        OGM.OnPlayerDeath();
    }

}
