using Unity.Entities;
using UnityEngine;

public class ReflectorAuthoring : MonoBehaviour
{
    public class Baker : Baker<ReflectorAuthoring>
    {
        public override void Bake(ReflectorAuthoring authoring)
        {
            AddComponent<Reflector>(GetEntity(TransformUsageFlags.Dynamic));
        }
    }
}