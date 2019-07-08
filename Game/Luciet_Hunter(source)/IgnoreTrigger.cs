using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreTrigger : MonoBehaviour {

    /*
    トリガを無効化するコードです。
     */

    Rigidbody rg;
	// Use this for initialization
	void Start () {
        rg = GetComponent<Rigidbody>();
	}
    // Rigidbodyをオブジェクトから持ってきます。
	
	// Update is called once per frame
	void Update () {
		
        //if(rg.isKinematic)
        //{
        //    rg.isKinematic = false;
        //}

	}

    private void FixedUpdate()
    {
        if (rg.isKinematic)
        {
            rg.isKinematic = false;
        }   
    }

    // モンスターや障害物とぶつかった場合トリガーを操作します。
   
    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.layer==LayerMask.NameToLayer("Enemy") || coll.gameObject.layer==LayerMask.NameToLayer("Dummy"))
        {
            Debug.Log("Kinematic On!");
            rg.isKinematic = true;
        }
        
    }
    

    private void OnCollisionStay(Collision coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Enemy") || coll.gameObject.layer == LayerMask.NameToLayer("Dummy"))
        {
            Debug.Log("Kinematic On!");
            rg.isKinematic = true;
        }
    }

    private void OnCollisionExit(Collision coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Enemy") || coll.gameObject.layer == LayerMask.NameToLayer("Dummy"))
        {
            rg.isKinematic = false;
        }
    }
}
