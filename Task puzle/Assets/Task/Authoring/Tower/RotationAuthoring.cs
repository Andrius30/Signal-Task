using Unity.Entities;
using UnityEngine;

public class RotationAuthoring : MonoBehaviour
{
    public class Baker : Baker<RotationAuthoring>
    {
        public override void Bake(RotationAuthoring authoring)
        {
            AddComponent<Rotation>(GetEntity(TransformUsageFlags.Dynamic));
        }
    }
}
