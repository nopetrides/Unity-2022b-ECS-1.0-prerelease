using System.Collections;
using System.Collections.Generic;
using Components;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

partial class TankMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var dt = SystemAPI.Time.DeltaTime;
        
        // Entities.ForEach is an older approach to processing queries. Its use is not
        // encouraged, but it remains convenient until we get feature parity with IFE.
        Entities
            .WithAll<Tank>()
            .ForEach((Entity entity, TransformAspect transform) =>
        {
            var pos = transform.Position;
            
            // This does not modify the actual position of the tank, only the point at
            // which we sample the 3D noise function. This way, every tank is using a
            // different slice and will move along its own different random flow field.
            pos.y = entity.Index;
            
            var angle = (0.5f + noise.cnoise(pos / 10f)) * 4.0f * math.PI;

            var dir = float3.zero;
            math.sincos(angle, out dir.x, out dir.z);
            transform.Position += dir * dt * 5.0f;
            transform.Rotation = quaternion.RotateY(angle);
        }).ScheduleParallel();
    }
}
