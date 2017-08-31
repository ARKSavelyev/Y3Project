using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour {

   
    [SerializeField]    private float MaxSpeed = 10f;                   
    [SerializeField]    private float JumpForce = 400f;
    [SerializeField]    private LayerMask WhatIsWall;
    [SerializeField]    private float WallJumpX = 8f;
    [SerializeField]    private float WallJumpY = 10f;
    [SerializeField]    private float WallStickTime = 3f;
    [Range(0, 1)]    [SerializeField]    private float AimSpeed = 0.36f;

    

    // Use this for initialization
    private Transform LeftCheck;
    private Transform RightCheck;
    private Transform GroundCheck;

    //
    private bool isPlayerOnGround = true;
    private bool isPlayerOnWall = false;
    private bool IsOnWallLeft = false;
    private bool IsOnWallRight = false;
    private bool WasOnWall = false;
    private bool isFacingRight = true;
    private bool isFacingLeft = false;
    private bool WasFacingRight = true;
    private bool isMoving = false;
    private bool isMoveSoundPlaying = false;
    private float CurrentSpeed;
    private float WallLandTime = 0f;
    private Vector2 Normal;
    private Vector2 Direction;
    private bool JustJumped = false;
    private Rigidbody2D RBody;
    private Animator animator;
    private AudioMaster AudioM;

    void Awake ()
    {
        Direction = new Vector2(1,0);
        CurrentSpeed = MaxSpeed;
        RBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        AudioM = GetComponent<AudioMaster>();
        LeftCheck = transform.Find("LeftCheck");
        RightCheck = transform.Find("RightCheck");
        GroundCheck = transform.Find("GroundCheck");
        
    }
    
    // Update is called once per frame

    private void Update()
    {
        float axis = Input.GetAxisRaw("Horizontal");
        if (((axis <-0.5f) || (axis>0.5f)) && (isPlayerOnGround))
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
            isMoveSoundPlaying = false;
            AudioM.StopSound("Moving");
        }
        if ((isMoving) && (!isMoveSoundPlaying))
        {
            isMoveSoundPlaying = true;
            AudioM.PlaySound("Moving");
        }
            
        
    }

    private void LateUpdate()
    {
        ProcessAnimations();
        SlopeCheck();
        if (isPlayerOnGround)
        {
            CheckAimRotation();
        }

    }
    void FixedUpdate ()
    {
        //ProcessAnimations();
        CheckAllTriggers();
        animator.SetBool("InAir", !isPlayerOnGround);
        //isPlayerOnWall = IsOnWallLeft || IsOnWallRight;
        WallCheck();
        //isPlayerOnWall = ((IsOnWallLeft && isFacingLeft) || (IsOnWallRight && isFacingRight));
        animator.SetBool("IsOnWall", isPlayerOnWall);
    }


    void WallCheck()
    {
        isPlayerOnWall = ((IsOnWallLeft && isFacingLeft) || (IsOnWallRight && isFacingRight));
        if ((WasOnWall) & (isPlayerOnGround))
        {
            WasOnWall = false;
        } 
        else if (((!WasOnWall) & (isPlayerOnWall)))
        {
            WasOnWall = true;
            WallLandTime = Time.time;
        }
    }

    IEnumerator JumpRoutine()
    {
        JustJumped = true;
        yield return new WaitForSeconds(0.3f);
        JustJumped = false;

    }

    
    void SlopeCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(GroundCheck.position, Vector2.down, 0.4f);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
               
                Normal = hit.normal;
                Debug.Log(hit.normal);
                transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                Direction.x = hit.normal.y;
                Direction.y = -hit.normal.x;
            }
        }
        else
        {
            transform.rotation = Quaternion.FromToRotation(hit.normal, Vector3.up);
        }
    }

    void CheckAimRotation()
    {
        bool check = animator.GetBool("IsAiming");
        if (check)
        {
            float mouseX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            float Dir = transform.localScale.x;
            if ((Dir>0) & (mouseX<transform.position.x))
            {
                FlipBase();
            }
            else if ((Dir < 0) & (mouseX > transform.position.x))
            {
                FlipBase();
            }

        }
    }

    public void Move(float AxisX, bool JumpButton)
    {
        bool isKeyDownLeft = AxisX < -0.5f;
        bool isKeyDownRight = AxisX > 0.5f;
        isFacingRight = isKeyDownRight;
        isFacingLeft = isKeyDownLeft;

        //player is on ground
        if (isPlayerOnGround == true)
        {
            //SlopeCheck();
            bool check = animator.GetBool("IsAiming");
            if (check)
            {
                CurrentSpeed = AimSpeed * MaxSpeed;
            }
            else
            {
                CurrentSpeed = MaxSpeed;
            }

            RBody.velocity = new Vector2(AxisX * CurrentSpeed, RBody.velocity.y);
            AudioM.PlaySound("Moving");
            if (JumpButton)
            {
                StartCoroutine(JumpRoutine());
                RBody.AddForce(new Vector2(0f, JumpForce));
                AudioM.PlaySound("Jump");
                isPlayerOnGround = false;
            }
        }

        else
        {
            if (isPlayerOnWall)
            {
              if ((Time.time < (WallLandTime+WallStickTime))&& (!JumpButton) && (RBody.velocity.y<0) )
                {
                    RBody.velocity = new Vector2(RBody.velocity.x, 0);
                }

                else if (JumpButton)
                {
                   WasOnWall = false;
                    if (IsOnWallLeft == true)
                    {
                        if (RBody.velocity.y <= 0)
                        {
                            RBody.velocity = new Vector2(WallJumpX, WallJumpY);
                            AudioM.PlaySound("Jump");
                            FlipBase();
                        }
                        else
                        {
                            RBody.velocity = new Vector2(WallJumpX, WallJumpY);
                            AudioM.PlaySound("Jump");
                            FlipBase();
                        }
                    }
                    else if (IsOnWallRight == true)
                    {
                        if (RBody.velocity.y <= 0)
                        {
                            RBody.velocity = new Vector2(-WallJumpX, WallJumpY);
                            AudioM.PlaySound("Jump"); 
                            FlipBase();
                        }
                        else
                        {
                            RBody.velocity = new Vector2(-WallJumpX, WallJumpY);
                            AudioM.PlaySound("Jump");
                            FlipBase();
                        }
                    }
                }
            }
         }
      }

        
    

    void CheckAllTriggers()
    {
        bool WasOnGround = isPlayerOnGround;
        CheckTrigger(GroundCheck, ref isPlayerOnGround, 0.38f);
        if ((WasOnGround == false) && (WasOnGround !=isPlayerOnGround) && (!JustJumped))
        {
            AudioM.PlaySound("JumpLand");
        }
        CheckWalls();
    }

    void CheckWalls()
    {
        Vector3 theScale = transform.localScale;
        if (theScale.x > 0)
        {
            CheckTrigger(LeftCheck, ref IsOnWallLeft, 0.2f);
            CheckTrigger(RightCheck, ref IsOnWallRight, 0.2f);
        }
        else
        {
            CheckTrigger(LeftCheck, ref IsOnWallRight, 0.2f);
            CheckTrigger(RightCheck, ref IsOnWallLeft, 0.2f);
        }
    }

    public void CheckTrigger(Transform Pos, ref bool Check, float radius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(Pos.position,radius, WhatIsWall);
        bool isObject = false;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                isObject = true;
        }
        Check = isObject;
    }

    
        public void ProcessAnimations()
        {
            if (isPlayerOnGround)
            {
                Flip();
            }
            if (RBody.velocity.x != 0)
            {
                animator.SetBool("IsMoving", true);
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }
        }
        

    
    //flip Character to face in the right direction
    private void Flip()
    {
        if ((isFacingLeft == true) & (WasFacingRight == true)) //((currentVelocity.x <= 0) & isFacingRight == false) //isKeyDownLeft == true
        {
            FlipBase();
        }
        else if ((isFacingRight == true) & (WasFacingRight == false))//((currentVelocity.x >= 0) & isFacingRight == true)
        {
            FlipBase();
        }
    }

    

    //basic flipping procedure
    private void FlipBase()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        WasFacingRight = !WasFacingRight;  
        //animator.transform.Rotate(0, 180, 0);
    }

}
