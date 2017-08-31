using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneScript : MonoBehaviour {

    //public int MaxEnemies;
    //public int CurrentNofEnemies;
    private GameMaster OGM;
	// Use this for initialization
	void Start () {
        GameObject _OGM = GameObject.Find("GM");
        OGM = _OGM.GetComponent<GameMaster>();
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Collider ent = collision.GetComponent<>
        if (collision.gameObject.tag == "Player")
        {
            OGM.SetPlayerZone(gameObject);
        }

        else if (collision.gameObject.tag == "EnemyRiding")
        {

            GameObject enemy = collision.gameObject;
           RRobotStats ST = enemy.GetComponent<RRobotStats>();
            if ((ST != null) && (ST.CurrentZone != gameObject))
            {
                ST.CurrentZone = gameObject;
            }

        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            OGM.SetPlayerZone(null);
        }

        else if (collision.gameObject.tag == "EnemyRiding")
        {
            GameObject enemy = collision.gameObject;
            RRobotStats ST = enemy.GetComponent<RRobotStats>();
            if (ST != null)
                ST.CurrentZone = null;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (OGM.GetPlayerZone() != gameObject)
            {
                OGM.SetPlayerZone(gameObject);
            }
        }

        if (collision.gameObject.tag == "EnemyRiding")
        {
            GameObject enemy = collision.gameObject;
            RRobotStats ST = enemy.GetComponent<RRobotStats>();
                if ((ST != null) &&(ST.CurrentZone != gameObject))
                {
                    ST.CurrentZone = gameObject;
                }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
