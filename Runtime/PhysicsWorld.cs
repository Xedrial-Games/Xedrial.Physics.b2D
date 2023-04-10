using b2Vec2 = System.Numerics.Vector2;

using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

using Quaternion = UnityEngine.Quaternion;

using Box2D.NetStandard.Dynamics.World;
using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Dynamics.World.Callbacks;

namespace Xedrial.Physics
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial class PhysicsWorld : SystemBase
    {
        private b2World m_PhysicsWorld;
        private readonly ContactListener m_ContactListener = new();
        
        public bool Debug;
        public Debug.DebugDraw DebugDraw;

        public void SetDebugProperties(Camera camera, Material material, Mesh quadMesh)
        {
            DebugDraw = new Debug.DebugDraw(camera, material, quadMesh);
            DebugDraw.AppendFlags(DrawFlags.Shape | DrawFlags.CenterOfMass);
            Debug = true;
        }

        public void AddTriggerEventsHandler(ITriggerEventsHandler triggerEventsHandler)
            => m_ContactListener.AddTriggerEventsHandler(triggerEventsHandler);

        protected override void OnCreate()
        {
            m_ContactListener.ClearTriggerEventsHandler();
            m_PhysicsWorld = new b2World(new b2Vec2(0.0f, -10f));
        }

        protected override void OnStartRunning()
        {
            OnPhysicsWorldStart();
        }

        public void OnPhysicsWorldStart()
        {
            m_PhysicsWorld.SetDebugDraw(DebugDraw);
            m_ContactListener.SetEntityManager(EntityManager);
            
            m_PhysicsWorld.SetContactListener(m_ContactListener);
        }

        public b2Body CreatePhysicsBody(Entity entity, BodyDef bodyDef)
        {
            b2Body body = CreateB2Body(bodyDef, entity, EntityManager);
            EntityManager.AddSharedComponentData(entity, new RigidBody2DComponent { Body = body });

            if (EntityManager.HasComponent<BoxCollider2DComponent>(entity))
            {
                var bc2d = EntityManager.GetSharedComponentData<BoxCollider2DComponent>(entity);
                float3 scale = EntityManager.HasComponent<NonUniformScale>(entity) ?
                    EntityManager.GetComponentData<NonUniformScale>(entity).Value : new float3(1);
                    
                b2PolygonShape boxShape = new();
                boxShape.SetAsBox(bc2d.Size.x * scale.x, bc2d.Size.y * scale.y,
                    new b2Vec2(bc2d.Offset.x, bc2d.Offset.y), 0.0f);
                    
                var fixtureDef = new b2FixtureDef
                {
                    shape = boxShape,
                    density = bc2d.Density,
                    friction = bc2d.Friction,
                    restitution = bc2d.Restitution,
                    isSensor = bc2d.IsSensor,
                    userData = entity
                };

                bc2d.Fixture = body.CreateFixture(fixtureDef);
                EntityManager.SetSharedComponentData(entity, bc2d);
            }

            return body;
        }

        public void DestroyBody(b2Body body) => m_PhysicsWorld.DestroyBody(body);

        protected override void OnUpdate()
        {
            float ts = Time.DeltaTime;

            const int velocityIteration = 6;
            const int positionIteration = 2;

            m_PhysicsWorld.Step(ts, velocityIteration, positionIteration);
            if (Debug)
                m_PhysicsWorld.DrawDebugData();
            
            Entities.WithStructuralChanges().ForEach((ref Translation translation, ref Rotation rotation, in RigidBody2DComponent rigidBody) =>
            {
                if (rigidBody.Body == null)
                    return;

                b2Vec2 position = rigidBody.Body.GetPosition();
                float angle = rigidBody.Body.GetAngle();
                translation.Value.xy = new float2(position.X, position.Y);
                rotation.Value = quaternion.AxisAngle(math.forward(), angle);
            }).WithoutBurst().Run();
        }

        private b2Body CreateB2Body(BodyDef bodyDef, Entity entity, EntityManager entityManager)
        {
            float3 position = entityManager.HasComponent<Translation>(entity) ?
                entityManager.GetComponentData<Translation>(entity).Value : float3.zero;
            
            float4 rotation = entityManager.HasComponent<Rotation>(entity) ?
                entityManager.GetComponentData<Rotation>(entity).Value.value : float4.zero;
            
            var b2BodyDef = new b2BodyDef
            {
                type = bodyDef.BodyType,
                position = new b2Vec2(position.x, position.y),
                angle = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w).eulerAngles.z,
                gravityScale = bodyDef.GravityScale,
                linearVelocity = new b2Vec2(bodyDef.StartLinearVelocity.x, bodyDef.StartLinearVelocity.y)
            };

            b2Body body = m_PhysicsWorld.CreateBody(b2BodyDef);
            return body;
        }
    }
}
