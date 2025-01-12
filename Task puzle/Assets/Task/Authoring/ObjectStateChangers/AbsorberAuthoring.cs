using Unity.Entities;
using UnityEngine;

public class AbsorberAuthoring : MonoBehaviour
{
    public class Baker : Baker<AbsorberAuthoring>
    {
        public override void Bake(AbsorberAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Absorber());
        }
    }
}
