using Unity.Entities;

namespace Components
{
    internal struct Turret : IComponentData
    {
        public Entity CannonBallSpawn;
        public Entity CannonBallPrefab;
    }
}
