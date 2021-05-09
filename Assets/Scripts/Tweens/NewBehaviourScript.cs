using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    MeshRenderer m_meshRenderer;
    public int num;
    void Start()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_meshRenderer.material.renderQueue -= num;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
