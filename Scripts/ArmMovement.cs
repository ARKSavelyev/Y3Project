using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmMovement : MonoBehaviour
{
    Animator animator;
    public int rotationOffset = 90;
    //Use this for initialization
    void Start()
    {
        animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (animator.GetBool("IsAiming") == true)
        {
            int mod = 1;
            int i = 0;
            Vector3 theScale = GameObject.FindGameObjectWithTag("Player").transform.localScale;
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            difference.Normalize();
            if (theScale.x < 0)//((Input.mousePosition.x < transform.position.x) & (theScale.x < 0))
            {
                mod = -1 * mod;
                i = 180;
            }
            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ + mod * rotationOffset + i);
        }
    }
}