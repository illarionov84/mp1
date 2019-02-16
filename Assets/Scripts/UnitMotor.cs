using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class UnitMotor : MonoBehaviour {
    
    NavMeshAgent agent;
    Transform target;

    void Awake () {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        if (target != null) {
            if (agent.velocity.magnitude == 0) FaceTarget();
            agent.SetDestination(target.position);
        }
    }

    public void MoveToPoint(Vector3 point) {
        agent.SetDestination(point);
    }

    private void FaceTarget() {
        Vector3 direction = target.position - transform.position;
        direction.y = 0;
        if (direction != Vector3.zero) {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    public void FollowTarget(Interactable newTarget) {
        agent.stoppingDistance = newTarget.radius;
        target = newTarget.interactionTransform;
    }

    public void StopFollowingTarget() {
        agent.stoppingDistance = 0f;
        agent.ResetPath();
        target = null;
    }

    public void SetMoveSpeed(int speed) {
        agent.speed = speed;
    }
}
