using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager _self;
    public static Phase m_Phase { get; private set; }

    public enum Phase
    {
        None = 0,
        Title,
        Ingame,
        Result,
    }

    /// <summary>
    /// 各シーンとフェーズの対応リスト
    /// </summary>
    public static Dictionary<Phase,string> m_ScenePhaseLists = new Dictionary<Phase,string>()
    {
        {Phase.Title,"Scenes/Title"},
        {Phase.Ingame,"Scenes/Ingame"},
        {Phase.Result,"Scenes/Ingame"},
    };
    
    
    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        _self = this;
    }
    
    /// <summary>
    /// フェーズ設定
    /// </summary>
    /// <param name="phase"></param>
    public static void UpdatePhase( Phase phase)
    {
        m_Phase = phase;
    }
    
    /// <summary>
    /// フェーズの更新
    /// </summary>
    /// <param name="phase"></param>
    public static void SwitchPhase(Phase phase)
    {
        if (phase != m_Phase)
        {
            UpdatePhase(phase);
            SceneManager.LoadScene(m_ScenePhaseLists[phase]);            
        }
    }

    /// <summary>
    /// フェーズ切替時に発生するイベントを発呼する
    /// </summary>
    private static void SwitchPhaseEvent(Phase phase)
    {
        switch (phase)
        {
            case Phase.Title:
                break;
            case Phase.Ingame:
                //部屋を散らかす
                _self.AddComponent<Trash>();
                break;
            case Phase.Result:
                //ゴミクラスを削除
                Destroy(_self.GetComponent<Trash>());
                //現状、プレイヤーがわの処理が作られていないので出せない
//                PopupManager.MakePopup(
//                    "Resources/ResultPopup"
//                );
                break;
        }
    }
}