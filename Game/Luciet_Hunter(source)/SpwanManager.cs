using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpwanManager : MonoBehaviour {
    /*
    モンスターを召喚するコードです。
     */

    public GameObject Zombie, Mortimer, Boss;

    public Transform[] SpwanTransform = new Transform[10];

    [HideInInspector]
    int ranLength1;
    int ranLength2;
    int[] ranArr1; int[] ranArr2;
    public int count1=0, count2=0, count3=0;
    // ランダムな位置で召喚するために変数を作ります。

    // Use this for initialization
    void Start () {

        ranLength1 = GameManager.instance.stg1_enemy;
        ranLength2 = GameManager.instance.stg2_enemy;
        RandomMaker1();
        RandomMaker2();
        RandomChecker();
    }
	
	// Update is called once per frame
	void Update () {
        RandomMaker1();
        RandomMaker2();
        RandomChecker();
    }

    void RandomChecker()
    {
        if(ranArr1.Length>10)
        {
            for(int i=0;i<ranArr1.Length;i++)
            {
                if(ranArr1[i]>9)
                {
                    ranArr1[i] -= 10;
                }
            }
        }
        if(ranArr2.Length>10)
        {
            for (int i = 0; i < ranArr2.Length; i++)
            {
                if (ranArr2[i] > 9)
                {
                    ranArr2[i] -= 10;
                }
            }
        }
    }
    // モンスターの数を数えます。

    public void SpwanEnemy()
    {
        if (GameManager.instance.stage_num == 1 && GameManager.instance.stg1_enemy > 0)
        {
            Instantiate(Zombie, SpwanTransform[ranArr1[count1]].position, Quaternion.identity);
            GameManager.instance.stg1_enemy--;
        }
        else if (count1 == GameManager.instance.stg1_enemy)
        {
            //GameManager.instance.isCountDownEnd = false;
        }

        if (GameManager.instance.stage_num == 2 && GameManager.instance.stg2_enemy > 0)
        {
            Instantiate(Mortimer, SpwanTransform[ranArr2[count2]].position, Quaternion.identity);
            GameManager.instance.stg2_enemy--;
        }
        else if (count2 == GameManager.instance.stg2_enemy)
        {
            //GameManager.instance.isCountDownEnd = false;
        }
        if (GameManager.instance.stage_num == 3 && count3 < GameManager.instance.boss_count)
        {
            Instantiate(Boss, SpwanTransform[ranArr1[count3]].position, Quaternion.identity);
            count3++;
        }
    }
    // ステージに合わせてモンスターを召喚します。

    int[] RandomMaker1()
    {
        ranArr1 = Enumerable.Range(0, ranLength1).ToArray();

        for (int i = 0; i < ranLength1; ++i)
        {
            int ranIdx = Random.Range(i, ranLength1);
            int tmp = ranArr1[ranIdx];

            ranArr1[ranIdx] = ranArr1[i];
            ranArr1[i] = tmp;
        }

        return ranArr1;
    }

    int[] RandomMaker2()
    {
        ranArr2 = Enumerable.Range(0, ranLength2).ToArray();

        for (int i = 0; i < ranLength2; ++i)
        {
            int ranIdx = Random.Range(i, ranLength2);
            int tmp = ranArr2[ranIdx];

            ranArr2[ranIdx] = ranArr2[i];
            ranArr2[i] = tmp;
        }

        return ranArr2;
    }
    // ランダムポジションを作ります。
    
}
