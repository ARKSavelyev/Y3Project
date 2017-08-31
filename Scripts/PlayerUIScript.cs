using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIScript : MonoBehaviour {

    [SerializeField]
    private Text HealthText;
    [SerializeField]
    private Text AmmoText;
    [SerializeField]
    private Text ScoreText;
    [SerializeField]
    private Text LivesText;

    private PlayerStats PS;
    private WeaponSwitch WS;
    // Use this for initialization
    void Start () {
		
	}
	
    void GetTargets()
    {
      //  GameObject PC = GameObject.FindGameObjectWithTag("Player");
      //  PS = PC.GetComponent<PlayerStats>();
      //  WS = PC.GetComponent<WeaponSwitch>();

    }

    public void SetAmmo(int CurrentAmmo)
    {
        AmmoText.text = CurrentAmmo.ToString();
    }
    
    public void SetHealth(int cur, int max)
    {
        HealthText.text = cur + "/" + max;
    }

    public void SetScore(int newScore)
    {
        Debug.Log("Points Added"); 
        ScoreText.text = newScore.ToString();
    }

    public void SetLives(int newLives)
    {
        LivesText.text = newLives.ToString();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
