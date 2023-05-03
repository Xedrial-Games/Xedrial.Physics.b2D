using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

using Box2D.NetStandard.Dynamics.World;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Fixtures;
using Unity.Collections;
using Xedrial.Mathematics;
using Xedrial.Physics.b2D.Components;

using b2Shape = Box2D.NetStandard.Collision.Shapes.Shape;
using b2Vec2 = System.Numerics.Vector2;
using Quaternion = UnityEngine.Quaternion;

namespace Xedrial.Physics.b2D.Systems
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    public partial class PhysicsSystem : SystemBase
    {
        private readonly ContactListener m_ContactListener = new();

        private DebugDraw m_DebugDraw;
        private bool m_Debug;

        protected override void OnCreate()
        {
            m_ContactListener.ClearTriggerEventsHandler();

            /*m_ContactListener.SetEntityManager(EntityManager);
            
            m_PhysicsWorld.SetContactListener(m_ContactListener);*/
        }

        protected override void OnUpdate()
        {
            float ts = SystemAPI.Time.fixedDeltaTime;

            const int velocityIteration = 6;
            const int positionIteration = 2;
            
            /*b2World physicsWorld = */
            b2World physicsWorld = SystemAPI.ManagedAPI.GetSingleton<PhysicsWorld2D>().Value;

            physicsWorld.Step(ts, velocityIteration, positionIteration);
            if (m_Debug)
                physicsWorld.DrawDebugData();

            var transformLookup = GetComponentLookup<LocalToWorld>(true);
            var velocityLookup = GetComponentLookup<PhysicsVelocity2D>(true);
            var gravityScaleLookup = GetComponentLookup<PhysicsGravityScale2D>(true);

            var ecb = new EntityCommandBuffer(Allocator.TempJob);

            foreach ((var bodyDef, Entity entity) in SystemAPI.Query<RefRO<BodyDef>>().WithEntityAccess())
            {
                var b2BodyDef = new b2BodyDef
                {
                    type = bodyDef.ValueRO.BodyType,
                    userData = entity
                };
            
                if (transformLookup.HasComponent(entity))
                {
                    LocalToWorld transform = transformLookup[entity];
                    b2BodyDef.position = new b2Vec2(transform.Position.x, transform.Position.y);
                    b2BodyDef.angle = xmath.ZRotationFromQuaternion(transform.Value.Rotation());
                }
            
                if (velocityLookup.HasComponent(entity))
                {
                    PhysicsVelocity2D vel = velocityLookup[entity];
                    b2BodyDef.linearVelocity = new b2Vec2(vel.Linear.x, vel.Linear.y);
                    b2BodyDef.angularVelocity = vel.Angular;
                }

                if (gravityScaleLookup.HasComponent(entity))
                {
                    PhysicsGravityScale2D gravityScale = gravityScaleLookup[entity];
                    b2BodyDef.gravityScale = gravityScale.Value;
                }

                b2Body body = physicsWorld.CreateBody(b2BodyDef);
                ecb.AddComponent(entity, new PhysicsBody2D { RuntimeBody = body });
                ecb.RemoveComponent<BodyDef>(entity);
            }
            
            ecb.Playback(EntityManager);
            ecb.Dispose();

            ecb = new EntityCommandBuffer(Allocator.TempJob);
            
            foreach ((var fixtureRO, PhysicsBody2D body, Entity entity) in SystemAPI.Query<RefRO<FixtureDef>, PhysicsBody2D>().WithEntityAccess())
            {
                b2Shape shape = null;
                FixtureDef fixtureDef = fixtureRO.ValueRO;
            
                if (fixtureDef.Shape == Components.Shape.Polygon)
                {
                    b2PolygonShape boxShape = new();
                    boxShape.SetAsBox(fixtureDef.Size.x, fixtureDef.Size.y,
                        new b2Vec2(fixtureDef.Offset.x, fixtureDef.Offset.y), 0.0f);
                    shape = boxShape;
                }
            
                if (shape == null)
                    return;
            
                var b2FixtureDef = new b2FixtureDef
                {
                    shape = shape,
                    density = fixtureDef.Density,
                    friction = fixtureDef.Friction,
                    restitution = fixtureDef.Restitution,
                    isSensor = fixtureDef.IsSensor,
                    userData = entity
                };

                ecb.AddComponent(entity, new PhysicsCollider2D
                {
                    Fixture = body.RuntimeBody.CreateFixture(b2FixtureDef)
                });
                ecb.RemoveComponent<FixtureDef>(entity);
            }
            
            ecb.Playback(EntityManager);
            ecb.Dispose();
        }

        public void DestroyBody(b2Body body)
        {
            b2World physicsWorld = SystemAPI.ManagedAPI.GetSingleton<PhysicsWorld2D>().Value;
            physicsWorld?.DestroyBody(body);
        } 
        /*
        public void SetDebugProperties(Camera camera, Material material, Mesh quadMesh)
        {
            m_DebugDraw = new DebugDraw(camera, material, quadMesh);
            m_DebugDraw.AppendFlags(DrawFlags.Shape | DrawFlags.CenterOfMass);
            m_Debug = true;
            
            m_PhysicsWorld.SetDebugDraw(m_DebugDraw);
        }*/

        public void AddTriggerEventsHandler(ITriggerEventsHandler triggerEventsHandler)
            => m_ContactListener.AddTriggerEventsHandler(triggerEventsHandler);
    }
}
