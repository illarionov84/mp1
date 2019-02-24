using UnityEngine;
using UnityEngine.Networking;

public class Unit : Interactable {

    [SerializeField] UnitMotor _motor;
    [SerializeField] UnitStats _stats;
    public UnitStats stats { get { return _stats; } }
    public UnitMotor motor { get { return _motor; } }
    public UnitSkills unitSkills;

    protected Interactable _focus;
    public Interactable focus { get { return _focus; } }
    protected float interactDistance;
    protected bool isDie;

    public delegate void UnitDenegate();
    public event UnitDenegate EventOnDamage;
    public event UnitDenegate EventOnDie;
    public event UnitDenegate EventOnRevive;

    public override void OnStartServer() {
        _motor.SetMoveSpeed(_stats.moveSpeed.GetValue());
        _stats.moveSpeed.onStatChanged += _motor.SetMoveSpeed;
    }

    void Update () {
        OnUpdate();
    }

    protected virtual void OnLiveUpdate() { }
    protected virtual void OnDieUpdate() { }

    protected void OnUpdate() {
        if (isServer) {
            if (!isDie) {
                if (_stats.curHealth == 0) Die();
                else OnLiveUpdate();
            } else {
                OnDieUpdate();
            }
        }
    }

    public override float GetInteractDistance(GameObject user) {
        Combat combat = user.GetComponent<Combat>();
        return base.GetInteractDistance(user) + (combat != null ? combat.attackDistance : 0f);
    }

    public override bool Interact(GameObject user) {
        Combat combat = user.GetComponent<Combat>();
        if (combat != null) {
            if (combat.Attack(_stats)) {
                DamageWithCombat(user);
            }
            return true;
        }
        return base.Interact(user);
    }

    public void UseSkill(int skillNum) {
        if (!isDie && skillNum < unitSkills.Count) {
            unitSkills[skillNum].Use(this);
        }
    }

    protected virtual void DamageWithCombat(GameObject user) {
        EventOnDamage();
    }

    public void TakeDamage(GameObject user, int damage) {
        _stats.TakeDamage(damage);
        DamageWithCombat(user);
    }

    public virtual void SetFocus(Interactable newFocus) {
        if (newFocus != _focus) {
            _focus = newFocus;
            interactDistance = _focus.GetInteractDistance(gameObject);
            _motor.FollowTarget(newFocus, interactDistance);
        }
    }

    public virtual void RemoveFocus() {
        _focus = null;
        _motor.StopFollowingTarget();
    }
    
    protected virtual void Die() {
        isDie = true;
        GetComponent<Collider>().enabled = false;
        EventOnDie();
        if (isServer) {
            hasInteract = false;
            RemoveFocus();
            _motor.MoveToPoint(transform.position);
            RpcDie();
        }
    }

    [ClientRpc]
    void RpcDie() {
        if (!isServer) Die();
    }
    
    protected virtual void Revive() {
        isDie = false;
        GetComponent<Collider>().enabled = true;
        EventOnRevive();
        if (isServer) {
            hasInteract = true;
            _stats.SetHealthRate(1);
            RpcRevive();
        }
    }

    [ClientRpc]
    void RpcRevive() {
        if (!isServer) Revive();
    }
}
