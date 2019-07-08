using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour {
    /*
    ここのコードはプレイヤの体力の管理を担当します。
     */
    
    public float playerHp;
    float initHp; public bool isPlayerDie = false;

	// Use this for initialization
	void Start () {
        initHp = playerHp;
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void Damage(float enemyDamage)
    {
        playerHp -= enemyDamage;
        Debug.Log("Damaged! " + playerHp);
    }
    // ダメージを受けたとき、体力を減少します。
    public void DeadChecker()
    {
        if (playerHp <= 0)
        {
            isPlayerDie = true;
            Debug.Log(gameObject.tag + " is Die!");
            GameManager.instance.GameOver();
        }
            
    }
    // プレイヤの体力によって死亡をチェックするメソッドです。
    public void Heal()
    {
        playerHp = initHp;
    }
    
}
