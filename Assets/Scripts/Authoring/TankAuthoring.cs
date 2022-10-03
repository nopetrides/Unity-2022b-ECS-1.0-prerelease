using System.Collections;
using System.Collections.Generic;
using Components;
using Unity.Entities;

internal class TankAuthoring : UnityEngine.MonoBehaviour
{
}

internal class TankBaker : Baker<TankAuthoring>
{
    public override void Bake(TankAuthoring authoring)
    {
        AddComponent<Tank>();
    }
}
