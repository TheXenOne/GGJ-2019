using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateClimb : State<EnemyAI>
{
    private static StateClimb instance;

    private StateClimb() { }

    public static StateClimb Instance {
        get {
            if (instance == null)
            {
                instance = new StateClimb();
            }
            return instance;
        }
    }

    public override void Enter(EnemyAI a_entity)
    {
        Debug.Log("Entered StateClimb.");
    }

    public override void Execute(EnemyAI a_entity)
    {
        
    }

    public override void Exit(EnemyAI a_entity)
    {
    }
}