using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleLimit : MonoBehaviour
{
    // カメラのアングルを固定するコードです。    

    public Vector3 m_vDir;
    public Vector3 m_vLowDir;
    public Vector3 m_vHighDir;
    public float m_fAngleSpeed = 20.0f;
    public float m_fLimitAngle = 90;
    public float m_fCurAngle;
    
    public Camera cam;
    // Use this for initialization
    void Start()
    {
        

        m_fCurAngle = transform.rotation.x;
    }


    // Update is called once per frame
    void Update()
    {

        if (m_fCurAngle < -m_fLimitAngle)
        {
            m_fCurAngle = -m_fLimitAngle + 1;
        }
        if (m_fCurAngle > m_fLimitAngle)
        {
            m_fCurAngle = m_fLimitAngle - 1;
        }
        AccumulateLimit();
        


    }
    // 最低アングルと最大アングルに合わせてアングルを調整します。
    
    void AccumulateLimit()
    {
        float fAngle = Input.GetAxis("Mouse Y") * Time.deltaTime * m_fAngleSpeed;

        if (m_fCurAngle < m_fLimitAngle && m_fCurAngle > -m_fLimitAngle)
        {
            m_fCurAngle += fAngle;

        }
        transform.localRotation = Quaternion.Euler(new Vector3(-m_fCurAngle, 0, 0));


    }



}
