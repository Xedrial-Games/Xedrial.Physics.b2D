using UnityEngine;
using Unity.Entities;
using Box2D.NetStandard.Dynamics.Bodies;

using Xedrial.Physics.b2D.Components;

namespace Xedrial.Physics.b2D.Authoring
{
    [DisallowMultipleComponent]
    public class RigidBody2DAuthoring : MonoBehaviour
    {
        [SerializeField] private b2BodyType m_BodyType;

        private class RigidBody2DBaker : Baker<RigidBody2DAuthoring>
        {
            public override void Bake(RigidBody2DAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new BodyDef
                {
                    BodyType = authoring.m_BodyType
                });
            }
        }
    }
}
