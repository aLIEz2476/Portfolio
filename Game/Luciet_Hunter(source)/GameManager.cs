using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public PlayerCtrl playerCtrl; public SpwanManager spwanManager;
    public static GameManager instance = null; float cnt_Time;
    public enum GameDifficult { EASY, SOSO, HARD, FXCK }
    public float playerShootRange = 0, playerWeaponDamage = 0;
    public int playerScore = 0;
    public GameDifficult diff; public float enemyRange;
    public PatrolList patrolList;

    public bool isDead_Boss = false, stg0_end = false, stg1_end = false, stg2_end = false, stg3_end = false;
    public bool isCountDownEnd = false;
    [SerializeField]
    public int stage_num = 0;
    [HideInInspector]
    public int stg1_enemy = 0, stg2_enemy = 0;
    [HideInInspector]
    public int stg0_enemyCount = 10, stg1_enemyCount = 0, stg2_enemyCount = 0, boss_count = 1;
    float countTime = 5.0f, timeRemaing;

    private void Awake()
    {
        DifficultChecker();
        stg1_enemyCount = stg1_enemy;
        stg2_enemyCount = stg2_enemy;
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

    }

    // Use this for initialization
    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine("EnemySpwan");

    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (!playerCtrl.Player1.GetComponent<PlayerDamage>().isPlayerDie && !playerCtrl.Player2.GetComponent<PlayerDamage>().isPlayerDie)
            SwitchPlayer();
        else
            ActiveChecker();
        StageChanger();

        playerCtrl.Player1.GetComponent<PlayerDamage>().DeadChecker();
        playerCtrl.Player2.GetComponent<PlayerDamage>().DeadChecker();


    }

    void ActiveChecker()
    {
        if (!playerCtrl.Player1.GetComponent<PlayerDamage>().isPlayerDie && playerCtrl.Player2.GetComponent<PlayerDamage>().isPlayerDie)
        {
            playerCtrl.cam_Player.transform.SetParent(playerCtrl.Player1.transform, false);
            Debug.Log("P2->P1");
            playerCtrl.Player1.SetActive(true);
            playerCtrl.Player1.GetComponent<Rigidbody>().isKinematic = false;
            playerCtrl.sel_Player = playerCtrl.Player1;
            //playerCtrl.sel_Player.transform.LookAt(playerCtrl.p1_gun.transform);
            playerCtrl.Player2.GetComponent<Rigidbody>().isKinematic = true;
            playerCtrl.Player2.SetActive(false);
        }
        if (playerCtrl.Player1.GetComponent<PlayerDamage>().isPlayerDie && !playerCtrl.Player2.GetComponent<PlayerDamage>().isPlayerDie)
        {
            playerCtrl.cam_Player.transform.SetParent(playerCtrl.Player2.transform, false);
            Debug.Log("P1->P2");
            playerCtrl.Player2.SetActive(true);
            playerCtrl.Player2.GetComponent<Rigidbody>().isKinematic = false;
            playerCtrl.sel_Player = playerCtrl.Player2;
            //playerCtrl.sel_Player.transform.LookAt(playerCtrl.p2_gun.transform);
            playerCtrl.Player1.GetComponent<Rigidbody>().isKinematic = true;
            playerCtrl.Player1.SetActive(false);
        }
    }

    void StageChanger()
    {
        if (!isCountDownEnd)
        {
            if (stg0_enemyCount == 0 && stage_num == 0)
            {
                stg0_end = true;
                isCountDownEnd = true;
                countTime = 5;
                CountDown();

            }
            if (stg1_enemyCount == 0 && stage_num == 1)
            {
                stg1_end = true;
                isCountDownEnd = true;
                countTime = 5;
                CountDown();

            }
            if (stg2_enemyCount == 0 && stage_num == 2)
            {
                stg2_end = true;
                isCountDownEnd = true;
                countTime = 5;
                CountDown();

            }
            if (isDead_Boss)
            {
                stg3_end = true;
            }
        }

    }

    void CountDown()
    {
        while(true)
        {
            countTime -= Time.deltaTime;
            if (countTime >= 0.1f && isCountDownEnd)
            {
                Debug.Log(countTime);
            }
            else if (countTime < 0)
            {
                
                MagazineReset();
                isCountDownEnd = false;
                stage_num++;
                Debug.Log(stage_num + "Stage is Start!");
                break;
            }
        }
        
    }

    void MagazineReset()
    {
        if (!isCountDownEnd)
        {
            playerCtrl.Player1.GetComponent<AttackCtrl>().magazine = playerCtrl.Player1.GetComponent<AttackCtrl>().initMagazine;
            playerCtrl.Player2.GetComponent<AttackCtrl>().magazine = playerCtrl.Player2.GetComponent<AttackCtrl>().initMagazine;
        }
    }

    IEnumerator EnemySpwan()
    {
        while (true)
        {
            if (!isCountDownEnd && (stg0_end || stg1_end || stg2_end))
            {
                spwanManager.SpwanEnemy();
                //yield return new WaitForSecondsRealtime(2.0f);
            }
            yield return new WaitForSecondsRealtime(2.0f);
        }
    }

    void SwitchPlayer()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            playerCtrl.transform.rotation = Quaternion.Euler(Vector3.zero);
            if (playerCtrl.sel_Player.CompareTag("Player_P1"))
            {

                playerCtrl.cam_Player.transform.SetParent(playerCtrl.transform, false);
                Debug.Log("P1->P2");
                playerCtrl.Player2.SetActive(true);
                playerCtrl.Player2.transform.rotation = Quaternion.Euler(Vector3.zero);
                //playerCtrl.Player2.GetComponent<Rigidbody>().isKinematic = false;
                playerCtrl.sel_Player = playerCtrl.Player2;
                //playerCtrl.sel_Player.transform.LookAt(playerCtrl.p2_gun.transform);
                //playerCtrl.Player1.GetComponent<Rigidbody>().isKinematic = true;
                playerCtrl.Player1.SetActive(false);
            }
            else
            {
                playerCtrl.cam_Player.transform.SetParent(playerCtrl.transform, false);
                Debug.Log("P2->P1");
                playerCtrl.Player1.SetActive(true);
                playerCtrl.Player1.transform.rotation = Quaternion.Euler(Vector3.zero);
                //playerCtrl.Player1.GetComponent<Rigidbody>().isKinematic = false;
                playerCtrl.sel_Player = playerCtrl.Player1;
                //playerCtrl.sel_Player.transform.LookAt(playerCtrl.p1_gun.transform);
                //playerCtrl.Player2.GetComponent<Rigidbody>().isKinematic = true;
                playerCtrl.Player2.SetActive(false);
            }
        }

    }

    public void GameWin()
    {
        Debug.Log("Congratulation, You Win!");
        playerCtrl.enabled = false;
        spwanManager.enabled = false;
    }

    public void GameOver()
    {
        Debug.Log("You Lose");
        playerCtrl.enabled = false;
        spwanManager.enabled = false;
    }

    void DifficultChecker()
    {
        if (diff == GameDifficult.EASY)
        {
            stg1_enemy = 8;
            stg2_enemy = 5;
            enemyRange = 2;
            playerCtrl.Player1.GetComponent<PlayerDamage>().playerHp = 200;
            playerCtrl.Player2.GetComponent<PlayerDamage>().playerHp = 200;
            playerCtrl.Player1.GetComponent<AttackCtrl>().initMagazine = 6;
            playerCtrl.Player2.GetComponent<AttackCtrl>().initMagazine = 6;
            playerCtrl.Player1.GetComponent<AttackCtrl>().bulletLot = 30;
            playerCtrl.Player2.GetComponent<AttackCtrl>().bulletLot = 30;
        }
        if (diff == GameDifficult.SOSO)
        {
            stg1_enemy = 16;
            stg2_enemy = 10;
            enemyRange = 3.5f;
            playerCtrl.Player1.GetComponent<PlayerDamage>().playerHp = 150;
            playerCtrl.Player2.GetComponent<PlayerDamage>().playerHp = 150;
            playerCtrl.Player1.GetComponent<AttackCtrl>().initMagazine = 5;
            playerCtrl.Player2.GetComponent<AttackCtrl>().initMagazine = 5;
            playerCtrl.Player1.GetComponent<AttackCtrl>().bulletLot = 30;
            playerCtrl.Player2.GetComponent<AttackCtrl>().bulletLot = 30;
        }
        if (diff == GameDifficult.HARD)
        {
            stg1_enemy = 32;
            stg2_enemy = 27;
            enemyRange = 5;
            playerCtrl.Player1.GetComponent<PlayerDamage>().playerHp = 100;
            playerCtrl.Player2.GetComponent<PlayerDamage>().playerHp = 100;
            playerCtrl.Player1.GetComponent<AttackCtrl>().initMagazine = 4;
            playerCtrl.Player2.GetComponent<AttackCtrl>().initMagazine = 4;
            playerCtrl.Player1.GetComponent<AttackCtrl>().bulletLot = 20;
            playerCtrl.Player2.GetComponent<AttackCtrl>().bulletLot = 20;
        }
        if (diff == GameDifficult.FXCK)
        {
            stg1_enemy = 55;
            stg2_enemy = 45;
            enemyRange = 7;
            playerCtrl.Player1.GetComponent<PlayerDamage>().playerHp = 100;
            playerCtrl.Player2.GetComponent<PlayerDamage>().playerHp = 100;
            playerCtrl.Player1.GetComponent<AttackCtrl>().initMagazine = 3;
            playerCtrl.Player2.GetComponent<AttackCtrl>().initMagazine = 3;
            playerCtrl.Player1.GetComponent<AttackCtrl>().bulletLot = 20;
            playerCtrl.Player2.GetComponent<AttackCtrl>().bulletLot = 20;

        }
    }

}
