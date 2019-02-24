using UnityEngine;

[RequireComponent(typeof(UnitMotor), typeof(PlayerStats))]
public class Character : Unit {
    
    [SerializeField] float reviveDelay = 5f;
    [SerializeField] GameObject gfx;
    
    Vector3 startPosition;
    float reviveTime;
    public Player player;

    new public PlayerStats stats { get { return base.stats as PlayerStats; } }

    void Start () {
        startPosition = Vector3.zero;
        reviveTime = reviveDelay;

        if (stats.curHealth == 0) {
            transform.position = startPosition;
            if (isServer) {
                stats.SetHealthRate(1);
                motor.MoveToPoint(startPosition);
            }
        }
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
        if (_focus != null) {
            if (!_focus.hasInteract) {
                // если с объектом нельзя больше работать снимаем фокус
                RemoveFocus();
            } else {
                float distance = Vector3.Distance(_focus.interactionTransform.position, transform.position);
                if (distance <= interactDistance) {
                    // действие если цель в зоне взаимодействия
                    if (!_focus.Interact(gameObject)) RemoveFocus();
                }
            }
        }
    }

    protected override void Die() {
        base.Die();
        gfx.SetActive(false);
    }

    protected override void Revive() {
        base.Revive();
        transform.position = startPosition;
        gfx.SetActive(true);
        if (isServer) {
            motor.MoveToPoint(startPosition);
        }
    }

    // функции для управления персонажем при помощи контролера

    public void SetMovePoint(Vector3 point) {
        if (!isDie) {
            RemoveFocus();
            motor.MoveToPoint(point);
        }
    }

    public void SetNewFocus(Interactable newFocus) {
        if (!isDie) {
            if (newFocus.hasInteract) SetFocus(newFocus);
        }
    }
}
