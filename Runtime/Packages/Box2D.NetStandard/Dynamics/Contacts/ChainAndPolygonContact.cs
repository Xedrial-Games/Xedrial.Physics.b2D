using Box2D.NetStandard.Collision;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Common;
using Box2D.NetStandard.Dynamics.Fixtures;

namespace Box2D.NetStandard.Dynamics.Contacts
{
    internal class ChainAndPolygonContact : Contact
    {
        private static readonly Collider<EdgeShape, b2PolygonShape> collider = new EdgeAndPolygonCollider();

        private readonly EdgeShape edge;

        public ChainAndPolygonContact(b2Fixture fA, int indexA, b2Fixture fB, int indexB) : base(fA, indexA, fB, indexB)
        {
            var chain = (ChainShape)FixtureA.Shape;
            chain.GetChildEdge(out edge, indexA);
        }

        internal override void Evaluate(out Manifold manifold, in b2Transform xfA, in b2Transform xfB)
        {
            collider.Collide(out manifold, edge, xfA, (b2PolygonShape)FixtureB.Shape, xfB);
        }
    }
}