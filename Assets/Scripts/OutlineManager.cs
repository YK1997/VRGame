using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFx.Outline;

[RequireComponent(typeof(OutlineBehaviour))]

public class OutlineManager : MonoBehaviour
{
    private bool m_IsOutline = false;

    private OutlineBehaviour m_OutlineBehaviour;
    
    // Start is called before the first frame update
    void Start()
    {
        m_OutlineBehaviour = gameObject.GetComponent<OutlineBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsOutline)
        {
            AddOutlineObject();
        }
        else
        {
            RemoveOutlineObject();
        }

        m_IsOutline = false;
    }
    
    public void SetOutline()
    {
        m_IsOutline = true;
    }

    void AddOutlineObject()
    {
        m_OutlineBehaviour.OutlineSettings = new OutlineSettings()
        {
            OutlineWidth = 10,
            OutlineColor = Color.yellow
        };
    }

    void RemoveOutlineObject()
    {
        m_OutlineBehaviour.OutlineSettings = new OutlineSettings() {OutlineWidth = 0};
    }

}
