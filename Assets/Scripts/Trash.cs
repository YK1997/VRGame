using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Trash : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject m_SpawnArea;
    private List<string> m_TrashPath = new List<string>()
    {
        "Trashes/petbbotle_001",
        "Trashes/petbbotle_002",
        "Trashes/petbbotle_003",
        "Trashes/petbbotle_004",
        "Trashes/petbbotle_005"
    };
//    [SerializeField] private string m_TrashPath = "Trashes/Cube";
    private List<GameObject> TrashList = new List<GameObject>();

    /// <summary>
    /// 最初にゴミを部屋にばらまく処理
    /// </summary>
    public void InitTrashes()
    {
        for (int i=0; i<=m_SpawnArea.transform.childCount;i++)
        {
            Spawn(GetSpawnPoint());
        }
    }
    
    /// <summary>
    /// ランダム位置で出現させる
    /// </summary>
    /// <returns></returns>
    public Vector3 GetSpawnPoint()
    {
        Vector3 point = Vector3.zero;
//        if (TrashList.Count >= m_SpawnArea.transform.childCount)
//        {
//            return point;
//        }
//        do
//        {
//            point = m_SpawnArea.transform.GetChild(
//                Random.RandomRange(0, (m_SpawnArea.transform.childCount))).position;
//            //TODO:現状positionとpointを完全一致で取得しているが、物理演算が入って座標がずれる可能性があるためおおよそのいちでチェックする
//        } while ((TrashList.Where(Trash => (Trash.transform.position == point)).Count() > 0) );

        point = m_SpawnArea.transform.GetChild(
            Random.RandomRange(0, (m_SpawnArea.transform.childCount))).position;
        return point;
    }

    public void Spawn(Vector3 spawn_point)
    {
        if (spawn_point != Vector3.zero)
        {
            var go = GameObject.Instantiate((GameObject) Resources.Load(m_TrashPath[Random.Range(0,m_TrashPath.Count)]));
            go.transform.position = spawn_point;
            TrashList.Add(go);
        }
    }
}
