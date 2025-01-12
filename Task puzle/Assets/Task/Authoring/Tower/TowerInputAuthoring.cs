using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class TowerInputAuthoring : MonoBehaviour
{
    public float rotationSpeed = 5f;

    public class Baker : Baker<TowerInputAuthoring>
    {
        public override void Bake(TowerInputAuthoring input)
        {
            AddComponent<TowerInput>(GetEntity(TransformUsageFlags.Dynamic), new TowerInput
            {
                rotationSpeed = input.rotationSpeed,
            });
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<LocalToWorld>(entity);
        }
    }
}
