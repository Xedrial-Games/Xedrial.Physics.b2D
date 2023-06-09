using Box2D.NetStandard.Collision;
using Box2D.NetStandard.Collision.Shapes;
using Box2D.NetStandard.Common;
using Box2D.NetStandard.Dynamics.Fixtures;

namespace Box2D.NetStandard.Dynamics.Contacts
{
    internal class ChainAndCircleContact : EdgeAndCircleContact
    {
        public ChainAndCircleContact(b2Fixture fA, int indexA, b2Fixture fB, int indexB) : base(fA, indexA, fB, indexB)
        {
            ((ChainShape)FixtureA.Shape).GetChildEdge(out edgeA, indexA);
        }

        internal override void Evaluate(out Manifold manifold, in b2Transform xfA, in b2Transform xfB)
        {
            base.Evaluate(out manifold, xfA, xfB);
        }
    }
}