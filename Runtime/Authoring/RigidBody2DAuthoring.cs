using UnityEngine;

using Unity.Entities;
using Unity.Mathematics;

using Box2D.NetStandard.Dynamics.Bodies;

namespace Xedrial.Physics.Authoring
{
    [DisallowMultipleComponent]
    public class RigidBody2DAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private b2BodyType m_BodyType;
        [SerializeField] private float2 m_StartLinearVelocity;
        [SerializeField] private float m_GravityScale = 1;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.World.GetOrCreateSystem<PhysicsWorld>().CreatePhysicsBody(entity, new BodyDef
            {
                BodyType = m_BodyType,
                GravityScale = m_GravityScale,
                StartLinearVelocity = m_StartLinearVelocity
            });
        }
    }
}
