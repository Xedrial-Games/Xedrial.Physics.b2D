using System.Collections.Generic;
using Box2D.NetStandard.Collision;
using Box2D.NetStandard.Dynamics.Contacts;
using Box2D.NetStandard.Dynamics.World;
using Box2D.NetStandard.Dynamics.World.Callbacks;

using Unity.Entities;

namespace Xedrial.Physics
{
    public struct EntityPair
    {
        public Entity EntityA;
        public Entity EntityB;
    }

    public enum ContactState
    {
        Enter, Exit
    }

    public struct TriggerEvent
    {
        public EntityPair EntityPair;
        public ContactState ContactState;
    }
    
    public interface ITriggerEventsHandler
    {
        public void Execute(TriggerEvent triggerEvent, EntityManager entityManager);
    }
    
    public class ContactListener : b2ContactListener
    {
        private readonly List<ITriggerEventsHandler> m_TriggerEventsHandlers = new();
        private EntityManager m_EntityManager;

        public void AddTriggerEventsHandler(ITriggerEventsHandler triggerEventsHandler)
            => m_TriggerEventsHandlers.Add(triggerEventsHandler);
        
        public void ClearTriggerEventsHandler() => m_TriggerEventsHandlers.Clear();
        
        public void SetEntityManager(EntityManager entityManager) => m_EntityManager = entityManager;
        
        public override void BeginContact(in Contact contact)
        {
            foreach (ITriggerEventsHandler triggerEventsHandler in m_TriggerEventsHandlers)
            {
                if (contact.FixtureA.UserData is not Entity entityA ||
                    contact.FixtureB.UserData is not Entity entityB) continue;
                
                var entityPair = new EntityPair
                {
                    EntityA = entityA,
                    EntityB = entityB
                };

                var triggerEvent = new TriggerEvent
                {
                    EntityPair = entityPair,
                    ContactState = ContactState.Enter
                };
                
                triggerEventsHandler.Execute(triggerEvent, m_EntityManager);
            }
        }

        public override void EndContact(in Contact contact)
        {
            foreach (ITriggerEventsHandler triggerEventsHandler in m_TriggerEventsHandlers)
            {
                if (contact.FixtureA.UserData is not Entity entityA ||
                    contact.FixtureB.UserData is not Entity entityB) continue;
                
                var entityPair = new EntityPair
                {
                    EntityA = entityA,
                    EntityB = entityB
                };

                var triggerEvent = new TriggerEvent
                {
                    EntityPair = entityPair,
                    ContactState = ContactState.Exit
                };
                
                triggerEventsHandler.Execute(triggerEvent, m_EntityManager);
            }
        }

        public override void PreSolve(in Contact contact, in Manifold oldManifold)
        {
        }

        public override void PostSolve(in Contact contact, in ContactImpulse impulse)
        {
        }
    }
}
