using UnityEngine;

public class PickupState : DroneControl
{
    private readonly float _altitude;
    private bool _isAttached;
    private Vector3 _target;
    public PickupState(Drone drone) : base(drone)
    {
        _altitude = 6;
        _isAttached = false;
    }

    public override void Control()
    {
        if (!_isAttached && Attach())
        {
            _isAttached = true;
            var position = Drone.transform.position;
            SetTarget(new Vector3(position.x, _altitude, position.z));
        }
            
        if (_isAttached)
        {
            if (IsTargetReached())
            {
                Drone.SetControls(new TravelingState(Drone, _target, true));
            }
            MoveTowardsTarget();
        }
    }
    
    private bool Attach()
    {
        var box = GetBox();
        if (box != null)
        {
            if(box.BoxType == BoxType.Heavy)
                return HeavyAttach(box.transform);
            return LightAttach(box.transform);
        }
        
        return false;
    }
    
    private bool LightAttach(Transform transform)
    {
        _target = Drone.DroneHub.GetLightLandingCoordinates();
        transform.parent = Drone.transform;
        return true;
    }
    
    private bool HeavyAttach(Transform transform)
    {
        _target = Drone.DroneHub.GetHeavyLandingCoordinates();
        transform.parent = Drone.transform;
        foreach (var helperDrone in Drone.DroneHub.HeavyBoxOperation.GetHelperDrones())
        {
            helperDrone.transform.parent = Drone.transform;
        }
        return true;
    }
}
