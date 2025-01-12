using Unity.Entities;
using UnityEngine;

public class ObjectSpawnerAuthoring : MonoBehaviour
{
    public GameObject absorberPrefab;
    public GameObject reflectorPrefab;
    public GameObject stateChangerPrefab;

    public int totalObjects = 20;
    [Range(0, 100)] public int absorberPercentage = 33;
    [Range(0, 100)] public int reflectorPercentage = 33;
    [Range(0, 100)] public int stateChangerPercentage = 34;
    public float minDistanceFromTower = 5f;
    public float minDistanceBetweenObjects = 2f;

    public Vector3 SpawnAreaMin = new Vector3(-10f, 0f, -10f);
    public Vector3 SpawnAreaMax = new Vector3(10f, 0f, 10f);

    public class Baker : Baker<ObjectSpawnerAuthoring>
    {
        public override void Bake(ObjectSpawnerAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new ObjectSpawnerComponent
            {
                absorberPrefab = GetEntity(authoring.absorberPrefab, TransformUsageFlags.Dynamic),
                reflectorPrefab = GetEntity(authoring.reflectorPrefab, TransformUsageFlags.Dynamic),
                stateChangerPrefab = GetEntity(authoring.stateChangerPrefab, TransformUsageFlags.Dynamic),
                totalObjects = authoring.totalObjects,
                absorberPercentage = authoring.absorberPercentage,
                reflectorPercentage = authoring.reflectorPercentage,
                stateChangerPercentage = authoring.stateChangerPercentage,
                spawnAreaMin = authoring.SpawnAreaMin,
                spawnAreaMax = authoring.SpawnAreaMax,
                minDistanceFromTower = authoring.minDistanceFromTower,
                minDistanceBetweenObjects = authoring.minDistanceBetweenObjects
            });
        }
    }
}
