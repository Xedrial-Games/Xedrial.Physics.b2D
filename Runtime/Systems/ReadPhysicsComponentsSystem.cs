using System;

using Unity.Entities;
using Unity.Transforms;

using b2Vec2 = System.Numerics.Vector2;

using Xedrial.Mathematics;
using Xedrial.Physics.b2D.Components;

namespace Xedrial.Physics.b2D.Systems
{
    [UpdateBefore(typeof(PhysicsSystem))]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial class ReadPhysicsComponentsSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var transformLookup = GetComponentLookup<LocalToWorld>(true);
            var velocityLookup = GetComponentLookup<PhysicsVelocity2D>(true);
            var gravityScaleLookup = GetComponentLookup<PhysicsGravityScale2D>(true);

            foreach ((PhysicsBody2D body, Entity entity) in SystemAPI.Query<PhysicsBody2D>().WithEntityAccess())
            {
                if (transformLookup.HasComponent(entity))
                {
                    LocalToWorld transform = transformLookup[entity];

                    var cPos = new b2Vec2(transform.Position.x, transform.Position.y);
                    float angle = xmath.ZRotationFromQuaternion(transform.Rotation);

                    b2Vec2 position = body.RuntimeBody.GetPosition();
                    if (cPos == position || xmath.Equal(angle, body.RuntimeBody.GetAngle()))
                        return;
                    
                    body.RuntimeBody.SetTransform(
                        cPos,
                        angle
                    );
                }

                if (velocityLookup.HasComponent(entity))
                {
                    PhysicsVelocity2D velocity = velocityLookup[entity];
                    
                    body.RuntimeBody.SetLinearVelocity(new b2Vec2(velocity.Linear.x, velocity.Linear.y));
                    body.RuntimeBody.SetAngularVelocity(velocity.Angular);
                }
                
                if (gravityScaleLookup.HasComponent(entity))
                    body.RuntimeBody.SetGravityScale(gravityScaleLookup[entity].Value);
            }
        }
    }
}