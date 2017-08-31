using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    public GameObject TriggerZones;

    public Transform PlayerSpawnPoint;
    public GameObject Player;
    public PlayerUIScript UIScript;
    public GameObject GameOver;
    [SerializeField] private int MaxLives = 3;
    [SerializeField] private AudioMaster AudioM;
    private int _PlayerLives;
    public int PlayerLives
    {
        get { return _PlayerLives; }
    }

    public int spawnDelay = 2;
    private GameObject CurrentPlayerZone;
    private int GameScore = 0;
    public GameObject[] Enemies;

    // Use this for initialization

    void Start()
    {
        _PlayerLives = MaxLives;
        UIScript.SetScore(GameScore);
        UIScript.SetLives(PlayerLives);
        CurrentPlayerZone = GetInitialZone();
        Cursor.visible = false;
        SpawnPlayer();
        AudioM.PlaySound("BackgroundMusic");
    }

    public IEnumerator RespawnPlayer()
    {
        AudioM.PlaySound("RespawnCountdown");
        yield return new WaitForSeconds(spawnDelay);
        AudioM.PlaySound("SpawnEffect");
        Instantiate(Player, PlayerSpawnPoint.position, PlayerSpawnPoint.rotation);
    }

    public void SpawnPlayer()
    {
        Debug.Log("Spawn Player GameMaster");
        Instantiate(Player, PlayerSpawnPoint.position, PlayerSpawnPoint.rotation);
        //AudioM.PlaySound("SpawnEffect");
    }
    public void OnPlayerDeath()
    {
        if (_PlayerLives > 0)
        {
            _PlayerLives--;
            UIScript.SetLives(PlayerLives);
            StartCoroutine(RespawnPlayer());
        }
        else
        {
            EndGame();
        }
    }

    public void DestroyRequiredEnemy(string requiredTag)
    {
        Enemies = GameObject.FindGameObjectsWithTag(requiredTag);
        if (Enemies != null || Enemies.Length != 0)
            foreach (GameObject Enemy in Enemies)
            {
                Object.Destroy(Enemy, 0f);
            }

    }

    

    public void EndGame()
    {
        Debug.Log("EndGame");
        AudioM.StopSound("BackgroundMusic");
        AudioM.PlaySound("GameOver");
        DestroyRequiredEnemy("EnemyFlying");
        DestroyRequiredEnemy("EnemyRiding");
        GameOver.SetActive(true);
        Cursor.visible = true;
    }

    public void IncreaseGameScore(int delta)
    {
        GameScore += delta;
        Debug.Log(GameScore);
        UIScript.SetScore(GameScore);
    }
	// Update is called once per frame
	void Update () {


        if (CurrentPlayerZone == null)
            CurrentPlayerZone = GetInitialZone();
        //Debug.Log(CurrentPlayerZone.name);
	}

    public void SetPlayerZone(GameObject Zone)
    {
        CurrentPlayerZone = Zone;
    }

    public GameObject GetPlayerZone()
    {
        return CurrentPlayerZone;
    }
    public GameObject GetInitialZone()
    {
        Transform Bt = TriggerZones.transform.Find("BottomZone");
        GameObject Bot = Bt.gameObject;
        return Bot;
    }

    
}
