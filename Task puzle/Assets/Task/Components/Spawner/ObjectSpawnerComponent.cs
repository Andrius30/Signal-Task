using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct ObjectSpawnerComponent : IComponentData
{
    public Entity absorberPrefab;
    public Entity reflectorPrefab;
    public Entity stateChangerPrefab;

    public int totalObjects;
    public int absorberPercentage;
    public int reflectorPercentage;
    public int stateChangerPercentage;

    public float3 spawnAreaMin;
    public float3 spawnAreaMax;

    public float minDistanceFromTower;
    public float minDistanceBetweenObjects;
}
