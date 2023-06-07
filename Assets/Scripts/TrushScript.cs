using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrushScript : MonoBehaviour
{
    enum layer
    {
        Default = 0,
        Water = 4,
        UI = 5,
        Floor = 8,
        MovableObject = 9,
    }

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
        if ((layer)other.gameObject.layer == layer.MovableObject)
        {
            Destroy(other.gameObject);
            ScoreManager.SetScore(10);
        }
    }
}
