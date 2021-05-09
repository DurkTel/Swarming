using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SokobanTips : MonoBehaviour
{
    public Texture[] arrows;
    private bool isReady;
    List<MeshRenderer> m_Materials = new List<MeshRenderer>();
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            m_Materials.Add(transform.GetChild(i).GetComponent<MeshRenderer>());
        }
    }


    public void PlayTipsAni(int arrow)
    {
        if (!isReady) return;
        foreach (var item in m_Materials)
        {
            item.gameObject.SetActive(true);
            item.material.SetTexture("_MainTex", arrows[arrow]);
            StartCoroutine(TipsAni(0.2f, item.material, false));
        }
    }

    IEnumerator TipsAni(float time, Material material, bool reverse)
    {
        bool end;
        float currentTime;
        currentTime = reverse ? 1.0f : 0f;
        while (true)
        {
            yield return new WaitForFixedUpdate();
            currentTime += reverse ? -Time.deltaTime / time : Time.deltaTime / time;
            material.SetFloat("_Alpha", Mathf.Max(0.2f, currentTime / 0.5f));
            material.SetFloat("_intensity", currentTime * 1.3f);
            end = reverse ? currentTime <= 0f : currentTime >= 1.0f;
            if (end) break;
        }
        if (!reverse)
            StartCoroutine(TipsAni(time + 2f, material, !reverse));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RobotPlayer"))
        {
            isReady = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RobotPlayer"))
        {
            isReady = false;
        }
    }
}
