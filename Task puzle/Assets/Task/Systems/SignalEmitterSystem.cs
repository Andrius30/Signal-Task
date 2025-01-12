using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

/// <summary>
/// The SignalEmitterSystem controls the behavior of signal emitters in the game:
/// - Rotates the emitter and emits signals at specified intervals based on its `EmitFrequency`.
/// - Signals are emitted in an arc, evenly distributed within a 90-degree range.
/// - Each signal is assigned a direction and speed, determined by its position in the arc.
/// 
/// Key functionality:
/// 1. Decreases the `EmitTimer` for each signal emitter by the time elapsed since the last update.
/// 2. Emits multiple signals in a 90-degree arc when the timer reaches zero, resetting the timer.
/// 3. Creates and initializes new signal entities, setting their position, rotation, scale, direction, and speed.
/// </summary>
public partial class SignalEmitterSystem : SystemBase
{
    protected override void OnUpdate()
    {
        if (PauseGame.IsPaused) return;
        float deltaTime = SystemAPI.Time.DeltaTime;

        Entities.ForEach((ref SignalEmitter emitter, in LocalToWorld localToWorld, in SignalPrefab signalPrefab) =>
        {
            emitter.EmitTimer -= deltaTime;

            if (emitter.EmitTimer <= 0)
            {
                emitter.EmitTimer = 1f / emitter.EmitFrequency;

                int signalCount = 10;
                float angleStep = math.radians(90f / (signalCount - 1));

                for (int i = 0; i < signalCount; i++)
                {
                    float angle = -math.radians(45f) + (i * angleStep);
                    float3 direction = math.mul(localToWorld.Rotation, math.forward()) +
                                       math.mul(localToWorld.Rotation, new float3(math.sin(angle), 0, math.cos(angle)));

                    Entity signal = EntityManager.Instantiate(signalPrefab.Prefab);

                    EntityManager.SetComponentData(signal, new LocalTransform
                    {
                        Position = localToWorld.Position,
                        Rotation = quaternion.identity,
                        Scale = 1f
                    });

                    EntityManager.SetComponentData(signal, new Signal
                    {
                        Direction = math.normalize(direction),
                        Speed = 5f
                    });
                }
            }
        }).WithStructuralChanges().Run();
    }
}
