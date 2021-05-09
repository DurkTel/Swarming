using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Swarming;

public class RotateAroundEffect : MonoBehaviour
{
    public Vector3 rot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(this.transform.parent.position, rot, 0.5f);
    }
}
