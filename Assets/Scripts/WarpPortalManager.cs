using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Valve.VR;

public class WarpPortalManager : MonoBehaviour
{
    //public SteamVR_Action_Boolean m_aAction;

    [SerializeField] 
    private GameObject m_LineObject;
    private LineRenderer m_line;
    private int m_Raylength = 5;
    private bool m_IsRotate = false;

    [SerializeField]
    private GameObject m_Portal;
    public static GameManager _self;
    
    // Start is called before the first frame update
    void Awake()
    {
//        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(m_LineObject);
        DontDestroyOnLoad(m_Portal);
        m_line = m_LineObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void EraceCurveRay()
    {
        // Debug.Log("ERASE!!!!!!");
        m_LineObject.SetActive(false);
        m_Portal.SetActive(false);
    }

    public void DrawCurveRay(
        GameObject controller,
        SteamVR_Action_Boolean grabAction,
        SteamVR_Input_Sources handType,
        GameObject warp_object
    )
    {
        Ray ray = new Ray(controller.transform.position, 
            controller.transform.forward);
        RaycastHit hit;
        bool is_raycast_hit = Physics.Raycast(
            ray, out hit, m_Raylength,LayerMask.GetMask(new string[]{"Floor","UI"}));
        Vector3 raycast_end;
        
        if (is_raycast_hit)
        {
            switch ((MoveObject.layer)hit.transform.gameObject.layer)
            {
                case MoveObject.layer.Floor:
                    
                    m_LineObject.SetActive(true);
                    m_Portal.SetActive(true);
                    
                    raycast_end = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                    m_Portal.transform.position = raycast_end; 
                    float height = controller.transform.position.y - raycast_end.y;
                    DrawLine(m_line,controller.transform.position,raycast_end,height,Color.blue);
                    if(grabAction.GetLastStateDown(handType))
    //                        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
                    {
                        //raycastでポイントした位置に移動する
                        Debug.Log("warp");
                        WarpToRaycastHit(hit,warp_object);
                    }
                    break;
            }
        }
        else
        {
            m_LineObject.SetActive(false);
            m_Portal.SetActive(false);
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="raycast_end"></param>
    /// <param name="target"></param>
    bool MovePoint(
        GameObject controller,
        RaycastHit hit,
        GameObject target,
        GameObject WarpObject
        )
    {
        bool ret = false;
        m_LineObject.SetActive(true);
        m_Portal.SetActive(false);
        Vector3 raycast_end = new Vector3(hit.point.x, hit.point.y, hit.point.z);
        DrawLine(m_line,controller.transform.position,raycast_end,0,Color.red);
        if (false)
//        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            //移動する
            WarpObject.transform.position = target.transform.position;
            WarpObject.transform.rotation = target.transform.rotation;
            ret = true;
        }
        return ret;
    }

    void DrawLine(LineRenderer line,Vector3 start,Vector3 end, float height, Color start_color)
    {
        //都度LineRendererを表示
        int vertex_count = 50;
        line.SetVertexCount(vertex_count);
        // portal.transform.position = end; 
        // float height = start.y - end.y;
        Vector3 highest_point = Vector3.Lerp(start, end, 0.75f)+(Vector3.up*height);
        List<Vector3> vertexes = new List<Vector3>();
        for (int i=0;i<vertex_count;i++)
        {
            float degree = (float)i/(float)vertex_count;
            Vector3 a = Vector3.Lerp(start, highest_point, degree);
            Vector3 b = Vector3.Lerp(highest_point, end, degree);
            vertexes.Add(Vector3.Lerp(a,b,degree));
        }
        line.startColor = start_color;
        line.endColor = Color.white;
        line.SetPositions(vertexes.ToArray());
        //特定の高さまで
    }
    
    /// <summary>
    /// ワープします。
    /// </summary>
    /// <param name="hit"></param>
    /// <param name="action"></param>
    /// <param name="m_Input"></param>
    /// <param name="warp_object"></param>
    void WarpToRaycastHit(
        RaycastHit hit,
        GameObject warp_object){
        //移動する
        warp_object.transform.position = new Vector3(
            hit.point.x,
            warp_object.transform.position.y,
            hit.point.z
        );
    }
    
    
    /// <summary>
    /// フェードアウト
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="interval"></param>
    /// <returns></returns>
    IEnumerator FadeOut(
        GameObject obj,float interval=0.01f)
    {
        m_IsFade = true;
        while ( obj.GetComponent<CanvasGroup>().alpha > 0 )
        {
            obj.GetComponent<CanvasGroup>().alpha -= 0.05f;
            yield return new WaitForSeconds(interval);
        }
        //ここで完全に非表示にする
        obj.transform.GetChild(0).gameObject.SetActive(false);
        m_IsFade = false;
    }

    /// <summary>
    /// フェードイン
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="interval"></param>
    /// <returns></returns>
    IEnumerator FadeIn(
        GameObject obj,float interval=0.01f)
    {
        m_IsFade = true;
        //ここで表示する
        obj.transform.GetChild(0).gameObject.SetActive(true);
        while ( obj.GetComponent<CanvasGroup>().alpha < 1 )
        {
            obj.GetComponent<CanvasGroup>().alpha += 0.05f;
            yield return new WaitForSeconds(interval);
        }
        m_IsFade = false;
    }
   

    private bool m_IsFade = false;
//
    void SwitchUIFade(GameObject obj)
    {
        if (m_IsFade)
        {
            return;
        }
        
        if (obj.GetComponent<CanvasGroup>().alpha >= 1)
        {
            StartCoroutine(FadeOut(obj));
        }
        else if(obj.GetComponent<CanvasGroup>().alpha <= 0)
        {
            StartCoroutine(FadeIn(obj));
        }
    }
}
