using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Fukidashi : MonoBehaviour
{
    public const int FAILED_TO_CREATE_FUKIDASHI = 0;
    /// <summary>
    /// グローバル座標用ふきだし
    /// </summary>
    /// <param name="text"></param>
    /// <param name="target"></param>
    /// <param name="camera"></param>
    /// <param name="pos"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public static GameObject MakeFukdashi(
        string text,
        GameObject target,
        Camera camera,
        Vector3 pos,
        Vector2 size ){
        //サイズ固定
        GameObject fukidashi = 
            GameObject.Instantiate(Resources.Load<GameObject>("fukidashi_world"));
        if (fukidashi==null)
        {
            return null;
        }
        fukidashi.GetComponent<Canvas>().worldCamera = camera;
        fukidashi.transform.position = pos;
        fukidashi.transform.Find("fukidashi/text").GetComponent<Text>().text = text;
        fukidashi.transform.Find("fukidashi/bg_white/shadow").GetComponent<RectTransform>().sizeDelta = size;
        fukidashi.transform.Find("fukidashi/bg_white/bg").GetComponent<RectTransform>().sizeDelta = size;
        return fukidashi;
    }

    private static Fukidashi m_Self;
    
    void Start()
    {
        m_Self = this;
        Debug.Log(m_Self);
    }

    void Update()
    {
        //見続ける処理
//        fukidashi.transform.LookAt(target.transform);
    }
    

    public static void Close(GameObject fukidashi,float close_time)
    {
        if (m_Self == null)
        {
            //TODO:オブジェクトがないので、適当に生成せずに何もしない
            //TODO:いっそawait実装でコルーチン外せば楽かもしれない
            return;
        }
        m_Self.CloseFukidashi(fukidashi,close_time);
    }

    public void CloseFukidashi(GameObject fukidashi,float close_time)
    {
        StartCoroutine(CloseAndAnimation(fukidashi,close_time));
    }
    
    public IEnumerator CloseAndAnimation(GameObject fukidashi,float close_time)
    {
        fukidashi.transform.Find("fukidashi").GetComponent<Animator>().CrossFade("wipe_close",close_time);
        yield return new WaitForSeconds(close_time);
        GameObject.Destroy(fukidashi);
    }
}