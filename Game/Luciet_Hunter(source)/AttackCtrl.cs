using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCtrl : MonoBehaviour
{
    /*
    プレイヤーの攻撃機能を担当するコードです。
    */

    public Camera cam;
    //public GameObject dummyGun;
    float range; int bulletCount, bulletAllCount;
    public int initMagazine, magazine, bulletLot;
    public LayerMask[] ignore;

    Ray ray;
    ParticleSystem muzzleFlash;
    PlayerCtrl playerCtrl;


    // Use this for initialization
    void Start()
    {
        playerCtrl = GetComponent<PlayerCtrl>();
        //muzzleFlash = GetComponentInChildren<ParticleSystem>();
        range = GameManager.instance.playerShootRange;
        magazine = initMagazine;
        bulletCount = bulletLot;
        bulletAllCount = bulletLot * initMagazine;

    }

    // Update is called once per frame
    void Update()
    {
        Fire();
        Reload();
        ShowBulletandLot();
    }

    void ShowBulletandLot()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log(bulletCount + " / " + bulletAllCount);
            Debug.Log(magazine + " / " + initMagazine);
        }
    }
    // デバッグ用で作った関数です。
    // 残った銃弾とマガジンを表示します。

    void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) || bulletCount == 0)
        {
            magazine--;
            bulletAllCount = bulletCount + (magazine * bulletLot);
            bulletCount = bulletLot;
            
        }

    }
    public void Fire()
    {
        int outMask =(1<<LayerMask.NameToLayer("Enemy"))| (1 << LayerMask.NameToLayer("Dummy")) | (1 << LayerMask.NameToLayer("UI"));
        
        RaycastHit rcHit;
        ray = cam.ScreenPointToRay(new Vector2(Screen.width / 2.0f, Screen.height / 2.0f));
        if (Input.GetMouseButtonDown(0))
        {
            bulletCount--;
            Debug.Log(bulletCount + " / " + bulletAllCount);
            //muzzleFlash.Play();
            if (Physics.Raycast(ray, out rcHit, range, ignore[0]|ignore[1]))
            {
                Debug.Log(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction * 10.0f, Color.cyan, 5.0f);
                
                if(rcHit.collider.gameObject.layer==LayerMask.NameToLayer("Enemy")&& rcHit.collider.gameObject.GetComponent<EnemyAI>())
                {
                    Debug.Log(rcHit.collider.gameObject.tag + " Hit Damage "+GameManager.instance.playerWeaponDamage);
                    rcHit.collider.gameObject.GetComponent<EnemyAI>().EnemyDamage();
                }
                else
                {
                    Debug.Log(rcHit.collider.gameObject.tag);
                    Debug.Log(rcHit.transform.name);
                }

                /*EnemyDamage enemyDamage = rcHit.collider.gameObject.GetComponent<EnemyDamage>();
                if (enemyDamage == null)
                {
                    //Debug.Log(rcHit.collider.gameObject.tag);
                }
                else
                    //enemyDamage.EnemyTakeDamage(playerCtrl.cannonDamage);*/


            }
        }
    }
}