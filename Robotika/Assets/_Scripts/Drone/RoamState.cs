using UnityEngine;

public class RoamState : DroneControl
{
    public RoamState(Drone drone) : base(drone)
    {
        Speed = 12;
        Altitude = 30;
        Drone.Speed = Speed;

        SetTarget(RandomCoords());
    }

    public override void Control()
    {
        if(Drone.SentHome)
            Drone.FlyHome();

        IsBoxBelow();
        if (IsTargetReached() || IsObstacleAhead())
        {
            Target = RandomCoords();
        }
        MoveTowardsTarget();
    }

    private Vector3 RandomCoords()
    {
        var bounds = Drone.FlyArea.bounds;
        float scaleX = bounds.size.x / 2;
        float scaleZ = bounds.size.z / 2;
        Vector3 center = Drone.FlyArea.transform.position;

        float x = Random.Range(center.x - scaleX, center.x + scaleX);
        float z = Random.Range(center.z - scaleZ, center.z + scaleZ);

        return new Vector3(x, Altitude, z);
    }
    
    Vector3 GetBoxCoordinates(Transform boxTransform)
    {
        Vector3 boxCenter = boxTransform.position;
        float boxHeight = boxTransform.localScale.y;
        float droneHeight = Drone.transform.localScale.y;

        float y = boxCenter.y + boxHeight / 2 + droneHeight / 2;

        return new Vector3(boxCenter.x, y, boxCenter.z);
    }
    
    private bool IsBoxBelow()
    {
        if(Drone.ConeDetector.IsDetected)
        {
            var box = GetBox();
            if (box != null && !box.IsNoticed)
            {
                if (box.BoxType == BoxType.Heavy)
                {
                    var operation = Drone.DroneHub.HeavyBoxOperation;
                    if (operation.IsActive)
                        return false;
                    Drone.DroneHub.RequestHelpForHeavyBox(Drone, box);
                }
                var boxLandingPosition = box.BoxType == BoxType.Heavy
                    ? box.GetHook().DronePosition.position
                    : GetBoxCoordinates(box.transform);

                box.IsNoticed = true;
                Drone.SetControls(new TravelingState(Drone, boxLandingPosition));
                return true;
            }
        }

        return false;
    }
}
