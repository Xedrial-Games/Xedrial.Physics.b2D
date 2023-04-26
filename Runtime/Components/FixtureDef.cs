using Unity.Entities;
using Unity.Mathematics;

namespace Xedrial.Physics.b2D.Components
{
    public enum Shape : byte
    {
        Circle, Polygon
    }

    public struct FixtureDef : IComponentData
    {
        /// <summary>
        /// The shape of the fixture needed
        /// </summary>
        public Shape Shape;
        
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
    }
}