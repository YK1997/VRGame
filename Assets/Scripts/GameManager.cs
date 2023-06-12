using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager _self;
    public static Phase m_Phase { get; private set; }
    public GameObject m_CameraRig;
    public GameObject m_Timer;

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
        {Phase.None,"Scenes/Initialize"},
        {Phase.Title,"Scenes/Title"},
        {Phase.Ingame,"Scenes/Ingame"},
        {Phase.Result,"Scenes/Ingame"},
    };
    
    
    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {
        _self = this;
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(this.m_CameraRig);
        SetOnSceneLoaded();
        SwitchPhase(Phase.Title);
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
    /// フェーズ切替時に発生するイベントを発呼する
    /// </summary>
    public static void SwitchPhase(Phase phase)
    {
        if (phase == m_Phase)
        {
            return;
        }
        UpdatePhase(phase);
        SceneManager.LoadScene(m_ScenePhaseLists[phase]);
    }

    private void Update()
    {
        if (m_Phase == Phase.Ingame && m_Timer != null)
        {
            try
            {
//                GameObject.Find("/Timer")
                //Updateの中でやるべきではないので後ほど修正
                float total_time = m_Timer.GetComponent<CountDownTimer>().GetTotalTime();
                if (total_time<=0)
                {
                    SwitchPhase(Phase.Result);
                }
            }
            catch (Exception e)
            {
                //無視
            }
        }
    }

    private void SetOnSceneLoaded()
    {
        SceneManager.sceneLoaded += (Scene nextScene, LoadSceneMode mode) =>
        {
            switch (m_Phase)
            {
                case Phase.Title:
                    //タイトル
                    GameObject popup = PopupManager.MakePopup(
                        "TitlePopup",
                        _self.m_CameraRig,
                        _self.m_CameraRig.transform.Find("Camera").GetComponent<Camera>(),
                        _self.m_CameraRig.transform.position + (_self.m_CameraRig.transform.forward * 1.0f)
                    );
                    OnClickSwitchScene onclick = popup.AddComponent<OnClickSwitchScene>();
                    onclick.m_Phase = Phase.Ingame;
                    break;
                case Phase.Ingame:
                    Debug.Log("is here");
                    Debug.Log(GameObject.Find("/CameraRigPoint"));
                    //部屋を散らかす
                    _self.m_CameraRig.transform.position = GameObject.Find("/CameraRigPoint").transform.position;
                    Trash trash = GameObject.Find("/room_with_furniture").GetComponent<Trash>();
                    Debug.Log("camera position:" + _self.m_CameraRig.transform.position.y);
                    trash.TrashSpawn();
                    m_Timer = GameObject.Find("/Timer");
                    break;
                case Phase.Result:
                    //タイトル
                    GameObject result_popup = PopupManager.MakePopup(
                        "ResultPopup",
                        _self.m_CameraRig,
                        _self.m_CameraRig.transform.Find("Camera").GetComponent<Camera>(),
                        _self.m_CameraRig.transform.Find("Camera").transform.position + (_self.m_CameraRig.transform.forward * 1.0f)
                    );
                    float score = ScoreManager.GetScore();
                    result_popup.GetComponentInChildren<Text>().text = $@"
片付けたゴミ:{score}個
壊した家具の数:xxxxxx個
マッマの評価:SSS
ランク:わごむ級
";

                    OnClickSwitchScene result_onclick = result_popup.AddComponent<OnClickSwitchScene>();
                    result_onclick.m_Phase = Phase.Title;
                    break;
            }
        };
    }

    void LoadScene(Phase phase)
    {
        if (SceneManager.GetActiveScene().name != m_ScenePhaseLists[phase])
        {
            SceneManager.LoadScene(m_ScenePhaseLists[phase]);            
        }
    }
}