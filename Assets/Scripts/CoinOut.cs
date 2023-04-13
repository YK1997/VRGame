using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class CoinOut : MonoBehaviour
{
    public Text m_TextObject;

    private int m_Score = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);
        Destroy(other.gameObject);
        m_Score += 100;
        m_TextObject.text = m_Score.ToString();
    }
}
