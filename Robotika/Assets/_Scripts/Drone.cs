using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField]
    private DroneControl _control;
    public float Speed = 3;
    public float RotationSpeed = 5;

    public Collider FlyArea;
    public float DroneDetectDistance = 3.5f;
    public ConeDetector ConeDetector;

    public LayerMask Layer;
    public LayerMask BoxLayer;

    public DroneHub DroneHub;
    private Vector3 _startingPad;
    public bool SentHome;
    public bool IsHelping;

    public string State;

    void Awake()
    {
        _startingPad = transform.position;
        _control = new RoamState(this);
        IsHelping = false;
        SentHome = false;
    }
    
    void Update()
    {
        State = _control.ToString();
        _control.MoveTowardsTarget();
        _control.RotateTowardsTarget();
    }

    public void SetControls(DroneControl control)
    {
        _control = control;
    }

    public void SetDroneHub(DroneHub droneHub)
    {
        DroneHub = droneHub;
    }

    public void SetAsHelper()
    {
        IsHelping = true;
        DroneHub.HeavyBoxOperation.AddHelperDrone(this);
        switch (_control)
        {
            case RoamState:
                _control = new TravelingState(this, DroneHub.HeavyBoxOperation.Box.GetHook().transform.position);
                break;
        }
    }

    public void FlyHome()
    {
        SentHome = true;

        switch (_control)
        {
            case RoamState:
                _control = new TravelingState(this, _startingPad, true, false);
                break;
        }
        
    }
}
