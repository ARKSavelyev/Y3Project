using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FRobotStats : BaseStats {

    
    public int RifleAmmoAdd = 8;
    public int ShotgunAmmoAdd = 2;
    public int KillPoints = 15;
    private GameMaster OGM;
    private GameObject Player;
    // Use this for initialization
    protected override void Start () {
        base.Start();
        OGM = GameObject.Find("GM").GetComponent<GameMaster>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        CheckFallBoundary();
    }

    private void OnDestroy()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        if (Player == null)
            return;

        WeaponSwitch PWS = Player.GetComponent<WeaponSwitch>();
        PWS.AddAmmoToWeapon(1, RifleAmmoAdd);
        PWS.AddAmmoToWeapon(2, ShotgunAmmoAdd);
        OGM.IncreaseGameScore(KillPoints);
   }
}
