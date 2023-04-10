using System.Numerics;
using Box2D.NetStandard.Common;

namespace Box2D.NetStandard.Collision
{
    public struct ShapeCastInput
    {
        public DistanceProxy proxyA;
        public DistanceProxy proxyB;
        public b2Transform transformA;
        public b2Transform transformB;
        public Vector2 translationB;
    }
}