using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrushScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("TrushScript");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);
        Destroy(other.gameObject);
        ScoreManager.SetScore(10);
    }
}
