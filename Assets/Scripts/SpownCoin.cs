using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpownCoin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spown()
    {
        GameObject coin = (GameObject)GameObject.Instantiate(Resources.Load("Coin"));
        coin.transform.Translate(0,0.25f,0);
    }
}
