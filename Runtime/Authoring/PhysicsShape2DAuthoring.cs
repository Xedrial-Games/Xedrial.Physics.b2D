using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

using Xedrial.Physics.b2D.Components;

namespace Xedrial.Physics.b2D.Authoring
{
    [DisallowMultipleComponent]
    public class PhysicsShape2DAuthoring : MonoBehaviour
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

        private Mesh m_QuadMesh;

        private void OnDrawGizmosSelected()
        {
            if (m_QuadMesh == null)
                CreateMesh();
            
            Gizmos.color = Color.green;
            Transform transform1 = transform;
            Vector3 size = new float3(m_Size * (float2)(Vector2)transform1.localScale * 2, 0);
            Gizmos.DrawWireMesh(m_QuadMesh, transform1.position, transform1.rotation, size);
            Gizmos.color = Color.white;
        }

        private void CreateMesh()
        {
            m_QuadMesh = new Mesh
            {
                vertices = new[]
                {
                    new Vector3(-0.5f, -0.5f),
                    new Vector3(-0.5f, 0.5f),
                    new Vector3(0.5f, 0.5f),
                    new Vector3(0.5f, -0.5f)
                },
                triangles = new[] { 0, 1, 2, 2, 3, 0 }
            };
            
            m_QuadMesh.RecalculateNormals();
        }

        private class BoxCollider2DBaker : Baker<PhysicsShape2DAuthoring>
        {
            public override void Bake(PhysicsShape2DAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new FixtureDef
                {
                    Offset = authoring.m_Offset,
                    Density = authoring.m_Density,
                    Friction = authoring.m_Friction,
                    Restitution = authoring.m_Restitution,
                    IsSensor = authoring.m_IsSensor,
                    Size = authoring.m_Size * (float2)(Vector2)authoring.transform.lossyScale,
                    Shape = Shape.Polygon
                });
            }
        }
    }
}
