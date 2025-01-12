using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// Responsible for taking inputs from user and rotating tower based on inputs
/// </summary>
public partial class TowerInputSystem : SystemBase
{
    protected override void OnUpdate()
    {
        if (PauseGame.IsPaused) return;
        float deltaTime = SystemAPI.Time.DeltaTime;

        Entities.ForEach((ref Rotation rotation, ref LocalTransform localTransform, in TowerInput input) =>
        {
            if (math.lengthsq(localTransform.Rotation) < 0.99f || math.lengthsq(localTransform.Rotation) > 1.01f)
            {
                localTransform.Rotation = quaternion.identity;
                //Debug.LogWarning("Invalid rotation detected. Resetting to identity.");
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                quaternion deltaRotation = quaternion.AxisAngle(math.up(), math.radians(-90) * input.rotationSpeed * deltaTime);
                localTransform.Rotation = math.mul(localTransform.Rotation, deltaRotation);
                //Debug.Log($"To the left: {localTransform.Rotation}");
            }

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                quaternion deltaRotation = quaternion.AxisAngle(math.up(), math.radians(90) * input.rotationSpeed * deltaTime);
                localTransform.Rotation = math.mul(localTransform.Rotation, deltaRotation);
                //Debug.Log($"To the right: {localTransform.Rotation}");
            }
        }).WithoutBurst().Run();
    }
}
