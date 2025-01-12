using Unity.Entities;
using UnityEngine;

public class SignalEmitterAuthoring : MonoBehaviour
{
    public float EmitFrequency = 1f; 
    public GameObject SignalPrefab;

    /// <summary>
    /// Converts the SignalEmitterAuthoring MonoBehaviour into ECS components: 
    /// SignalEmitter and SignalPrefab. Initializes the SignalEmitter with 
    /// default values and links the provided prefab for signal generation 
    /// during the baking process.
    /// </summary>
    public class Baker : Baker<SignalEmitterAuthoring>
    {
        public override void Bake(SignalEmitterAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new SignalEmitter
            {
                EmitFrequency = authoring.EmitFrequency,
                EmitTimer = 0f
            });

            if (authoring.SignalPrefab == null)
            {
                Debug.LogError("SignalPrefab null! You have to drag SignalPrefab in the Inspector.");
                return;
            }

            if (authoring.SignalPrefab != null)
            {
                var prefabEntity = GetEntity(authoring.SignalPrefab, TransformUsageFlags.Dynamic);
                AddComponent(entity, new SignalPrefab
                {
                    Prefab = prefabEntity
                });
            }
        }
    }
}
