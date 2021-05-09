using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WallFade : MonoBehaviour
{
    public GameObject wall;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(MaterialFade(true));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(MaterialFade(false));
        }
    }


    IEnumerator MaterialFade(bool isFade)
    {
        if (isFade)
        {
            for (float i = 1f; i >= 0; i -= Time.deltaTime)
            {
                wall.GetComponent<MeshRenderer>().material.SetFloat("_transparency", i);
                yield return 0;
            }
        }
        else
        {
            for (float i = 0f; i <= 1; i += Time.deltaTime)
            {
                wall.GetComponent<MeshRenderer>().material.SetFloat("_transparency", i);
                yield return 0;
            }
        }
    }

}
