using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAutoClose : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Disable", 3f);
    }

    private void Disable()
    {
        PoolManager.Instance.Push(this.name, this.gameObject);
    }
}
