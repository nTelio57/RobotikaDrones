using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HeavyBoxOperation
{
    public bool IsActive;
    public Drone Head;
    private List<Drone> _helpers;
    public Box Box;
    public int AttachedDronesCount;

    public HeavyBoxOperation()
    {
        _helpers = new List<Drone>();
        IsActive = false;
        AttachedDronesCount = 0;
    }
    
    public void AddHelperDrone(Drone drone)
    {
        if(!_helpers.Contains(drone))
            _helpers.Add(drone);
    }
    
    public List<Drone> GetHelperDrones()
    {
        return _helpers;
    }
    
    public void Start(Drone head, Box box)
    {
        Head = head;
        Head.IsHelping = true;
        IsActive = true;
        Box = box;
        AttachedDronesCount = 0;
    }
    
    public void Reset()
    {
        Head.IsHelping = false;
        foreach (var helper in _helpers)
        {
            helper.IsHelping = false;
        }
        Head = null;
        _helpers = new List<Drone>();
        IsActive = false;
        Box = null;
        AttachedDronesCount = 0;
    }
    
    public void SetDroneAttached()
    {
        AttachedDronesCount++;
    }
    
    public Vector3 GetTravelTarget(Drone drone)
    {
        int index = 0;
        if(!drone.Equals(Head))
            index = _helpers.IndexOf(drone)+1;
        return Box.Hooks[index].DronePosition.position;
    }
}
