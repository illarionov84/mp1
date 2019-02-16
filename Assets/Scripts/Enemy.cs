using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitMotor), typeof(EnemyStats))]
public class Enemy : Unit {

    [Header("Movement")]
    [SerializeField] float moveRadius = 10f;
    [SerializeField] float minMoveDelay = 4f;
    [SerializeField] float maxMoveDelay = 12f;
    Vector3 startPosition;
    Vector3 curDistanation;
    float changePosTime;

    [Header("Behavior")]
    [SerializeField] bool aggressive;
    [SerializeField] float rewardExp;
    [SerializeField] float viewDistance = 5f;
    [SerializeField] float reviveDelay = 5f;

    float reviveTime;
    List<Character> enemies = new List<Character>();

    void Start () {
        startPosition = transform.position;
        changePosTime = Random.Range(minMoveDelay, maxMoveDelay);
        reviveTime = reviveDelay;
    }

    void Update() {
        OnUpdate();
    }

    protected override void OnDieUpdate() {
        base.OnDieUpdate();
        if (reviveTime > 0) {
            reviveTime -= Time.deltaTime;
        } else {
            reviveTime = reviveDelay;
            Revive();
        }
    }

    protected override void OnLiveUpdate() {
        base.OnLiveUpdate();
        if (focus == null) {
            // блуждание
            Wandering(Time.deltaTime);
            // поиск цели если монстр агресивный
            if (aggressive) FindEnemy();
        } else {
            float distance = Vector3.Distance(focus.interactionTransform.position, transform.position);
            if (distance > viewDistance || !focus.hasInteract) {
                // если цель далеко перестаём приследовать
                RemoveFocus();
            } else if (distance <= focus.radius) {
                // действие если цель взоне взаимодействия
                if (!focus.Interact(gameObject)) RemoveFocus();
            }
        }
    }
    
    protected override void Revive() {
        base.Revive();
        transform.position = startPosition;
        if (isServer) {
            motor.MoveToPoint(startPosition);
        }
    }

    protected override void Die() {
        base.Die();
        if (isServer) {
            for (int i = 0; i < enemies.Count; i++) {
                enemies[i].player.progress.AddExp(rewardExp / enemies.Count);
            }
            enemies.Clear();
        }
    }

    void FindEnemy() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, viewDistance, 1 << LayerMask.NameToLayer("Player"));
        for (int i = 0; i < colliders.Length; i++) {
            Interactable interactable = colliders[i].GetComponent<Interactable>();
            if (interactable != null && interactable.hasInteract) {
                SetFocus(interactable);
                break;
            }
        }
    }

    void Wandering(float deltaTime) {
        changePosTime -= deltaTime;
        if (changePosTime <= 0) {
            RandomMove();
            changePosTime = Random.Range(minMoveDelay, maxMoveDelay);
        }
    }

    void RandomMove() {
        curDistanation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up) * new Vector3(moveRadius, 0, 0) + startPosition;
        motor.MoveToPoint(curDistanation);
    }

    public override bool Interact(GameObject user) {
        if (base.Interact(user)) {
            SetFocus(user.GetComponent<Interactable>());
            return true;
        }
        return false;
    }

    protected override void DamageWithCombat(GameObject user) {
        base.DamageWithCombat(user);
        Character character = user.GetComponent<Character>();
        if (character != null && !enemies.Contains(character)) enemies.Add(character); 
    }

    protected override void OnDrawGizmosSelected() {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }
}
