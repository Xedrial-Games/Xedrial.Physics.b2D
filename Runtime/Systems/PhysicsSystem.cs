using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

using Box2D.NetStandard.Dynamics.World;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.Fixtures;

using Xedrial.Physics.b2D.Components;

using b2Shape = Box2D.NetStandard.Collision.Shapes.Shape;
using b2Vec2 = System.Numerics.Vector2;

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

            RequireForUpdate<PhysicsWorld2D>();
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

            Entities
                .WithStructuralChanges()
                .WithoutBurst()
                .ForEach((Entity entity, in BodyDef bodyDef, in FixtureDef fixtureDef) =>
                {
                    CreatePhysicsBody(entity, bodyDef, fixtureDef, physicsWorld);
                    
                    EntityManager.RemoveComponent<BodyDef>(entity);
                    EntityManager.RemoveComponent<FixtureDef>(entity);
                }).Run();
        }

        private void CreatePhysicsBody(Entity entity, BodyDef bodyDef, FixtureDef fixtureDef, b2World world)
        {
            b2Body body = CreateB2Body(bodyDef, entity, world);
            EntityManager.AddComponentData(entity, 
                new PhysicsBody2D { RuntimeBody = body });

            b2Shape shape = null;
            
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

            EntityManager.AddComponentData(entity, new PhysicsCollider2D
            {
                Fixture = body.CreateFixture(b2FixtureDef)
            });
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

        private b2Body CreateB2Body(BodyDef bodyDef, Entity entity, b2World world)
        {
            var b2BodyDef = new b2BodyDef
            {
                type = bodyDef.BodyType
            };
            
            if (EntityManager.HasComponent<LocalTransform>(entity))
            {
                var transform = EntityManager.GetComponentData<LocalTransform>(entity);
                b2BodyDef.position = new b2Vec2(transform.Position.x, transform.Position.y);
                b2BodyDef.angle = math.radians(((Quaternion)transform.Rotation).eulerAngles.z);
            }
            
            if (EntityManager.HasComponent<PhysicsVelocity2D>(entity))
            {
                var vel = EntityManager.GetComponentData<PhysicsVelocity2D>(entity);
                b2BodyDef.linearVelocity = new b2Vec2(vel.Linear.x, vel.Linear.y);
                b2BodyDef.angularVelocity = vel.Angular;
            }

            if (EntityManager.HasComponent<PhysicsGravityScale2D>(entity))
            {
                var gravityScale = EntityManager.GetComponentData<PhysicsGravityScale2D>(entity);
                b2BodyDef.gravityScale = gravityScale.Value;
            }

            b2Body body = world.CreateBody(b2BodyDef);
            return body;
        }
    }
}
