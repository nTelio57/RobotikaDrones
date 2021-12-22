﻿using UnityEngine;

public class DropoffState : DroneControl
{
    private bool _isAttached;
    private readonly int _altitude;
    private bool _descended;

    public DropoffState(Drone drone, Vector3 target) : base(drone)
    {
        _isAttached = true;
        _descended = false;
        _altitude = 8;
        SetTarget(target);
    }
    public override void Control()
    {
        if (IsTargetReached())
        {
            if (_isAttached && Deattach())
            {
                _isAttached = false;
                _descended = true;
                var position = Drone.transform.position;
                SetTarget(new Vector3(position.x, _altitude, position.z));
            }
            else if (!_isAttached && _descended)
            {
                Drone.DroneHub.CheckIfDone();
                Drone.SetControls(new RoamState(Drone));
                if (Drone.IsHelping)
                    Drone.SetAsHelper();
            }
        }
        MoveTowardsTarget();
    }
    
    private bool Deattach()
    {
        var box = GetBox();
        if (box != null)
        {
            if (box.BoxType == BoxType.Heavy)
                return HeavyBoxDeattach(box.transform);
            return LightBoxDeattach(box.transform);
        }

        return false;
    }
    
    private bool LightBoxDeattach(Transform transform)
    {
        transform.parent = null;
        return true;
    }

    private bool HeavyBoxDeattach(Transform transform)
    {
        transform.parent = null;
        var childDrones = Drone.GetComponentsInChildren<Drone>();
        foreach (var drone in childDrones)
        {
            if (!drone.Equals(Drone))
            {
                drone.transform.parent = null;
                drone.SetControls(new RoamState(drone));
            }
        }
        Drone.DroneHub.HeavyBoxOperation.Reset();

        return true;
    }
}
