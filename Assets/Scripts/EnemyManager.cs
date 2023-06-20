using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [CanBeNull] public static List<GameObject> m_Enemies = new List<GameObject>();

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
        while (true)
        {
            yield return new WaitForSeconds(10.0f);
            if (GameManager.m_Phase == GameManager.Phase.Ingame)
            {
                var ossan_trans = GameObject.Find("OssanSpawnPoint").transform;
                m_OssanSpawn_Point = ossan_trans.position;
                m_OssanSpawn_Rotate = ossan_trans.rotation.eulerAngles;

                //時間ごとに最大数まで出現
                //ランダムで出現する敵を決める
                //出現
                if (m_Enemies.Count <= 0)
                {
                    var enemy = GameObject.Instantiate(Resources.Load<GameObject>("Ojisan"));
                    enemy.transform.position = m_OssanSpawn_Point;
                    enemy.transform.eulerAngles = m_OssanSpawn_Rotate;
                    m_Enemies.Add(enemy);                
                }
            }
        }
    }

    public static void ResetEnemyList()
    {
        m_Enemies = new List<GameObject>();
    }
    
    public void RemoveFromEnemyList(GameObject enemy_object)
    {
        m_Enemies.RemoveAll(enemy =>(enemy.GetInstanceID() == enemy_object.GetInstanceID()));
//        m_Enemies = m_Enemies.Where(enemy =>(enemy.GetInstanceID() != enemy_object.GetInstanceID())).ToList();
    }
}
