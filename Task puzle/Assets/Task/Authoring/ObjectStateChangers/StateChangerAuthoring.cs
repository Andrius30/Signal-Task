using Unity.Entities;
using UnityEngine;

public class StateChangerAuthoring : MonoBehaviour
{
    public class Baker : Baker<StateChangerAuthoring>
    {
        public override void Bake(StateChangerAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new StateChanger { IsOn = false });
        }
    }
}
