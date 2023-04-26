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
            var translationFromEntity = GetComponentLookup<LocalTransform>();
            var velocityFromEntity = GetComponentLookup<PhysicsVelocity2D>();
            var gravityScaleFromEntity = GetComponentLookup<PhysicsGravityScale2D>();
            
            Entities
                .WithoutBurst()
                .ForEach((Entity entity, in PhysicsBody2D body) =>
                {
                    if (translationFromEntity.HasComponent(entity))
                    {
                        b2Vec2 position = body.RuntimeBody.GetPosition();
                        float angle = body.RuntimeBody.GetAngle();

                        LocalTransform transform = translationFromEntity[entity];
                        transform.Position.xy = new float2(position.X, position.Y);
                        transform.Rotation = quaternion.RotateZ(angle);
                        translationFromEntity[entity] = transform;
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
                }).Run();
        }
    }
}