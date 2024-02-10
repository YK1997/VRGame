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
    public GameObject m_Camera;
    public GameObject m_Timer;
    private const int GAME_TIMER_SECOND = 600;

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
    
    
    
    Vector3 CorrectCameraPosition()
    {
        return new Vector3(
            m_CameraRig.transform.position.x - m_Camera.transform.localPosition.x,
            m_Camera.transform.position.y,
            m_CameraRig.transform.position.z - m_Camera.transform.localPosition.z
        );
    }
    
    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {
        _self = this;
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(this.m_CameraRig);
//        DontDestroyOnLoad(this.m_Camera);
        m_Camera = m_CameraRig.transform.Find("Camera").gameObject;
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
                    popup.transform.Translate(0,1,0); 
                    OnClickSwitchScene onclick = popup.AddComponent<OnClickSwitchScene>();
                    onclick.m_Phase = Phase.Ingame;
                    
                    //タイトル
                    GameObject tutorial_popup = PopupManager.MakePopup(
                        "TutorialPopup",
                        _self.m_CameraRig ,
                        _self.m_CameraRig.transform.Find("Camera").GetComponent<Camera>(),
                        _self.m_CameraRig.transform.position + (_self.m_CameraRig.transform.forward * 1.0f)
                    );
                    tutorial_popup.transform.Translate(-2.015f,1.14f,0); 

                    break;
                case Phase.Ingame:
//                    Debug.Log("is here");
//                    Debug.Log(GameObject.Find("/CameraRigPoint"));
                    //部屋を散らかす
                    _self.m_CameraRig.transform.position = GameObject.Find("/CameraRigPoint").transform.position;
                    // Trash trash = GetComponentInChildren<Trash>();
                    Trash trash = GameObject.Find("/Class").GetComponent<Trash>();
//                    Debug.Log("camera position:" + _self.m_CameraRig.transform.position.y);
                    trash.InitTrashes();
                    m_Timer = GameObject.Find("/Timer");
                    //とりあえず5分
                    m_Timer.GetComponent<CountDownTimer>().SetSeconds(GAME_TIMER_SECOND);
                    break;
                case Phase.Result:
                    //タイトル
                    GameObject result_popup = PopupManager.MakePopup(
                        "ResultPopup",
                        _self.m_CameraRig,
                        _self.m_CameraRig.transform.Find("Camera").GetComponent<Camera>(),
                        _self.m_CameraRig.transform.Find("Camera").transform.position + 
                        (_self.m_CameraRig.transform.forward * 1.0f)
                    );
                    float score = ScoreManager.GetScore();
                    result_popup.GetComponentInChildren<Text>().text = $@"
片付けたゴミ:{score}個
壊した家具の数:xxxxxx個
マッマの評価:SSS
ランク:わごむ級
";
                    ScoreManager.ResetScore();
                    EnemyManager.ResetEnemyList();
                    OnClickSwitchScene result_onclick = result_popup.AddComponent<OnClickSwitchScene>();
                    result_onclick.m_Phase = Phase.Title;
                    
                    //終わった時のキンコンカンコン
                    // GameObject.Find("Speaker").GetComponent<AudioSource>().Play();
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

    public static GameObject GetCamera()
    {
        return _self.m_CameraRig.transform.Find("Camera").gameObject;
    }
}