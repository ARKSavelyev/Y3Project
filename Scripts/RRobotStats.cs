using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RRobotStats : BaseStats{
    public GameObject CurrentZone;
    private GameObject DefaultZone;
    public int RifleAmmoAdd = 8;
    public int ShotgunAmmoAdd = 2;
    public int KillPoints = 15;

    private GameMaster OGM;
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        OGM = GameObject.Find("GM").GetComponent<GameMaster>();
        DefaultZone = OGM.GetInitialZone();
        CurrentZone = DefaultZone;
    }

    // Update is called once per frame
    void Update()
    {
        CheckFallBoundary();
        if (CurrentZone == null)
            CurrentZone = DefaultZone;
    }

    public GameObject GetCurrentZone()
    {
        return CurrentZone;
    }

    public void SetCurrentZone(ref GameObject NewZone)
    {
        CurrentZone = NewZone;
    }

    private void OnDestroy()
    {
        WeaponSwitch PWS = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponSwitch>();
        PWS.AddAmmoToWeapon(1, RifleAmmoAdd);
        PWS.AddAmmoToWeapon(2, ShotgunAmmoAdd);
        OGM.IncreaseGameScore(KillPoints);
    }
}
