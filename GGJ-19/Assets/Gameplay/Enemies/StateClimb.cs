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
        a_entity.characterController.enabled = false;
        a_entity.enemyCapsuleCollider.enabled = false;
    }

    public override void Execute(EnemyAI a_entity)
    {
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
        bool drew = false;
        RaycastHit feetPointHit;
        if (Physics.Raycast(a_entity.feetPoint.transform.position, a_entity.feetPoint.transform.TransformDirection(Vector3.forward), out feetPointHit, a_entity.maxClimbDistance))
        {
            Debug.DrawRay(a_entity.feetPoint.transform.position, a_entity.feetPoint.transform.TransformDirection(Vector3.forward) * feetPointHit.distance, Color.red);
            drew = true;

            if (feetPointHit.collider.gameObject.tag == "Caravan")
            {
                return false;
            }
        }

        if (drew == false)
        {
            Debug.DrawRay(a_entity.headPoint.transform.position, a_entity.headPoint.transform.TransformDirection(Vector3.forward) * a_entity.maxClimbDistance, Color.red);
        }

        return true;
    }

    private void Climb(EnemyAI a_entity)
    {
        int layermask = 1 << 9;
        RaycastHit climbPointHit;
        if (Physics.Raycast(a_entity.climbPoint.transform.position, a_entity.climbPoint.transform.TransformDirection(Vector3.forward), out climbPointHit, a_entity.maxClimbDistance, layermask))
        {
            if (climbPointHit.collider.gameObject.tag == "Caravan")
            {
                MeshCollider meshCollider = climbPointHit.collider as MeshCollider;
                if (meshCollider != null && meshCollider.sharedMesh != null)
                {
                    Mesh mesh = meshCollider.sharedMesh;
                    Vector3[] normals = mesh.normals;
                    int[] triangles = mesh.triangles;

                    Vector3 n0 = normals[triangles[climbPointHit.triangleIndex * 3 + 0]];
                    Vector3 n1 = normals[triangles[climbPointHit.triangleIndex * 3 + 1]];
                    Vector3 n2 = normals[triangles[climbPointHit.triangleIndex * 3 + 2]];
                    Vector3 baryCenter = climbPointHit.barycentricCoordinate;
                    Vector3 interpolatedNormal = n0 * baryCenter.x + n1 * baryCenter.y + n2 * baryCenter.z;
                    interpolatedNormal.Normalize();
                    interpolatedNormal = climbPointHit.transform.TransformDirection(interpolatedNormal);
                    //float angle = Vector3.SignedAngle(a_entity.transform.forward, -interpolatedNormal, Vector3.up);
                    //a_entity.transform.Rotate(new Vector3(-angle * Time.deltaTime, 0, 0));
                    float angle = Vector3.SignedAngle(Vector3.up, -interpolatedNormal, Vector3.up);
                    if (angle < (180 - 80))
                    {
                        a_entity.transform.rotation = Quaternion.LookRotation(-interpolatedNormal);
                    }

                    a_entity.transform.localPosition += a_entity.transform.up * a_entity.climbSpeed * Time.deltaTime;
                }
            }
        }
        else
        {
            RaycastHit feetPointHit;
            if (Physics.Raycast(a_entity.feetPoint.transform.position, a_entity.feetPoint.transform.TransformDirection(Vector3.forward), out feetPointHit, a_entity.maxClimbDistance))
            {
                if (feetPointHit.collider.gameObject.tag == "Caravan")
                {
                    MeshCollider meshCollider = feetPointHit.collider as MeshCollider;
                    if (meshCollider != null && meshCollider.sharedMesh != null)
                    {
                        Mesh mesh = meshCollider.sharedMesh;
                        Vector3[] normals = mesh.normals;
                        int[] triangles = mesh.triangles;

                        Vector3 n0 = normals[triangles[feetPointHit.triangleIndex * 3 + 0]];
                        Vector3 n1 = normals[triangles[feetPointHit.triangleIndex * 3 + 1]];
                        Vector3 n2 = normals[triangles[feetPointHit.triangleIndex * 3 + 2]];
                        Vector3 baryCenter = feetPointHit.barycentricCoordinate;
                        Vector3 interpolatedNormal = n0 * baryCenter.x + n1 * baryCenter.y + n2 * baryCenter.z;
                        interpolatedNormal.Normalize();
                        interpolatedNormal = feetPointHit.transform.TransformDirection(interpolatedNormal);
                        //float angle = Vector3.SignedAngle(a_entity.transform.forward, -interpolatedNormal, Vector3.up);
                        //a_entity.transform.Rotate(new Vector3(-angle * Time.deltaTime, 0, 0));
                        float angle = Vector3.SignedAngle(Vector3.up, -interpolatedNormal, Vector3.up);
                        if (angle < (180 - 80))
                        {
                            a_entity.transform.rotation = Quaternion.LookRotation(-interpolatedNormal);
                        }

                        a_entity.transform.localPosition += a_entity.transform.up * a_entity.climbSpeed * Time.deltaTime;
                    }
                }
            }
        }

        if (!a_entity.isClimbing)
        {
            //a_entity.velocity = new Vector3(a_entity.velocity.x, a_entity.velocity.y + (a_entity.climbSpeed * Time.deltaTime), a_entity.velocity.z);
            a_entity.isClimbing = true;
        }
    }

    private void StopClimbing(EnemyAI a_entity)
    {
        a_entity.velocity = new Vector3(a_entity.velocity.x, a_entity.velocity.y - (a_entity.climbSpeed * Time.deltaTime), a_entity.velocity.z);
        Vector3 playerPos = EnemyAI.playerObject.transform.position;
        Vector3 enemyPos = a_entity.transform.position;
        enemyPos.y = playerPos.y;
    }

    public override void Exit(EnemyAI a_entity)
    {
        StopClimbing(a_entity);
        a_entity.isClimbing = false;
    }
}