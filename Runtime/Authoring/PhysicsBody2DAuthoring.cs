using UnityEngine;
using Unity.Entities;
using Box2D.NetStandard.Dynamics.Bodies;

using Xedrial.Physics.b2D.Components;

namespace Xedrial.Physics.b2D.Authoring
{
    [DisallowMultipleComponent]
    public class PhysicsBody2DAuthoring : MonoBehaviour
    {
        [SerializeField] private b2BodyType m_BodyType;

        private class RigidBody2DBaker : Baker<PhysicsBody2DAuthoring>
        {
            public override void Bake(PhysicsBody2DAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.WorldSpace);
                AddComponent(entity, new BodyDef
                {
                    BodyType = authoring.m_BodyType
                });
            }
        }
    }
}
