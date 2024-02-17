using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Ossan : MonoBehaviour
{
    private Trash m_Trash;
    private EnemyManager m_EnemyManager;
    private const float TRASH_INTERVAL = 2.0f;
    // Start is called before the first frame update
    void Awake()
    {
        m_Trash = GameObject.Find("/Class").GetComponent<Trash>();
        m_EnemyManager = GameObject.Find("/GameManagerObject").GetComponent<EnemyManager>();
        StartCoroutine(SpawnTrash());
        //おっさんの出現位置を変更
        var ossan_trans = GameObject.Find("OssanSpawnPoint").transform;
        gameObject.transform.position = ossan_trans.position;
        gameObject.transform.eulerAngles = ossan_trans.rotation.eulerAngles;
    }

    IEnumerator SpawnTrash()
    {
        while (true)
        {            
            if (m_Trash != null)
            {
                gameObject.GetComponent<Animator>().SetBool("oji_nage",true);
                m_Trash.Spawn(m_Trash.GetSpawnPoint());
                yield return new WaitForSeconds(TRASH_INTERVAL);
                gameObject.GetComponent<Animator>().SetBool("oji_nage",false);
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
    
    public void OnClick(Collision other){
        //おっさんが驚いて落ちる
        //リストからおっさん削除
        m_EnemyManager.RemoveFromEnemyList(gameObject);
        //爆発エフェクト
        //おっさん削除
        Destroy(this.gameObject);
    }
}
