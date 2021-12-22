using UnityEngine;

public class TravelingState : DroneControl
{
    private readonly bool _isDelivering;
    private readonly float _altitude;
    private readonly Vector3 _target;
    private readonly bool _isTravelingHome;

    public TravelingState(Drone drone, Vector3 target, bool isTravelingHome, bool isDelivering = false) : base(drone)
    {
        _isDelivering = isDelivering;
        _altitude = 25;
        Speed = 16;
        _target = target;
        _isTravelingHome = isTravelingHome;
        InitTarget(target);
    }
    public TravelingState(Drone drone, Vector3 target, bool isDelivering = false) : base(drone)
    {
        _isDelivering = isDelivering;
        _altitude = 25;
        Speed = 16;
        _target = target;
        InitTarget(target);
    }

    private void InitTarget(Vector3 target)
    {
        SetTarget(_isDelivering ? new Vector3(target.x, _altitude, target.z) : target);
    }

    public override void Control()
    {
        if (IsTargetReached())
        {
            if (_isDelivering)
            {
                Drone.SetControls(new DropoffState(Drone, _target));
                return;
            }

            if (!_isTravelingHome)
            {
                BoxType boxType = GetBoxType();
                switch (boxType)
                {
                    case BoxType.Light:
                        Drone.SetControls(new PickupState(Drone));
                        break;
                    case BoxType.Heavy:
                        Drone.SetControls(new WaitingState(Drone));
                        break;
                    case BoxType.None:
                        Drone.SetControls(new WaitingState(Drone));
                        break;
                }
            }
        }
        MoveTowardsTarget();
    }
    
    private BoxType GetBoxType()
    {
        var box = GetBox();
        return box != null ? box.BoxType : BoxType.None;
    }
}
