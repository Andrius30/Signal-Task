using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Rendering;

/// <summary>
/// Manages interactions between signals and different game objects:
/// - Absorbers destroy signals within a defined radius.
/// - Reflectors bounce signals, altering their direction while keeping them in the x-z plane.
/// - State changers toggle their state and update their visual color when hit by a signal.
/// </summary>
public partial class SignalInteractionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float interactionRadius = 5f;

        EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp); // A special object that collects commands to modify entities or components in the ECS world.

        foreach (var (signal, signalTransform, entity) in SystemAPI.Query<RefRW<Signal>, RefRO<LocalTransform>>().WithEntityAccess())
        {
            float3 signalPosition = signalTransform.ValueRO.Position;
            var signalData = signal.ValueRW;

            // absorb
            foreach (var absorberTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Absorber>()) // .WithAll<Absorber>() The .WithAll<>() method in Unity ECS/DOTS specifies an additional filter for the entities to include in the query. It ensures that only entities containing specific components (in this case, Absorber) are included in the query.
            {
                var distance = math.distance(signalPosition, absorberTransform.ValueRO.Position);
                if (distance < interactionRadius)
                {
                    //Debug.Log("Signalas sugertas absorberyje!");
                    ecb.DestroyEntity(entity);
                    break;
                }
            }

            // Reflect signal in the same plane (x-z level)
            foreach (var reflectorTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Reflector>())
            {
                if (math.distance(signalPosition, reflectorTransform.ValueRO.Position) < interactionRadius)
                {
                    // Calculate normal in x-z plane
                    float3 normal = reflectorTransform.ValueRO.Position - signalPosition;
                    normal.y = 0; // Ignore vertical component
                    normal = math.normalize(normal);

                    // Reflect signal direction
                    signalData.Direction = math.reflect(signalData.Direction, normal);

                    // Update signal
                    signal.ValueRW = signalData;
                }
            }

            // change state and color based on state
            foreach (var (stateChanger, stateChangerTransform, stateChangerEntity) in SystemAPI.Query<RefRW<StateChanger>, RefRO<LocalTransform>>().WithEntityAccess())
            {
                if (math.distance(signalPosition, stateChangerTransform.ValueRO.Position) < interactionRadius)
                {
                    //Debug.Log("Signalas pakeite state!");
                    stateChanger.ValueRW.IsOn = !stateChanger.ValueRW.IsOn;
                    if (!EntityManager.HasComponent<URPMaterialPropertyBaseColor>(stateChangerEntity))
                    {
                        ecb.AddComponent(stateChangerEntity, new URPMaterialPropertyBaseColor
                        {
                            Value = stateChanger.ValueRW.IsOn ? new float4(0, 1, 0, 1) : new float4(1, 0, 0, 1)
                        });
                    }
                    else
                    {
                        ecb.SetComponent(stateChangerEntity, new URPMaterialPropertyBaseColor
                        {
                            Value = stateChanger.ValueRW.IsOn ? new float4(0, 1, 0, 1) : new float4(1, 0, 0, 1)
                        });
                    }
                }
            }
        }

        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}
