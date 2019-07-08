using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public enum Type { TARGET, ZOMBIE, MORTIMER, BOSS }
    public enum State { IDLE, TRACE, ATTACK, DIE }
    PatrolList patrolList;
    public Type type;
    public State state;

    public float enemyHp, enemySpeed, enemyDamage, initHp;
    float enemyRange;
    float fdist, enemyOrgSpeed, enemyTraceSpeed;
    float traceSpeed; int patrolCount;
    Animator animator;
    NavMeshAgent naviAgent;
    int[] patrolArr; int patrolArrLength;

    readonly int Enemy_hashMove = Animator.StringToHash("isMove");
    readonly int Enemy_hashDetect = Animator.StringToHash("isDetect");
    readonly int Enemy_hashDead = Animator.StringToHash("isDead");
    readonly int Enemy_hashAttack = Animator.StringToHash("isAttack");
    readonly int Enemy_hashCombo = Animator.StringToHash("isCombo");

    // Use this for initialization
    void Awake()
    {
        enemyTraceSpeed = enemySpeed * 2;
        enemyOrgSpeed = enemySpeed;
        patrolCount = 0;
        state = State.IDLE;

    }

    void Start()
    {
        enemyRange = GameManager.instance.enemyRange;
        EnemyTypeSelecter();
        patrolArrLength = 10;
        patrolList = GameManager.instance.patrolList;
        PatrolRandomMaker();
        animator = GetComponent<Animator>();
        naviAgent = GetComponent<NavMeshAgent>();
        initHp = enemyHp;

        if (type != Type.TARGET)
            StartCoroutine("StateChanger");

    }

    // Update is called once per frame
    void Update()
    {

        fdist = Vector3.Distance(this.transform.position, GameManager.instance.playerCtrl.transform.position);
        DeadCheck();
        if(!gameObject.CompareTag("Enemy_Target"))
            naviAgent.speed = enemySpeed;
    }

    void DeadCheck()
    {
        if (enemyHp <= 0 && type != Type.BOSS)
        {
            state = State.DIE;
            if (type == Type.TARGET)
            {
                GameManager.instance.playerScore++;
                GameManager.instance.stg0_enemyCount--;
            }

            if (type == Type.ZOMBIE)
            {
                GameManager.instance.playerScore += 10;
                GameManager.instance.stg1_enemyCount--;
            }

            if (type == Type.MORTIMER)
            {
                GameManager.instance.playerScore += 20;
                GameManager.instance.stg2_enemyCount--;
            }
            gameObject.SetActive(false);
        }
        if (enemyHp <= 0 && type == Type.BOSS)
        {
            state = State.DIE;
            GameManager.instance.playerScore += 100;
            GameManager.instance.boss_count = 0;
            GameManager.instance.isDead_Boss = true;
            gameObject.SetActive(false);
            GameManager.instance.GameWin();
        }
    }

    void BossComboChecker(float dist)
    {
        if (state == State.ATTACK && dist < enemyRange / 4)
        {
            animator.SetBool(Enemy_hashCombo, true);
        }
        else
        {
            animator.SetBool(Enemy_hashCombo, false);
        }
    }

    void EnemyTypeSelecter()
    {
        if (gameObject.CompareTag("Enemy_Target"))
        {
            type = Type.TARGET;
            enemyHp = 1;
            enemySpeed = 0;
            enemyDamage = 0;
        }
        if (gameObject.CompareTag("Enemy_Zombie"))
        {
            type = Type.ZOMBIE;
            enemyHp = 50;
            enemySpeed = 0.5f;
            enemyDamage = 5;
        }
        if (gameObject.CompareTag("Enemy_Mortimer"))
        {
            type = Type.MORTIMER;
            enemyHp = 80;
            enemySpeed = 1.2f;
            enemyDamage = 10;
        }
        if (gameObject.CompareTag("Enemy_Boss"))
        {
            type = Type.BOSS;
            enemyHp = 1000;
            enemySpeed = 3;
            enemyDamage = 30;
        }
        if (GameManager.instance.diff == GameManager.GameDifficult.HARD || GameManager.instance.diff == GameManager.GameDifficult.FXCK)
        {
            enemyHp *= 2;
            enemySpeed *= 1.2f;
            enemyDamage *= 1.5f;
        }
    }

    void EnemyAttack(GameObject Target)
    {
        naviAgent.SetDestination(Target.transform.position);
        animator.SetTrigger(Enemy_hashAttack);
        Target.GetComponent<PlayerDamage>().Damage(enemyDamage);
    }

    void EnemyTracer(Vector3 PlayerPosition)
    {
        animator.SetBool(Enemy_hashMove, false);
        naviAgent.SetDestination(GameManager.instance.playerCtrl.transform.position);
        enemySpeed = enemyTraceSpeed;
        animator.SetBool(Enemy_hashDetect, true);
    }

    void EnemyPatroler()
    {

        if (patrolCount == 10)
        {
            patrolCount = 0;
            PatrolRandomMaker();
        }
        naviAgent.SetDestination(patrolList.patrol_Transform[patrolCount].position);
        animator.SetBool(Enemy_hashMove, true);
        patrolCount++;

    }

    int[] PatrolRandomMaker()
    {
        patrolArr = Enumerable.Range(0, patrolArrLength).ToArray();

        for (int i = 0; i < patrolArrLength; ++i)
        {
            int ranIdx = Random.Range(i, patrolArrLength);
            int tmp = patrolArr[ranIdx];

            patrolArr[ranIdx] = patrolArr[i];
            patrolArr[i] = tmp;
        }

        return patrolArr;
    }

    public void EnemyDamage()
    {
        enemyHp -= GameManager.instance.playerWeaponDamage;
    }

    IEnumerator StateChanger()
    {
        while (true)
        {
            Collider[] colls = Physics.OverlapSphere(transform.position, enemyRange);
            for (int i = 0; i < colls.Length; i++)
            {
                if (colls[i].gameObject.CompareTag("Player_P1") || colls[i].gameObject.CompareTag("Player_P2"))
                {
                    state = State.TRACE;
                    EnemyTracer(colls[i].gameObject.transform.position);
                    if (Vector3.Distance(colls[i].gameObject.transform.position, gameObject.transform.position) < enemyRange/2)
                    {
                        state = State.ATTACK;
                        EnemyAttack(colls[i].gameObject);
                        if (gameObject.CompareTag("Enemy_Boss"))
                            BossComboChecker(fdist);
                    }
                }
                else
                {
                    state = State.IDLE;
                    EnemyPatroler();
                }
            }
            yield return null;

        }

    }
}
