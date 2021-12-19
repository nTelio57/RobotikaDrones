using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class ConeDetector : MonoBehaviour
{
    public Collider ConeCollider;
    public bool IsDetected;
    public LayerMask BoxLayer;
    private Collider _other;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Debug.Log("Enter");
            IsDetected = true;
            _other = other;
        }
            
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Debug.Log("Stay");
            IsDetected = true;
            _other = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Debug.Log("Exit");
            IsDetected = false;
            _other = null;
        }
    }

    public Collider GetCollider()
    {
        return _other;
    }

}