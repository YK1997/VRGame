using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static int m_Score = 0;
    // Start is called before the first frame update
    void Start()
    {
        ResetScore();
    }

    /// <summary>
    /// スコアリセット
    /// </summary>
    public static void ResetScore()
    {
        m_Score = 0;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="score"></param>
    public static void SetScore(int score)
    {
        m_Score += score;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static int GetScore()
    {
        return m_Score;
    }
}
