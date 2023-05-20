using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using Valve.VR;

public class RayCastTest : MonoBehaviour
{
    private LineRenderer m_Line;
    private FixedJoint m_FixedJoint;
    public GameObject m_Camerarig;
    private Ray m_Ray;

    private const int RAYCAST_LENGTH = 3;

    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Input_Sources handType;

    enum layer
    {
        Floor = 8,
        MovableObject = 9,
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Line = GetComponent<LineRenderer>();
        m_FixedJoint = GetComponent<FixedJoint>();
    }

    IEnumerator PickUp(GameObject obj)
    {
        while (Vector3.Distance(obj.transform.position,gameObject.transform.position)>=0.5f)
        {
            obj.transform.position = Vector3.Lerp(
                obj.transform.position,
                gameObject.transform.position, 0.3f);
            yield return new WaitForSeconds(0.01f);
        }
        Rigidbody target_rigidbody = obj.GetComponent<Rigidbody>();
        if (target_rigidbody != null)
        {
            m_FixedJoint.connectedBody = target_rigidbody;
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
        if (Physics.Raycast(m_Ray,out raycast_hit,RAYCAST_LENGTH))
        {
            //触れたオブジェクトが動かせるものなら枠を光らせる
            if ((layer) raycast_hit.transform.gameObject.layer == layer.MovableObject)
            {
                OutlineManager outline_manager = raycast_hit.transform.gameObject.GetComponent<OutlineManager>();
                if (outline_manager == null)
                {
                    raycast_hit.transform.gameObject.AddComponent<OutlineManager>();
                }
                //1フレームだけアウトラインを設定
                outline_manager.SetOutline();

            }
            
            //Debug.Log(raycast_hit.transform.gameObject.name);
            if (grabAction.GetStateDown(handType))
            {
                switch ((layer)raycast_hit.transform.gameObject.layer)
                {
                    case layer.Floor:
                        m_Camerarig.transform.position = raycast_hit.point;
                        break;
                    case layer.MovableObject:
                        MoveObject(raycast_hit);
                        break;
                }
                // Debug.Log(raycast_hit.point);
                // m_Camerarig.transform.position = raycast_hit.point;
            }

            if (grabAction.GetStateUp(handType))
            {
                switch ((layer)raycast_hit.transform.gameObject.layer)
                {
                    case layer.MovableObject:
                        ReleaseObject();
                        break;
                }
            }
        }

    }

    void MoveObject(RaycastHit raycast_hit)
    {

        StartCoroutine(PickUp(raycast_hit.transform.gameObject));
        // Rigidbody target_rigidbody = raycast_hit.transform.gameObject.GetComponent<Rigidbody>();
        // if (target_rigidbody != null)
        // {
        //     m_FixedJoint.connectedBody = target_rigidbody;
        // }
    }

    void ReleaseObject()
    {
        if (m_FixedJoint.connectedBody != null)
        {
            m_FixedJoint.connectedBody = null;
        }
    }

}