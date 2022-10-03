using System.Collections;
using System.Collections.Generic;
using Components;
using Unity.Entities;

internal class TurretAuthoring : UnityEngine.MonoBehaviour
{
    public UnityEngine.GameObject CannonBallPrefab;
    public UnityEngine.Transform CannonBallSpawn;
}

internal class TurretBaker : Baker<TurretAuthoring>
{
    public override void Bake(TurretAuthoring authoring)
    {
        AddComponent<Turret>(new Turret
        {
            CannonBallPrefab = GetEntity(authoring.CannonBallPrefab),
            CannonBallSpawn = GetEntity(authoring.CannonBallSpawn),
        });

        AddComponent<Shooting>();
    }
}
