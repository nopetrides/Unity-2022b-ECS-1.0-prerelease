using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

// Unmanaged systems based on ISystem can be Burst compiled, but this is not yet the default.
// So we have to explicitly opt into Burst compilation with the [BurstCompile] attribute.
// It has to be added on BOTH the struct AND the OnCreate/OnDestroy/OnUpdate functions to be
// effective.
namespace Systems
{
    [BurstCompile]
    internal partial struct TurretRotationSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var rotation = quaternion.RotateY(SystemAPI.Time.DeltaTime * math.PI);

            foreach (var transform in SystemAPI.Query<TransformAspect>().WithAll<Turret>())
            {
                transform.RotateWorld(rotation);
            }
        }
    }
}