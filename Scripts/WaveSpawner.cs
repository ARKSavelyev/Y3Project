using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {

    public enum SpawnState  {SPAWNING, WAITING, COUNTING};

    [System.Serializable]
    public class Wave
    {
        public string name;
        public GameObject FlyingR;
        public int CountF;
        public GameObject RidingR;
        public int CountR;
        public int EnemiesAtOnce;
        public float rate;
    }

    public GameMaster OGM;
    public Wave[] Waves;
    public int NextWave = 0;
    public float TimeBetweenWaves = 5f;
    public float WaveCountDown;
    public GameObject Player;
    public Transform PlayerSpawnPoint;
    public SpawnState state = SpawnState.COUNTING;
    
    
    
    // Use this for initialization
    private Transform[] FlyingSP;
    private Transform[] RidingSP;
    private float SearchCountdown = 1f;


    void Start ()
    {
        WaveCountDown = TimeBetweenWaves;
        GetSpawnZones();
        //SpawnPlayer();
    }

  

    public void SpawnPlayer()
    {
        

        Instantiate(Player, PlayerSpawnPoint.position, PlayerSpawnPoint.rotation);
    }

    // Update is called once per frame
    void Update ()
    {
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
                return;
            }
            else
            {
                return;
            }
        }

		if (WaveCountDown<0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(Waves[NextWave]));
            }
        }
        else
        {
            WaveCountDown -= Time.deltaTime;
        }
	}

    void GetSpawnZones()
    {
        GameObject zone = OGM.GetInitialZone();
        FlyingSP = new Transform[] {zone.transform.Find("Middle") };
        RidingSP = new Transform[] { zone.transform.Find("Right"), zone.transform.Find("Left") };
        //Debug.Log(RidingSP.Length);
    }

    void WaveCompleted()
    {
        state = SpawnState.COUNTING;
        WaveCountDown = TimeBetweenWaves;

        if (NextWave + 1 > Waves.Length - 1)
        {
            NextWave = 0;
        }
        else
        {
            NextWave++;
        }
    }


    bool EnemyIsAlive ()
    {
        SearchCountdown -= Time.deltaTime;
        if (SearchCountdown <= 0f)
        {
            SearchCountdown = 1f;
            if ((GameObject.FindGameObjectWithTag("EnemyFlying") == null) && (GameObject.FindGameObjectWithTag("EnemyRiding") == null))
            {
                return false;
            }
        }

       

        return true;
    }

    IEnumerator SpawnWave (Wave _wave)
    {
        state = SpawnState.SPAWNING;
        for (int i = 0; i < _wave.CountF; i++)
        {
            SpawnEnemy(_wave.FlyingR, FlyingSP);
            yield return new WaitForSeconds(1f / _wave.rate);
        }
        for (int t = 0; t < _wave.CountR; t++)
        {
            SpawnEnemy(_wave.RidingR, RidingSP);
            yield return new WaitForSeconds(1f / _wave.rate);
        }
        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnEnemy(GameObject _enemy, Transform[] SpawnPoints)
    {

        Transform sp = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
            Instantiate(_enemy, sp.position, sp.rotation);
            Debug.Log(_enemy.name);
        
        /*if (!IsTaken(transform, 1.1f))
        {
            Instantiate(_enemy, transform.position, transform.rotation);
        }
        else
        {
            Instantiate(_enemy, new Vector2 (transform.position.x-1.5f,transform.position.y-1.5f), transform.rotation);
        }
        //Instantiate(_enemy, transform.position, transform.rotation);
        */
    }

    public bool IsTaken(Transform Pos, float radius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(Pos.position, radius);
        //bool isObject = false;
        if (colliders.Length != 0)
        {
            return true;
        }
        return false;

    }
}
