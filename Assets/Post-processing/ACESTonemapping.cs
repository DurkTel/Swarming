using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]
public class ACESTonemapping : MonoBehaviour
{
    public Material material;
    // Start is called before the first frame update
    void Start()
    {
        if(material == null || material.shader == null || material.shader.isSupported == false)
        {
            enabled = false;
            return;
        }
    }

    // Update is called once per frame
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material, 0);
    }
}
