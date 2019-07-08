using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{


    float rotationY = 0F; float boost_Speed, org_Speed;

    int[] arrow; //Rigidbody rg;
    bool isJump = true, isBoost = false;
    public bool isAllDead = false;
    public float moveSpeed = 5.0f;
    public float xSensitivity = 1.0f;
    public float ySensitivity = 1.0f;
    Vector3 xLimit, yLimit, v3;

    [HideInInspector]
    public GameObject sel_Player;
    Animator animator;
    Rigidbody rg;
    public GameObject Player1, Player2;
    public Camera cam_Player;
    //public GameObject p1_gun, p2_gun;
    Transform org_cam;

    int Player_hashMove_F = Animator.StringToHash("PlayerMove_F");
    int Player_hashMove_B = Animator.StringToHash("PlayerMove_B");
    int Player_hashMove_L = Animator.StringToHash("PlayerMove_L");
    int Player_hashMove_R = Animator.StringToHash("PlayerMove_R");
    int Player_hashJump = Animator.StringToHash("PlayerJump");
    int Player_hashJumpOff = Animator.StringToHash("PlayerJumpOff");


    private void Awake()
    {
        sel_Player = Player1;
        arrow = new int[] { Player_hashMove_F, Player_hashMove_B, Player_hashMove_L, Player_hashMove_R };
        cam_Player.transform.SetParent(Player1.transform, false);
        //Player2.GetComponent<Rigidbody>().isKinematic = true;
        Player2.SetActive(false);
    }

    // Use this for initialization
    void Start()
    {
        org_cam = cam_Player.transform;
        org_Speed = moveSpeed;
        boost_Speed = moveSpeed * 1.5f;
        rg = GetComponent<Rigidbody>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        animator = sel_Player.GetComponent<Animator>();
        isJump = false;
        if(!isAllDead||GameManager.instance.isCountDownEnd||GameManager.instance.isDead_Boss)
        {
            KeyControl();
            MouseMove();
        }
        //if(Input.GetKeyDown(KeyCode.A))
        //{
        //    transform.Rotate(Vector3.up*10);
        //}
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    transform.Rotate(Vector3.down * 10);
        //}
        //if(Input.GetKeyDown(KeyCode.W))
        //{
        //    transform.Translate(Vector3.forward * Time.deltaTime*moveSpeed);
        //}
        


    }


    void PlayerDeathCheck()
    {
        if (Player1.GetComponent<PlayerDamage>().isPlayerDie && Player2.GetComponent<PlayerDamage>().isPlayerDie)
        {
            isAllDead = true;
        }
            
    }

    void MouseMove()
    {

        float yRot = Input.GetAxis("Mouse X") * xSensitivity;
        float xRot = Input.GetAxis("Mouse Y") * ySensitivity;

        if (xRot < -0.5f)
        {
            xRot = -0.5f;
        }
        if (xRot > 1.0f)
        {
            xRot = 1.0f;
        }
        transform.Rotate(0, yRot, 0);
        cam_Player.transform.Rotate(-xRot, 0, 0);
        

        if (cam_Player.transform.rotation.x <= -0.5f)
        {
            v3 = new Vector3(-45, 0, 0);
            cam_Player.transform.Rotate(v3);
        }
        if (transform.rotation.x >= 1.0f)
        {
            v3 = new Vector3(90, 0, 0);
            cam_Player.transform.Rotate(v3);
        }
    }

    void KeyControl()
    {


        // ESC를 누르면 커서 등판

        //회전하고 싶은 축과 입력축이 반대인 것에 유의


        if (Input.GetKey(KeyCode.W))
        {
            //rg.MovePosition(Vector3.forward * moveSpeed * Time.deltaTime);
            //sel_Player.transform.Translate(Vector3.forward* moveSpeed * Time.deltaTime);
            //transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            animator.SetBool(Player_hashMove_F, true);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            animator.SetBool(Player_hashMove_F, false);
        }
        if (Input.GetKey(KeyCode.A))
        {
            //sel_Player.transform.Translate(Vector3.left* moveSpeed * Time.deltaTime);
            //transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

            animator.SetBool(Player_hashMove_L, true);
            transform.Translate(Vector3.left * org_Speed * Time.deltaTime);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            animator.SetBool(Player_hashMove_L, false);
        }
        if (Input.GetKey(KeyCode.S))
        {
            //sel_Player.transform.Translate(-Vector3.forward* moveSpeed * Time.deltaTime);
            //transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);
            animator.SetBool(Player_hashMove_B, true);
            transform.Translate(-Vector3.forward * org_Speed * Time.deltaTime);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            animator.SetBool(Player_hashMove_B, false);
        }
        if (Input.GetKey(KeyCode.D))
        {
            //sel_Player.transform.Translate(-Vector3.left* moveSpeed * Time.deltaTime);
            //transform.Translate(-Vector3.left * moveSpeed * Time.deltaTime);
            animator.SetBool(Player_hashMove_R, true);
            transform.Translate(-Vector3.left * org_Speed * Time.deltaTime);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            animator.SetBool(Player_hashMove_R, false);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            if (isJump)
            {
                rg.AddForce(Vector3.up * 5, ForceMode.Impulse);
                animator.SetTrigger(Player_hashJump);
                isJump = false;
            }

            else
            {
                return;
            }
            



        }
        if (!Input.anyKey)
        {
            for (int i = 0; i < arrow.Length; i++)
            {
                animator.SetBool(arrow[i], false);
            }
        }
        if (Input.GetKey(KeyCode.LeftShift) && !(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
        {
            moveSpeed = boost_Speed;
            sel_Player.GetComponent<Animator>().SetTrigger("PlayerRun");
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = org_Speed;
        }

        //if (!Input.anyKey)
        //{
        //    for (int i = 0; i < arrow.Length; i++)
        //    {

        //    }
        //}
        // 이동 스크립트
    }
}
