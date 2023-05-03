using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

using b2Vec2 = System.Numerics.Vector2;

using Xedrial.Physics.b2D.Components;

namespace Xedrial.Physics.b2D.Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystem))]
    public partial class WritePhysicsComponentsSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var transformLookup = GetComponentLookup<LocalToWorld>();
            var velocityFromEntity = GetComponentLookup<PhysicsVelocity2D>();
            var gravityScaleFromEntity = GetComponentLookup<PhysicsGravityScale2D>();
            
            foreach ((PhysicsBody2D body, Entity entity) in SystemAPI.Query<PhysicsBody2D>().WithEntityAccess())
            {
                if (transformLookup.HasComponent(entity))
                {
                    b2Vec2 position = body.RuntimeBody.GetPosition();
                    float angle = body.RuntimeBody.GetAngle();
                    
                    LocalToWorld transform = transformLookup[entity];

                    var translation = new float3(position.X, position.Y, transform.Value.c3.z);
                    var rotation = quaternion.RotateZ(angle);
                    var scale = transform.Value.Scale();
                    
                    transformLookup[entity] = new LocalToWorld
                    {
                        Value = float4x4.TRS(translation, rotation, scale)
                    };
                }

                if (velocityFromEntity.HasComponent(entity))
                {
                    b2Vec2 linear = body.RuntimeBody.GetLinearVelocity();
                    float angular = body.RuntimeBody.GetAngularVelocity();
                    velocityFromEntity[entity] = new PhysicsVelocity2D
                    {
                        Linear = new float2(linear.X, linear.Y),
                        Angular = angular
                    };
                }

                if (gravityScaleFromEntity.HasComponent(entity))
                    gravityScaleFromEntity[entity] = new PhysicsGravityScale2D
                        { Value = body.RuntimeBody.GetGravityScale() };
            }
        }
    }
}