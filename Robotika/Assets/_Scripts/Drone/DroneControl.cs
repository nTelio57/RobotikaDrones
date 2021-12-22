using System;
using UnityEngine;

public abstract class DroneControl
{
    protected Drone Drone;
    protected Vector3 Target;
    protected float Speed;
    protected float Altitude;

    protected DroneControl(Drone drone)
    {
        Drone = drone;
    }

    public void SetTarget(Vector3 target)
    {
        Target = target;
    }

    public bool IsTargetReached()
    {
        return Vector3.Distance(Target, Drone.transform.position) <= 0.1;
    }

    public void RotateTowardsTarget()
    {
        var transform = Drone.transform;
        Vector3 targetDirection = Target - transform.position;
        var newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Drone.RotationSpeed * Time.deltaTime, 0);
        var newRotation = Quaternion.LookRotation(newDirection).eulerAngles;
        newRotation.x = 0;
        Drone.transform.rotation = Quaternion.Euler(newRotation);
    }

    protected bool IsObstacleAhead()
    {
        return (Physics.Raycast(Drone.transform.position, Drone.transform.TransformDirection(Vector3.forward),
            out var hit,
            Drone.DroneDetectDistance, Drone.Layer));
    }

    protected Box GetBox()
    {
        var collider = Drone.ConeDetector.GetCollider();
        var box = collider.transform.GetComponent<Box>();
        if (box != null)
            return box;

        return null;
    }

    protected void MoveTowardsTarget()
    {
        var directionVector = Vector3.MoveTowards(Drone.transform.position, Target, Drone.Speed * Time.deltaTime);
        Drone.transform.position = directionVector;
    }

    public abstract void Control();
}
