using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingRobotDestroy : MonoBehaviour {
    GameObject Robot;
	// Use this for initialization
	void Start () {
        Robot = transform.parent.parent.gameObject;
	}
    private void OnDestroy()
    {
        Destroy(Robot);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
