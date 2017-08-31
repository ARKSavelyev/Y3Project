using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStats : MonoBehaviour {

    //private GameObject BaseObject;
    [System.Serializable]
    public class Stats
    {
        public int MaxHealth = 100;

        private int _curHealth;
        public int CurHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, MaxHealth); }
        }

        public void Init()
        {
            CurHealth = MaxHealth;
        }
    }
    
    public float FallBoundary = -20f;
    public Stats EntityStats;
    // Use this for initialization

    [Header("Optional: ")]
    [SerializeField]
    private StatusIndicatorScript statusIndicator;


    protected virtual void Start ()
    {
        EntityStats.Init();
        UpdateStatusIndicator();
    }
    
    public virtual void Damage(int Damage)
    {
        EntityStats.CurHealth -= Damage;
        if (EntityStats.CurHealth <= 0)
        {
            Destroy(gameObject);
        }
        UpdateStatusIndicator();
    }

    public void UpdateStatusIndicator()
    {
        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(EntityStats.CurHealth, EntityStats.MaxHealth);
        }
    } 

    public void CheckFallBoundary()
    {
        if (transform.position.y <= FallBoundary)
            Damage(1000);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
