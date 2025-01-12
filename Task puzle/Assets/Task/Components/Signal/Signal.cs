using Unity.Entities;
using Unity.Mathematics;

public struct Signal : IComponentData
{
    public float3 Direction;
    public float Speed; 
}
