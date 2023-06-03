using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    //リセット(オブジェクトの初期化)
    //拾ったものをスコアとして適用
    

    /// <summary>
    /// 捨てたものを特典として加算
    /// </summary>
    /// <param name="obj"></param>
    public static void SetScore(GameObject obj)
    {
        //ゲームオブジェクトから捨てるものを逆引きする
        
        //スコアを加算(現在固定)
        ScoreManager.SetScore(1);
    }
}
