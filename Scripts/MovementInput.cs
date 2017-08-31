using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementScript))]
public class MovementInput : MonoBehaviour {

    public GameObject Particles;
    private Animator animator;
    private MovementScript MovScript;
    private bool isSlowMo = false;
    private bool isKeyDownJump = false;

    // Use this for initialization
    void Awake () {
        animator = GetComponent<Animator>();
        MovScript = GetComponent<MovementScript>();
        //allRenderers = new List<Renderer>(GetComponentsInChildren<Renderer>(true));
       }


    private void Start()
    {
        ParticleEffect();
    }

    void ParticleEffect()
    {
        GameObject PartClone = Instantiate(Particles, transform.position, transform.rotation);
        Destroy(PartClone, 2f);
    }


    // Update is called once per frame
    void Update ()
    {
        ProcessInput();
    }

    
    private void ProcessInput()
    {
        isKeyDownJump = Input.GetButtonDown("Jump");
        float inputAxisX = Input.GetAxisRaw("Horizontal");
        MovScript.Move(inputAxisX, isKeyDownJump);
        ProcessAim();
        ProcessSlowMo();
    }

    void ProcessAim()
    {
        if (Input.GetMouseButton(1))
        {
            animator.SetBool("IsAiming", true);
            animator.SetFloat("Moving Blend", 0);
        }
        else
        {
            animator.SetBool("IsAiming", false);
            animator.SetFloat("Moving Blend", 1);
        }
    }

    void ProcessSlowMo()
    {
        if (Input.GetKeyDown("e") && (isSlowMo == false))
        {
            Time.timeScale = 0.3f;
            isSlowMo = true;
        }
        else if (Input.GetKeyDown("e") && (isSlowMo == true))
        {
            Time.timeScale = 1.0f;
            isSlowMo = false;
        }
    }
    }
