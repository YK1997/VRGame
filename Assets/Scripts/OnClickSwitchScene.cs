using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class OnClickSwitchScene : MonoBehaviour
{
    [FormerlySerializedAs("m_Scene")] public GameManager.Phase m_Phase = GameManager.Phase.None;
    public void OnClick(Collision other)
    {
        if (GameManager.Phase.None != m_Phase)
        {
            GameManager.SwitchPhase(m_Phase);
        }
    }
}
