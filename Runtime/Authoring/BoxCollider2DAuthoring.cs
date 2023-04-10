using System;
using Unity.Entities;
using Unity.Mathematics;

using UnityEngine;

namespace Xedrial.Physics.Authoring
{
    [DisallowMultipleComponent]
    public class BoxCollider2DAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        /// <summary>
        /// The collider offset from the translation
        /// </summary>
        public float2 m_Offset;

        /// <summary>
        /// The collider size
        /// </summary>
        public float2 m_Size = new(0.5f, 0.5f);

        // TODO: Move into physics material
        /// <summary>
        /// The collider density
        /// </summary>
        public float m_Density = 1.0f;

        /// <summary>
        /// The collider friction
        /// </summary>
        public float m_Friction = 0.5f;

        /// <summary>
        /// The collider restitution
        /// </summary>
        public float m_Restitution;

        /// <summary>
        /// The collider restitution
        /// </summary>
        public bool m_IsSensor;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem _)
        {
            dstManager.AddSharedComponentData(entity, new BoxCollider2DComponent
            {
                Offset = m_Offset,
                Size = m_Size,
                Density = m_Density,
                Friction = m_Friction,
                Restitution = m_Restitution,
                IsSensor = m_IsSensor
            });
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Transform transform1 = transform;
            Vector3 size = new float3(m_Size * (float2)(Vector2)transform1.localScale * 2, 0);
            Gizmos.DrawWireCube(transform1.position, size);
            Gizmos.color = Color.white;
        }
    }
}
