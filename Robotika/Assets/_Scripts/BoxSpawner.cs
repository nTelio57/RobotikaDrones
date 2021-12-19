using UnityEngine;
using Random = UnityEngine.Random;

public class BoxSpawner : MonoBehaviour
{
    public GameObject HeavyBox;
    public int HeavyCount = 2;

    public GameObject LightBox;
    public int LightCount = 5;

    public LayerMask GroundLayer;
    public Collider collider;
    
    void Start()
    {
        SpawnBoxes(HeavyBox, HeavyCount);
        SpawnBoxes(LightBox, LightCount);
    }

    private void SpawnBoxes(GameObject prefab, int count)
    {
        for(int i = 0; i < count; i++)
        {
            float height = prefab.transform.localScale.y;
            var randomCoords = RandomCoords(height / 2);
            while (IsColliding(randomCoords, prefab.transform.localScale/2))
            {
                height = prefab.transform.localScale.y;
                randomCoords = RandomCoords(height / 2);
            }
            Instantiate(prefab, randomCoords, Quaternion.identity, transform);
        }
    }

    private bool IsColliding(Vector3 center, Vector3 range)
    {
        Collider[] colliders = Physics.OverlapBox(center, range, Quaternion.identity, ~GroundLayer);
        return colliders.Length > 0;
    }

    private Vector3 RandomCoords(float y = 0)
    {
        var bounds = collider.bounds;
        float scaleX = bounds.size.x / 2;
        float scaleZ = bounds.size.z / 2;
        Vector3 center = transform.position;

        float x = Random.Range(center.x - scaleX, center.x + scaleX);
        float z = Random.Range(center.z - scaleZ, center.z + scaleZ);

        return new Vector3(x, y, z);
    }
}
