using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;

[BurstCompile]
partial struct TankSpawningSystem : ISystem
{
    // Queries should not be created on the spot in OnUpdate, so they are cached in fields.
    EntityQuery m_BaseColorQuery;
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Config>();

        m_BaseColorQuery = state.GetEntityQuery(ComponentType.ReadOnly<URPMaterialPropertyBaseColor>());
    }
    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Runs once, with a hard coded seed
        // Constant seed makes the behaviour deterministic, just like the noise
        var random = Random.CreateFromIndex(1234);
        var hue = random.NextFloat();
        
        // Helper creates any amount of as distinct colors as possible
        // https://martin.ankerl.com/2009/12/09/how-to-create-random-colors-programmatically/
        URPMaterialPropertyBaseColor RandomColor()
        {
            // 0.618034005f == 2 / (math.sqrt(5) + 1) == inverse of the golden ratio
            hue = (hue + 0.618034005f) % 1;
            var color = UnityEngine.Color.HSVToRGB(hue, 1.0f, 1.0f);
            return new URPMaterialPropertyBaseColor { Value = (UnityEngine.Vector4)color };
        }
        
        
        var config = SystemAPI.GetSingleton<Config>();

        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        var vehicles = CollectionHelper.CreateNativeArray<Entity>(config.TankCount, Allocator.Temp);
        ecb.Instantiate(config.TankPrefab, vehicles);

        // An EntityQueryMask provides an efficient test of whether a specific entity would
        // be selected by an EntityQuery.
        var queryMask = m_BaseColorQuery.GetEntityQueryMask();

        foreach (var vehicle in vehicles)
        {
            // Every prefab root contains a LinkedEntityGroup, a list of all of its entities.
            ecb.SetComponentForLinkedEntityGroup(vehicle,queryMask,RandomColor());
        }

        state.Enabled = false;
    }
}
