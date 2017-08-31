using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{

    public struct WeaponState
    {
        public GameObject Weapon;
        public bool Active;
    }
    private int CurrentWeapon;
    public GameObject position;
    public int currentWeapon = 0;
    private int nrWeapons;
    public WeaponState[] WeaponArray;
    int[,] OffsetArray;
    private Animator animator;

    void Start()
    {
        OffsetArray = new int[,] {  {65, 45 }, {-36, 23} , {-15, 62 } };

        animator = GetComponent<Animator>();
        WeaponArray = new WeaponState[3];
        LoadWeapon("Rifle", 1, true);
        LoadWeapon("Shotgun", 2, true);
        LoadWeapon("Pistol", 0, true);
        WeaponArray[1].Weapon.SetActive(false);
        WeaponArray[2].Weapon.SetActive(false);
        PlaceWeapon(WeaponArray[0].Weapon);
        ChangeOffset(0);
        CurrentWeapon = 0;
        animator.SetLayerWeight(1, 1.0f);
    }

    void LoadWeapon(string name, int i, bool status)
    {
        GameObject temp = (Resources.Load(name) as GameObject);
        WeaponArray[i].Weapon = Instantiate(temp, position.transform.position, position.transform.rotation, position.transform);
        WeaponArray[i].Weapon.transform.parent = position.transform;
        WeaponArray[i].Active = status;
    }

    void PlaceWeapon(GameObject Weapon)
    {
        Weapon.transform.position = position.transform.position;
        Weapon.transform.parent = position.transform;
    }

    void Update()
    {
        for (int i = 1; i <= WeaponArray.Length; i++)
        {
            if (Input.GetKeyDown("" + i))
            {
                SwitchWeapon(i - 1);

            }
        }
    }
    //change offset for arm animation rotation
    private void ChangeOffset(int i)
    {
        GameObject Arm = transform.Find("MainBody/StickUpTorso/RightUpperArm").gameObject;
        SetOffset(Arm, i, 0);
        Arm = transform.Find("MainBody/StickUpTorso/LeftUpperArm").gameObject;
        SetOffset(Arm, i, 1);
    }

    private void SetOffset(GameObject _Arm, int y, int count)
    {
        ArmMovement ArmScript = _Arm.GetComponent<ArmMovement>();
        ArmScript.rotationOffset = OffsetArray[y, count];
    }

    public void AddAmmoToWeapon(int WeaponN, int AmmoAmount)
    {
        BaseShoot weapon = WeaponArray[WeaponN].Weapon.GetComponent<BaseShoot>();
        weapon.AddAmmo(AmmoAmount);
    }
    
    public int GetCurrentWeaponAmmo()
    {
        return WeaponArray[CurrentWeapon].Weapon.GetComponent<BaseShoot>().GetAmmo();
    }

    void SwitchWeapon(int index)
    {
        //Check if Weapon Is Available
        if (WeaponArray[index].Active == true)
        {
            for (int i = 0; i < WeaponArray.Length; i++)
            {
                
                if (i == index)
                {
                    //Set Weapon GameObject Active
                    WeaponArray[i].Weapon.SetActive(true);
                    //Set it's animation layer's weight to 1
                    animator.SetLayerWeight(i+1, 1.0f);
                    //change offset for Aim animation
                    ChangeOffset(i);
                    //set current weapon
                    currentWeapon = i;
                }
                else
                {
                    //if not the weapon chosen, set it as inactive and disable 
                    //it's animation layer
                    WeaponArray[i].Weapon.SetActive(false);
                    animator.SetLayerWeight(i + 1, 0f);
                }
            }
        }
    }
}