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
        Debug.Log("Enemy entered StateClimb.");
        a_entity.isClimbing = false;
        a_entity.useGravity = false;
    }

    public override void Execute(EnemyAI a_entity)
    {
        //bool atTopOfCaravan = IsAtCaravanTop(a_entity);
        //RaycastHit climbPointHit;
        //if (Physics.Raycast(a_entity.climbPoint.transform.position, a_entity.climbPoint.transform.TransformDirection(Vector3.forward), out climbPointHit, a_entity.maxClimbDistance))
        //{
        //    if (climbPointHit.collider.gameObject.tag == "Caravan")
        //    {
        //        Climb(a_entity);
        //    }
        //    Debug.DrawRay(a_entity.climbPoint.transform.position, a_entity.climbPoint.transform.TransformDirection(Vector3.forward) * climbPointHit.distance, Color.red);
        //}

        if (IsAtCaravanTop(a_entity))
        {
            a_entity.enemyStateMachine.ChangeState(StatePullup.Instance);
        }
        else
        {
            Climb(a_entity);
        }
    }

    private bool IsAtCaravanTop(EnemyAI a_entity)
    {
        RaycastHit headPointHit;
        if (Physics.Raycast(a_entity.headPoint.transform.position, a_entity.headPoint.transform.TransformDirection(Vector3.forward), out headPointHit, a_entity.maxClimbDistance))
        {
            Debug.DrawRay(a_entity.headPoint.transform.position, a_entity.headPoint.transform.TransformDirection(Vector3.forward) * headPointHit.distance, Color.red);

            if (headPointHit.collider.gameObject.tag == "Caravan")
            {
                return false;
            }
        }

        Debug.DrawRay(a_entity.headPoint.transform.position, a_entity.headPoint.transform.TransformDirection(Vector3.forward) * a_entity.maxClimbDistance, Color.red);

        return true;
    }

    private void Climb(EnemyAI a_entity)
    {
        if (!a_entity.isClimbing)
        {
            a_entity.velocity = new Vector3(a_entity.velocity.x, a_entity.velocity.y + (a_entity.climbSpeed * Time.deltaTime), a_entity.velocity.z);
            a_entity.isClimbing = true;
        }
    }

    private void StopClimbing(EnemyAI a_entity)
    {
        a_entity.velocity = new Vector3(a_entity.velocity.x, a_entity.velocity.y - (a_entity.climbSpeed *Time.deltaTime), a_entity.velocity.z);
    }

    public override void Exit(EnemyAI a_entity)
    {
        StopClimbing(a_entity);
        a_entity.isClimbing = false;
    }
}