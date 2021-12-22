using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingState : DroneControl
{
    private bool _isAttached;
    public WaitingState(Drone drone) : base(drone)
    {
        _isAttached = false;
        if (!Drone.IsHelping && !Drone.DroneHub.HeavyBoxOperation.IsActive)
        {
            Drone.DroneHub.RequestHelpForHeavyBox(Drone, GetBox());
        }
    }

    public override void Control()
    {
        if (Drone.IsHelping && Drone.DroneHub.HeavyBoxOperation.Box.Equals(GetBox()) && !_isAttached)
        {
            _isAttached = true;
            Drone.DroneHub.HeavyBoxOperation.SetDroneAttached();
        }

        if (Drone.DroneHub.HeavyBoxOperation.AttachedDronesCount == 4 && Drone.Equals(Drone.DroneHub.HeavyBoxOperation.Head))
        {
            Drone.SetControls(new PickupState(Drone));
        }
    }
}
