using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    internal struct CannonBall : IComponentData
    {
        public float3 Speed;
    }
}
