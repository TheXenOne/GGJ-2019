using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Gameplay
{
    public class Gameplay : MonoBehaviour
    {
        ILoopState state;

        // Start is called before the first frame update
        void Start()
        {
            ChangeState(new BreakState());
        }

        // Update is called once per frame
        void Update()
        {

        }

        void ChangeState(ILoopState nextState)
        {
            if (state != null)
            {
                state.Exit();
            }
            
            
            state = nextState;

            if (state != null)
            {
                state.Enter();
            }
        }
    }
}