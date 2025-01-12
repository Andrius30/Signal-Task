using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// Handles the spawning of game objects (absorbers, reflectors, and state changers) in the scene and monitors victory conditions.
/// This system ensures proper placement of objects and manages game state transitions based on player interaction with state changers.
/// It integrates with the game UI to trigger victory conditions.
/// </summary>
public partial class ObjectSpawnerSystem : SystemBase
{
    private float onTime = 0f; // Time duration the "On" state is maintained.
    private const float requiredOnTime = 3f; // Required time for achieving victory.

    protected override void OnStartRunning()
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
        var spawnerQuery = SystemAPI.GetSingleton<ObjectSpawnerComponent>();

        int groundLayer = LayerMask.NameToLayer("Ground");
        NativeList<float3> spawnedPositions = new NativeList<float3>(spawnerQuery.totalObjects, Unity.Collections.Allocator.Temp);

        int absorberCount = (int)(spawnerQuery.totalObjects * (spawnerQuery.absorberPercentage / 100f));
        int reflectorCount = (int)(spawnerQuery.totalObjects * (spawnerQuery.reflectorPercentage / 100f));
        int stateChangerCount = spawnerQuery.totalObjects - absorberCount - reflectorCount;

        for (int i = 0; i < spawnerQuery.totalObjects; i++)
        {
            float3 position = GenerateValidPosition(
                spawnerQuery.spawnAreaMin,
                spawnerQuery.spawnAreaMax,
                spawnerQuery.minDistanceFromTower,
                spawnerQuery.minDistanceBetweenObjects,
                groundLayer,
                spawnedPositions
            );

            if (position.Equals(float3.zero))
            {
                Debug.LogWarning("Failed to find a valid position for an object.");
                continue;
            }

            Entity prefabToSpawn;
            if (i < absorberCount)
            {
                prefabToSpawn = spawnerQuery.absorberPrefab;
            }
            else if (i < absorberCount + reflectorCount)
            {
                prefabToSpawn = spawnerQuery.reflectorPrefab;
            }
            else
            {
                prefabToSpawn = spawnerQuery.stateChangerPrefab;
            }

            Entity spawnedEntity = ecb.Instantiate(prefabToSpawn);
            ecb.SetComponent(spawnedEntity, new LocalTransform
            {
                Position = position,
                Rotation = quaternion.identity,
                Scale = 4f
            });

            spawnedPositions.Add(position);
        }

        ecb.Playback(EntityManager);
        ecb.Dispose();
        spawnedPositions.Dispose();
    }

    protected override void OnUpdate()
    {
        if (PauseGame.IsPaused) return;
        int onCount = 0;

        foreach (var stateChanger in SystemAPI.Query<RefRO<StateChanger>>())
        {
            if (stateChanger.ValueRO.IsOn)
            {
                onCount++;
            }
        }

        //Debug.Log($"Current 'On' Count: {onCount}");

        if (onCount >= 3)
        {
            onTime += SystemAPI.Time.DeltaTime;

            if (onTime >= requiredOnTime)
            {
                //Debug.Log("Victory! All required objects stayed 'On' for the specified time.");
                PauseGame.IsPaused = true;
                onTime = 0f;
                UIManager.onVictory.Invoke(true);
            }
        }
        else
        {
            onTime = 0f;
        }
    }

    /// <summary>
    /// Generates a valid position on the ground within the specified area, ensuring no overlap.
    /// </summary>
    private float3 GenerateValidPosition(
        float3 areaMin,
        float3 areaMax,
        float minDistanceFromTower,
        float minDistanceBetweenObjects,
        int groundLayer,
        NativeList<float3> existingPositions)
    {
        for (int attempts = 0; attempts < 10; attempts++)
        {
            float3 randomPosition = new float3(
                UnityEngine.Random.Range(areaMin.x, areaMax.x),
                50f,
                UnityEngine.Random.Range(areaMin.z, areaMax.z)
            );

            Ray ray = new Ray(new Vector3(randomPosition.x, randomPosition.y, randomPosition.z), Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, 1 << groundLayer))
            {
                float3 hitPosition = new float3(hit.point.x, hit.point.y, hit.point.z);

                if (math.distance(hitPosition, float3.zero) < minDistanceFromTower)
                    continue;

                bool isTooClose = false;
                foreach (float3 existingPosition in existingPositions)
                {
                    if (math.distance(hitPosition, existingPosition) < minDistanceBetweenObjects)
                    {
                        isTooClose = true;
                        break;
                    }
                }

                if (!isTooClose)
                    return hitPosition;
            }
        }

        return float3.zero;
    }
}
