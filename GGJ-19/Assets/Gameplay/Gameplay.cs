using Assets.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Gameplay
{
    public class Gameplay : MonoBehaviour
    {
        public LoopState m_state;
        public Caravan m_caravan;
        public int m_battlesFought;

        public static Gameplay Instance;
        public static Caravan Caravan => Instance.m_caravan;

        void Awake()
        {
            Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            // Set initial state
            ChangeState(GetComponent<BreakState>());

            Player.Instance.RespawnRandom();
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