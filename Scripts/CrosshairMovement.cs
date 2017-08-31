using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float mouseX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        float mouseY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        Vector2 followthecursor = new Vector2(mouseX, mouseY);
        transform.position = followthecursor;// ((Vector2)transform.position + (Vector2)followthecursor);
    }
}
