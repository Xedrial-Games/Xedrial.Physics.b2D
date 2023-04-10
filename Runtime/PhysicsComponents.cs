using System;

using Unity.Entities;
using Unity.Mathematics;

using Box2D.NetStandard.Dynamics.Bodies;
using Box2D.NetStandard.Dynamics.Fixtures;

namespace Xedrial.Physics
{
    public struct BoxCollider2DComponent : ISharedComponentData, IEquatable<BoxCollider2DComponent>
    {
        /// <summary>
        /// The collider offset from the translation
        /// Default value = { 0.0f, 0.0f }
        /// </summary>
        public float2 Offset;

        /// <summary>
        /// The collider size
        /// Default value = { 0.5f, 0.5f }
        /// </summary>
        public float2 Size;

        // TODO: Move into physics material
        /// <summary>
        /// The collider density
        /// Default value = 1,0f
        /// </summary>
        public float Density;

        /// <summary>
        /// The collider friction
        /// Default value = 0.5f
        /// </summary>
        public float Friction;

        /// <summary>
        /// The collider restitution
        /// Default value = 0.0f
        /// </summary>
        public float Restitution;

        /// <summary>
        /// The collider restitution
        /// Default value = false
        /// </summary>
        public bool IsSensor;

        // Storage for runtime
        public b2Fixture Fixture;

        public bool Equals(BoxCollider2DComponent other)
        {
            return Offset.Equals(other.Offset) && Size.Equals(other.Size) && Density.Equals(other.Density) && Friction.Equals(other.Friction) && Restitution.Equals(other.Restitution) && IsSensor == other.IsSensor && Equals(Fixture, other.Fixture);
        }

        public override bool Equals(object obj)
        {
            return obj is BoxCollider2DComponent other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Offset, Size, Density, Friction, Restitution, IsSensor, Fixture);
        }
    }

    public struct RigidBody2DComponent : ISharedComponentData, IEquatable<RigidBody2DComponent>
    {
        public b2Body Body;
        
        public bool Equals(RigidBody2DComponent other)
        {
            return Equals(Body, other.Body);
        }

        public override bool Equals(object obj)
        {
            return obj is RigidBody2DComponent other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Body);
        }
    }

    public struct BodyDef
    {
        public b2BodyType BodyType;
        public float GravityScale;
        public float2 StartLinearVelocity;
    }
}
