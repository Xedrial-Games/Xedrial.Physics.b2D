using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Xedrial.Physics.b2D.Components;
using b2Vec2 = System.Numerics.Vector2;

namespace Xedrial.Physics.b2D.Systems
{
    [UpdateBefore(typeof(PhysicsSystem))]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial class ReadPhysicsComponentsSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var transformLookup = GetComponentLookup<LocalTransform>(true);
            var velocityFromEntity = GetComponentLookup<PhysicsVelocity2D>(true);
            var gravityScaleFromEntity = GetComponentLookup<PhysicsGravityScale2D>(true);
            
            Entities
                .WithoutBurst()
                .WithReadOnly(transformLookup)
                .WithReadOnly(velocityFromEntity)
                .WithReadOnly(gravityScaleFromEntity)
                .ForEach((Entity entity, in PhysicsBody2D body) =>
                {
                    if (transformLookup.HasComponent(entity))
                    {
                        LocalTransform transform = transformLookup[entity];

                        var cPos = new b2Vec2(transform.Position.x, transform.Position.y);
                        float angle = math.radians(((Quaternion)transform.Rotation).eulerAngles.z);

                        b2Vec2 position = body.RuntimeBody.GetPosition();
                        if (cPos == position || Math.Abs(angle - body.RuntimeBody.GetAngle()) < 0f)
                            return;
                        
                        body.RuntimeBody.SetTransform(
                            cPos,
                            angle
                        );
                    }

                    if (velocityFromEntity.HasComponent(entity))
                    {
                        PhysicsVelocity2D velocity = velocityFromEntity[entity];
                        
                        body.RuntimeBody.SetLinearVelocity(new b2Vec2(velocity.Linear.x, velocity.Linear.y));
                        body.RuntimeBody.SetAngularVelocity(velocity.Angular);
                    }
                    
                    if (gravityScaleFromEntity.HasComponent(entity))
                        body.RuntimeBody.SetGravityScale(gravityScaleFromEntity[entity].Value);
                }).Run();
        }
    }
}