using Unity.Entities;
using UnityEngine;

namespace Xedrial.Physics.b2D.Components
{
    public struct PhysicsGravityScale2D : IComponentData
    {
        public float Value;
    }

    public class PhysicsGravityScale2DAuthoring : MonoBehaviour
    {
        [SerializeField] private float m_Value = 1f;

        public class PhysicsGravityScale2DBaker : Baker<PhysicsGravityScale2DAuthoring>
        {
            public override void Bake(PhysicsGravityScale2DAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new PhysicsGravityScale2D { Value = authoring.m_Value });
            }
        }
    }
}