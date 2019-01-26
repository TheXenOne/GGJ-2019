using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Gameplay
{
    public class Gameplay : MonoBehaviour
    {
        public LoopState m_state;

        public GameObject m_caravan;

        public static Gameplay Instance;
        public static GameObject Caravan => Instance.m_caravan;

        // Start is called before the first frame update
        void Start()
        {
            Instance = this;

            // Set initial state
            ChangeState(GetComponent<BreakState>());
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void ChangeStateWithTravelTo<Type>() where Type: LoopState
        {
            var travel = GetComponent<TravelState>();
            var next = GetComponent<Type>();
            travel.m_nextState = next;

            ChangeState(travel);
        }

        public void ChangeState(LoopState nextState)
        {
            if (m_state != null)
            {
                m_state.enabled = false;
                m_state.Exit();
            }

            Debug.Log("Transitioning from game state " + ((m_state != null) ? m_state.GetType().Name : "(none)") +
                " to " + ((nextState != null) ? nextState.GetType().Name : "(none)"));
            m_state = nextState;

            if (m_state)
            {
                m_state.Enter();
                m_state.enabled = true;
            }
        }
    }
}