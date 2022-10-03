using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

struct Config : IComponentData
{
    public Entity TankPrefab;
    public int TankCount;
    public float SafeZoneRadius;
}
