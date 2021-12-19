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

    public override void MoveTowardsTarget()
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
            Ascend();
        }
    }

    private bool Attach()
    {
        var boxObject = GetBoxBelow();
        var box = boxObject.transform.GetComponent<Box>();
        if (box != null)
        {
            if(box.BoxType == BoxType.Heavy)
                return HeavyAttach(boxObject.transform);
            return LightAttach(boxObject.transform);
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

    private void Ascend()
    {
        var directionVector = Vector3.MoveTowards(Drone.transform.position, Target, Drone.Speed * Time.deltaTime);
        Drone.transform.position = directionVector;
    }
}
