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
    private string m_TrashPath = "Trashes/Cube";
//    [SerializeField] private string m_TrashPath = "Trashes/Cube";
    private List<GameObject> TrashList = new List<GameObject>();
    void Start()
    {
        NpcSpawner();
    }

    void NpcSpawner()
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
    Vector3 GetSpawnPoint()
    {
        Vector3 point = new Vector3();
        if (TrashList.Count >= m_SpawnArea.transform.childCount)
        {
            return point;
        }
        do
        {
            point = m_SpawnArea.transform.GetChild(
                Random.RandomRange(0, (m_SpawnArea.transform.childCount))).position;
            //TODO:現状positionとpointを完全一致で取得しているが、物理演算が入って座標がずれる可能性があるためおおよそのいちでチェックする
        } while ((TrashList.Where(Trash => (Trash.transform.position == point)).Count() > 0) );

        return point;
    }

    void Spawn(Vector3 spawn_point)
    {
        if (spawn_point!=null)
        {
//            Resources.FindObjectsOfTypeAll(m_TrashPath);
            var go = GameObject.Instantiate((GameObject) Resources.Load(m_TrashPath));
            go.transform.position = spawn_point;
            TrashList.Add(go);
        }
    }
}
