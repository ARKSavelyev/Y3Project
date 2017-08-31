using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairScript : MonoBehaviour {

    private Transform Aim;
    private Transform Crosshair;
    private Vector3 direction;
    private float distance;
    //private Vector2 mousePosition;
	// Use this for initialization
	void Start () {
        Aim = transform.Find("AimPoint");
        Crosshair = transform.Find("Crosshair");
        direction = Aim.transform.position-transform.position;
        //distance = direction.magnitude;
        direction.Normalize();
	}
	
	// Update is called once per frame
	void Update () {
        float mouseX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        float mouseY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        Vector2 mousePosition = new Vector2(mouseX, mouseY);
        distance = Vector2.Distance(mousePosition,transform.position );
        //Vector2 endPosition = transform.position + distance * direction;
        Crosshair.position = transform.position;
        Crosshair.position += distance * direction;


    }
}
