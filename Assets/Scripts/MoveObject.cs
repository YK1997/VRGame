using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using Valve.VR;

public class MoveObject : MonoBehaviour
{
    private LineRenderer m_Line;
    private FixedJoint m_FixedJoint;
    public GameObject m_Camerarig;
    public GameObject m_Camera;
    private Ray m_Ray;
    private Rigidbody m_Rigidbody;// KONAKA:ADD 2023.8.19
    
    private const int RAYCAST_LENGTH = 3;
    
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Input_Sources handType;
    
    public SteamVR_Action_Boolean m_TurnLeft;
    public SteamVR_Action_Boolean m_TurnRight;
//      
    enum layer
    {
        Default = 0,
        Water = 4,
        UI = 5,
        Floor = 8,
        MovableObject = 9,
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Line = GetComponent<LineRenderer>();
        m_FixedJoint = GetComponent<FixedJoint>();
        m_Camera = m_Camerarig.transform.Find("Camera").gameObject;
    }
    
    IEnumerator PickUp(GameObject obj)
    {
        while (obj != null && Vector3.Distance(obj.transform.position,gameObject.transform.position)>=0.2f)
        {
            Debug.Log(obj.name);
            Debug.Log(obj.transform.position);
            Debug.Log(gameObject.name);
            Debug.Log(gameObject.transform.position);

            obj.transform.position = Vector3.Lerp(
                obj.transform.position,
                gameObject.transform.position, 0.9f);
            Debug.Log(obj.name);
            Debug.Log(obj.transform.position);
            Debug.Log(gameObject.name);
            Debug.Log(gameObject.transform.position);

            yield return new WaitForSeconds(0.01f);
        }
        
        if (obj != null)
        {
            Rigidbody target_rigidbody = obj.GetComponent<Rigidbody>();
            if (target_rigidbody != null)
            {
                
                m_FixedJoint.connectedBody = target_rigidbody;
            }            
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_Line.SetPosition(0,transform.position);
        //Debug.Log(transform.forward);
        m_Line.SetPosition(1,transform.position+(transform.forward*RAYCAST_LENGTH));

        m_Ray = new Ray(transform.position,transform.forward);
        RaycastHit raycast_hit;
        Debug.DrawRay(transform.position,transform.forward*RAYCAST_LENGTH, Color.green, 0.1f, false);
        if (Physics.Raycast(m_Ray,out raycast_hit,RAYCAST_LENGTH))
        {
            //----------------------------------------------------------
            //竹田追加 ボタン押下処理を追加
            //----------------------------------------------------------
            if (grabAction.GetLastStateDown(handType))
            {
                //対象のオブジェクトでクリック扱いにする。
                //レシーバがなくてもエラー表示にさせない
                raycast_hit.transform.gameObject.SendMessage("OnClick",
                    new Collision(),SendMessageOptions.DontRequireReceiver);
            }
            //----------------------------------------------------------
            //竹田追加 ボタン押下処理を追加
            //----------------------------------------------------------
            
            //触れたオブジェクトが動かせるものなら枠を光らせる
            if ((layer) raycast_hit.transform.gameObject.layer == layer.MovableObject)
            {
                try
                {
                    OutlineManager outline_manager = raycast_hit.transform.gameObject.GetComponent<OutlineManager>();
                    if (outline_manager == null)
                    {
                        raycast_hit.transform.gameObject.AddComponent<OutlineManager>();
                    }
                }
                catch (Exception e)
                {
                    //OutlineManagerがない場合エラーを吐かないようにする
                }
                //1フレームだけアウトラインを設定
                //OutlineManager.SetOutline();
            }

            // ボタン押されたとき
            //Debug.Log(raycast_hit.transform.gameObject.name);
            if (grabAction.GetStateDown(handType))
            {
                Debug.Log("GetStateDown");
//                Debug.Log(raycast_hit.transform.gameObject.name);
//                Debug.Log(raycast_hit.transform.gameObject.transform.position);
//                Debug.Log(gameObject.name);
//                Debug.Log(gameObject.transform.position);
//                Debug.Log(raycast_hit.transform.gameObject.layer);  // KONAKA
                switch ((layer)raycast_hit.transform.gameObject.layer)
                {
                    case layer.MovableObject:
                        ObjectMove(raycast_hit);
                        break;
                    case layer.Floor:
                        WarpToRaycastHit(
                            raycast_hit,
                            m_Camerarig,
                            m_Camera.transform.localPosition);
                        break;
                }
            }

            // ボタン離したとき
            if (grabAction.GetStateUp(handType))
            {
                Debug.Log("GetStateUp");
//                Debug.Log(raycast_hit.transform.gameObject.name);
//                Debug.Log(raycast_hit.transform.gameObject.transform.position);
//                Debug.Log(gameObject.name);
//                Debug.Log(gameObject.transform.position);
//                Debug.Log(raycast_hit.transform.gameObject.layer);  // KONAKA
//                switch ((layer)raycast_hit.transform.gameObject.layer)
//                {
//                    case layer.MovableObject:
//                        
                        ReleaseObject();
//                        
//                        break;
//                }
            }
            
        }
        //左右回転
        Turn();
    }
    
    void ObjectMove(RaycastHit raycast_hit)
    {
        StartCoroutine(PickUp(raycast_hit.transform.gameObject));
    }

    void ReleaseObject()
    {
//        if (m_FixedJoint.connectedBody != null)
//        {
            m_FixedJoint.connectedBody = null;
//        }
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
        GameObject warp_object,
        Vector3 correct_position)
    {
        //物を持っていたら移動させない
        
        if (m_FixedJoint.connectedBody != null)
        {
            return;
        }
        warp_object.transform.position = new Vector3(
            hit.point.x - correct_position.x,
            warp_object.transform.position.y,
            hit.point.z - correct_position.z
        );
    }
    void Turn()
    {
        //左右回転
        if (m_TurnRight.GetLastStateDown(handType))
        {
            m_Camera.transform.Rotate(0,45,0);
            
        }
        if (m_TurnLeft.GetLastStateDown(handType))
        {
            m_Camera.transform.Rotate(0,-45,0);
        }
    }
}
