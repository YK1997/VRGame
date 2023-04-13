using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRUserContoller : MonoBehaviour
{
    public GameObject m_Camerarig;

    public SteamVR_Action_Boolean m_TurnLeft;

    public SteamVR_Action_Boolean m_TurnRight;

    public SteamVR_Input_Sources m_Input;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_TurnRight.GetLastStateDown(m_Input))
        {
            m_Camerarig.transform.Rotate(0,45,0);
        }

        if (m_TurnLeft.GetLastStateDown(m_Input))
        {
            m_Camerarig.transform.Rotate(0,-45,0);
        }
    }
}
