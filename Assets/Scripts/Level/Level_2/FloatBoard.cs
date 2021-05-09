using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatBoard : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody m_rig;
    public float speed;
    void Start()
    {
        m_rig = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        m_rig.velocity = new Vector3(0, 0, speed);
    }



    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("RobotPlayer"))
        {
            other.transform.position = new Vector3(this.transform.position.x, other.transform.position.y, this.transform.position.z);
            Physics.SyncTransforms();

        }
    }
}
