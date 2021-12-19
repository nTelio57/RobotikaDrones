using System;
using UnityEngine;

[Serializable]
public abstract class DroneControl
{
    protected Drone Drone;
    [SerializeField]
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

    protected Collider GetBoxBelow()
    {
        return Drone.ConeDetector.GetCollider();
    }

    public abstract void MoveTowardsTarget();
}
