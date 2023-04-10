using System;
using System.Numerics;
using Box2D.NetStandard.Common;

namespace Box2D.NetStandard.Dynamics.World.Callbacks
{
    /// <summary>
    ///  Implement and register this class with a b2World to provide debug drawing of physics
    ///  entities in your game.
    /// </summary>
    public abstract class b2DebugDraw
    {
        protected DrawFlags _drawFlags;

        protected b2DebugDraw() => _drawFlags = 0;

        public DrawFlags Flags
        {
            get => _drawFlags;
            set => _drawFlags = value;
        }

        /// <summary>
        ///  Append flags to the current flags.
        /// </summary>
        public void AppendFlags(DrawFlags flags)
        {
            _drawFlags |= flags;
        }

        /// <summary>
        ///  Clear flags from the current flags.
        /// </summary>
        public void ClearFlags(DrawFlags flags)
        {
            _drawFlags &= ~flags;
        }

        /// <summary>
        ///  Draw a transform. Choose your own length scale.
        /// </summary>
        /// <param name="xf">A transform.</param>
        public abstract void DrawTransform(in b2Transform xf);

        public abstract void DrawPoint(in Vector2 position, float size, in b2Color color);

        /// <summary>
        ///  Draw a closed polygon provided in CCW order.
        /// </summary>
        public abstract void DrawPolygon(in Vector2[] vertices, int vertexCount, in b2Color color);

        /// <summary>
        ///  Draw a solid closed polygon provided in CCW order.
        /// </summary>
        public abstract void DrawSolidPolygon(in Vector2[] vertices, int vertexCount, in b2Color color);

        /// <summary>
        ///  Draw a circle.
        /// </summary>
        public abstract void DrawCircle(in Vector2 center, float radius, in b2Color color);

        /// <summary>
        ///  Draw a solid circle.
        /// </summary>
        public abstract void DrawSolidCircle(in Vector2 center, float radius, in Vector2 axis, in b2Color color);

        /// <summary>
        ///  Draw a line segment.
        /// </summary>
        public abstract void DrawSegment(in Vector2 p1, in Vector2 p2, in b2Color color);
    }
}