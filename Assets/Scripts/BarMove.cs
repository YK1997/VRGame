using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarMove : MonoBehaviour
{
    private int m_Mark = 1;

    private const double MOVEPOS_Z = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(this.gameObject.transform.position.z);
        if (this.gameObject.transform.position.z >= MOVEPOS_Z)
        {
            m_Mark = -1;
        }

        if (this.gameObject.transform.position.z <= -MOVEPOS_Z)
        {
            m_Mark = 1;
        }
        this.gameObject.transform.Translate(0,0,0.0009f * m_Mark);
    }
}
