using System.Collections;
using System.Collections.Generic;
using Components;
using Unity.Entities;
using Unity.Rendering;

internal class CannonBallAuthoring : UnityEngine.MonoBehaviour
{
}

internal class CannonBallBaker : Baker<CannonBallAuthoring>
{
    public override void Bake(CannonBallAuthoring authoring)
    {
        AddComponent<CannonBall>();
        AddComponent<URPMaterialPropertyBaseColor>();
    }
}

