using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [CanBeNull] public List<GameObject> m_Enemies = new List<GameObject>();

    private Vector3 m_OssanSpawn_Point = new Vector3(); 
    private Vector3 m_OssanSpawn_Rotate = new Vector3(); 
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemy()
    {
        if (GameManager.m_Phase == GameManager.Phase.Ingame)
        {
            var ossan_trans = GameObject.Find("OssanSpawnPoint").transform;
            m_OssanSpawn_Point = ossan_trans.position;
            m_OssanSpawn_Rotate = ossan_trans.rotation.eulerAngles;

            //時間ごとに最大数まで出現
            //ランダムで出現する敵を決める
            //出現
            yield return new WaitForSeconds(5.0f);
            if (m_Enemies.Count <= 0)
            {
                var enemy = GameObject.Instantiate(Resources.Load<GameObject>("Ojisan"));
                enemy.transform.position = m_OssanSpawn_Point;
                enemy.transform.eulerAngles = m_OssanSpawn_Rotate;
                m_Enemies.Add(enemy);                
            }
        }
    }

    public void DestroyEnemyList()
    {
//        m_Enemies.Find()
    }
}
