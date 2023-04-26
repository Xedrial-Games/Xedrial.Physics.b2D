using UnityEngine;
using Unity.Entities;

using Box2D.NetStandard.Dynamics.World;
using b2Vec2 = System.Numerics.Vector2;

using Xedrial.Physics.b2D.Components;

namespace Xedrial.Physics.b2D.Authoring
{
    public class PhysicsWorld2DAuthoring : MonoBehaviour
    {
        [SerializeField] private Vector2 m_Gravity = new(0.0f, -9.81f);
        
        public class PhysicsWorld2DBaker : Baker<PhysicsWorld2DAuthoring>
        {
            public override void Bake(PhysicsWorld2DAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponentObject(entity, new PhysicsWorld2D
                {
                    Value = new b2World(new b2Vec2(authoring.m_Gravity.x, authoring.m_Gravity.y))
                });
            }
        }
    }
}