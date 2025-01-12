using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class SignalAuthoring : MonoBehaviour
{
    public float Speed = 5f;

    /// <summary>
    /// Converts the SignalAuthoring MonoBehaviour into an ECS Signal component
    /// and initializes it with default values during the baking process.
    /// </summary>
    public class Baker : Baker<SignalAuthoring>
    {
        public override void Bake(SignalAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Signal
            {
                Direction = float3.zero, // default value
                Speed = authoring.Speed // default speed
            });
        }
    }
}
