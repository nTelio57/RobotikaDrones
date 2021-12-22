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
            IsDetected = true;
            _other = other;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            IsDetected = true;
            _other = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            IsDetected = false;
            _other = null;
        }
    }
    
    public Collider GetCollider()
    {
        return _other;
    }

}