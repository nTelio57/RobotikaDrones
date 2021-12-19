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

    public override void MoveTowardsTarget()
    {
        /*if (Drone.IsHelping && !Drone.DroneHub.HeavyBoxOperation.Box.Equals(GetBox()))
        {
            Drone.SetControls(new TravelingState(Drone, Drone.DroneHub.HeavyBoxOperation.GetTravelTarget(Drone)));
            Drone.DroneHub.HeavyBoxOperation.AddHelperDrone(Drone);
        }*/


        Debug.Log(GetBox().gameObject.name);
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

    private Box GetBox()
    {
       /* if ((Physics.Raycast(Drone.transform.position, Drone.transform.TransformDirection(Vector3.down), out var hit,
            Altitude + 5, Drone.BoxLayer)))
        {
            var box = hit.transform.GetComponent<Box>();
            if (box != null)
                return box;
        }*/

        var collider = Drone.ConeDetector.GetCollider();
        var box = collider.transform.GetComponent<Box>();
        if (box != null)
            return box;

        return null;
    }
}
