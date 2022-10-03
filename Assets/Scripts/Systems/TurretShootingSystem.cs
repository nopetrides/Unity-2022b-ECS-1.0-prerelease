using System.Collections;
using System.Collections.Generic;
using Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
partial struct TurretShootingSystem : ISystem
{
     ComponentLookup<LocalToWorldTransform> m_LocalToWorldTransformFromEntity;

     [BurstCompile]
     public void OnCreate(ref SystemState state)
     {
          m_LocalToWorldTransformFromEntity = state.GetComponentLookup<LocalToWorldTransform>(true);
     }
     [BurstCompile]
     public void OnDestroy(ref SystemState state)
     {
     }
     [BurstCompile]
     public void OnUpdate(ref SystemState state)
     {
          m_LocalToWorldTransformFromEntity.Update(ref state);

          var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
          var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

          var turretShootJob = new TurretShoot
          {
               LocalToWorldTransformFromEntity = m_LocalToWorldTransformFromEntity,
               ECB = ecb
          };

          turretShootJob.Schedule();
     }
}

[WithAll(typeof(Shooting))]
[BurstCompile]
partial struct TurretShoot : IJobEntity
{
     [ReadOnly] public ComponentLookup<LocalToWorldTransform> LocalToWorldTransformFromEntity;
     public EntityCommandBuffer ECB;

     void Execute(in TurretAspect turret)
     {
          var instance = ECB.Instantiate(turret.CannonBallPrefab);
          var spawnLocalToWorld = LocalToWorldTransformFromEntity[turret.CannonBallSpawn];
          var cannonBallTransform = UniformScaleTransform.FromPosition(spawnLocalToWorld.Value.Position);

          cannonBallTransform.Scale = LocalToWorldTransformFromEntity[turret.CannonBallPrefab].Value.Scale;
          
          ECB.SetComponent(instance, new LocalToWorldTransform
          {
               Value = cannonBallTransform
          });
          ECB.SetComponent(instance, new CannonBall
          {
               Speed = spawnLocalToWorld.Value.Forward() * 20.0f
          });
          
          // propagate the color from the turret to the cannonball
          ECB.SetComponent(instance, new URPMaterialPropertyBaseColor { Value = turret.Color});
     }

}

