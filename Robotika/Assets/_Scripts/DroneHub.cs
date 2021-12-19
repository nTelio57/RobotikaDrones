using UnityEngine;

public class DroneHub : MonoBehaviour
{
    [SerializeField]
    private Drone[] Drones;

    public Transform[] LightLandingZones;
    private int LightLandingsOccupied;
    public Transform[] HeavyLandingZone;
    private int HeavyLandingsOccupied;

    public HeavyBoxOperation HeavyBoxOperation;

    private void Awake()
    {
        LightLandingsOccupied = 0;
        HeavyLandingsOccupied = 0;
        HeavyBoxOperation = new HeavyBoxOperation();
        Init();
    }

    private void Init()
    {
        foreach (var drone in Drones)
        {
            drone.SetDroneHub(this);
        }
    }

    public Vector3 GetLightLandingCoordinates()
    {
        if(LightLandingsOccupied >= LightLandingZones.Length)
            return Vector3.zero;
        return LightLandingZones[LightLandingsOccupied++].position;
    }

    public Vector3 GetHeavyLandingCoordinates()
    {
        if (HeavyLandingsOccupied >= HeavyLandingZone.Length)
            return Vector3.zero;
        return HeavyLandingZone[HeavyLandingsOccupied++].position;
    }

    public void RequestHelpForHeavyBox(Drone sender, Box box)
    {
        if(HeavyBoxOperation.IsActive)
            sender.SetControls(new RoamState(sender));//send to current operation

        HeavyBoxOperation.Start(sender, box);

        foreach (var drone in Drones)
        {
            if(!drone.Equals(sender))
                drone.SetAsHelper();
        }
    }

    public bool CheckIfDone()
    {
        if (HeavyLandingZone.Length == HeavyLandingsOccupied && LightLandingZones.Length == LightLandingsOccupied)
        {
            foreach (var drone in Drones)
            {
                drone.FlyHome();
            }
            return true;
        }
        return false;
    }
}
