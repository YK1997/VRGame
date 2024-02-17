using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goki : MonoBehaviour
{
    private Trash m_Trash;
    private EnemyManager m_EnemyManager;
    // Start is called before the first frame update
    void Awake()
    {
        m_Trash = GameObject.Find("/Class").GetComponent<Trash>();
        m_EnemyManager = GameObject.Find("/GameManagerObject").GetComponent<EnemyManager>();
        gameObject.GetComponent<Animator>().SetBool("g_move",true);
        gameObject.transform.position = m_Trash.GetSpawnPoint();
    }

    private void OnDestroy()
    {
        m_EnemyManager.RemoveFromEnemyList(gameObject);
    }
}
