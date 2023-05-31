using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public const int FAILED_TO_CREATE_POPUP = 0;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="popup_resource_name"></param>
    /// <param name="target"></param>
    /// <param name="camera"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static GameObject MakePopup(
        string popup_resource_name,
        GameObject target,
        Camera camera,
        Vector3 pos){
        //サイズ固定
        GameObject Popup = 
            GameObject.Instantiate(Resources.Load<GameObject>(popup_resource_name));
        if (Popup==null)
        {
            return null;
        }
        Popup.GetComponent<Canvas>().worldCamera = camera;
        Popup.transform.position = pos;
        return Popup;
    }

    private static PopupManager m_Self;
    
    void Start()
    {
        m_Self = this;
        Debug.Log(m_Self);
    }

    void Update()
    {
        //見続ける処理
//        Popup.transform.LookAt(target.transform);
    }
    

    public static void Close(GameObject Popup,float close_time)
    {
        if (m_Self == null)
        {
            //TODO:オブジェクトがないので、適当に生成せずに何もしない
            //TODO:いっそawait実装でコルーチン外せば楽かもしれない
            return;
        }
        m_Self.ClosePopup(Popup,close_time);
    }

    public void ClosePopup(GameObject Popup,float close_time)
    {
        StartCoroutine(CloseAndAnimation(Popup,close_time));
    }
    
    public IEnumerator CloseAndAnimation(GameObject Popup,float close_time)
    {
        Popup.transform.Find("Popup").GetComponent<Animator>().CrossFade("wipe_close",close_time);
        yield return new WaitForSeconds(close_time);
        GameObject.Destroy(Popup);
    }    
    
//    void OpenPopup()
//    {
//        
////        yield return new WaitForSeconds(1.0f);
//        GameObject canvas = GameObject.Find("/Canvas");
//        GameObject popup = (GameObject)GameObject.Instantiate(Resources.Load("simple_popup"));
//        popup.transform.SetParent(canvas.transform);
//        RectTransform rect = popup.GetComponent<RectTransform>();
//        rect.localPosition = new Vector3(0,0,0);
////        yield return new WaitForSeconds(3.0f);
//    }
}