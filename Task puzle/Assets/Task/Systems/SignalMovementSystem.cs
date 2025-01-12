using Unity.Entities;
using UnityEngine;
using Unity.Transforms;

/// <summary>
/// Moves signal based on direction and speed
/// </summary>
public partial class SignalMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        if (PauseGame.IsPaused) return;
        float deltaTime = SystemAPI.Time.DeltaTime;

        Entities.ForEach((ref LocalTransform transform, in Signal signal) =>
        {
            transform.Position += signal.Direction * signal.Speed * deltaTime;
        }).ScheduleParallel();
    }
}
