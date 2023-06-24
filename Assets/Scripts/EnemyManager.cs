using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class EnemyManager : MonoBehaviour
{
    [CanBeNull] public static List<GameObject> m_Enemies = new List<GameObject>();

    private Vector3 m_OssanSpawn_Point = new Vector3(); 
    private Vector3 m_OssanSpawn_Rotate = new Vector3();
    private List<Hashtable> m_EnemyMasterDats = new List<Hashtable>()
    {
        new Hashtable(){{"name","ojisan"},{"max_num",1},{"spawn_time",5.0f},},
        new Hashtable(){{"name","g"},{"max_num",3},{"spawn_time",3.0f},},
    };
    
    /// <summary>
    /// 
    /// </summary>
    private Dictionary<string,float> m_EnemiesPopTimer = new Dictionary<string, float>()
    {
        {"ojisan",0},
        {"g",0},
    };
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        //敵の出現タイマを更新
        //TODO:毎アップデートLINQかますと重いかも知れないので、DeltaTime使うか検討
        UpdateEnemyRespawnTimer();
    }

    /// <summary>
    /// 敵出現タイマ更新
    /// </summary>
    public void UpdateEnemyRespawnTimer()
    {
        m_EnemiesPopTimer = m_EnemiesPopTimer
            .Select((row) => (new {row.Key, Value=(float)row.Value + Time.deltaTime}))
            .ToDictionary(row=>row.Key,row=>row.Value);
    }
    
    
    /// <summary>
    /// 敵のリスポーン時間をチェック
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool CheckEnemyRespawnTime(string name)
    {
        bool ret = false;
        Hashtable enemy_list_row = m_EnemyMasterDats.Find(v => v["name"] == name);
        Debug.Log("m_EnemiesPopTimer:"+m_EnemiesPopTimer[name]+" spawn_time:"+(float)enemy_list_row["spawn_time"]);
        if (m_EnemiesPopTimer[name] >= (float)enemy_list_row["spawn_time"])
        {
            ret = true;
            //時間をリセット
            m_EnemiesPopTimer[name] = 0;
        }
        return ret;
    }
    
    /// <summary>
    /// 現在出現している敵の個数を確認
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool CheckEnemyRespawnNum(string name)
    {
        bool ret = false;
        //削除されたオブジェクトを除く
        //TODO:処理はコルーチンの頭に入れるべき
        m_Enemies = m_Enemies.Where(v => v != null).ToList();
        //出現している敵の数
        int spawned_enemy_count = m_Enemies.Where(enemy =>(enemy.name == name)).Count();
        Hashtable enemy_list_row = m_EnemyMasterDats.Find(v => v["name"] == name);
        if (spawned_enemy_count <= (int)enemy_list_row["max_num"])
        {
            ret = true;
        }
        return ret;
    }
    

    /// <summary>
    /// 敵出現処理
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            if (GameManager.m_Phase == GameManager.Phase.Ingame)
            {
                foreach (string enemy_name in m_EnemyMasterDats.Select(v=>v["name"]).ToList())
                {                    
                    //出現する敵ごとにリスポーン時間をチェック
                    if (!CheckEnemyRespawnTime(enemy_name))
                    {
                        //出現時間前
                        continue;
                    }
                    //敵が上限まで出ているかチェック
                    if (!CheckEnemyRespawnNum(enemy_name))
                    {
                        continue;
                    }
                    
                    //敵出現
                    var enemy = GameObject.Instantiate(Resources.Load<GameObject>(enemy_name));
                    
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
