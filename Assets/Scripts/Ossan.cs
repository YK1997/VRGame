using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Ossan : MonoBehaviour
{
    private Trash m_Trash;
    private const float TRASH_INTERVAL = 3.0f;
    // Start is called before the first frame update
    void Awake()
    {
        m_Trash = GameObject.Find("/Trash").GetComponent<Trash>();
        StartCoroutine(SpawnTrash());
    }

    IEnumerator SpawnTrash()
    {
        while (true)
        {
            yield return new WaitForSeconds(TRASH_INTERVAL);
            gameObject.GetComponent<Animator>().SetBool("oji_nage",true);
            m_Trash.Spawn(m_Trash.GetSpawnPoint());
        }
        yield return null;
    }
    
    public void OnClick(Collision other){
        //おっさんが驚いて落ちる
        
        //爆発エフェクト
        Destroy(this.gameObject);
    }
}
