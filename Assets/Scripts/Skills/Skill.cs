using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Skill : NetworkBehaviour {

    public Sprite icon;

    [SerializeField] float castTime = 1f;
    [SerializeField] float cooldown = 1f;

    [HideInInspector] public float castDelay, cooldownDelay;
    protected Unit unit;
    protected Interactable target;

    List<Skill> _skills;
    int _skillIndex;

    protected virtual void Start() {
        _skills = new List<Skill>(GetComponents<Skill>());
        _skillIndex = _skills.FindIndex(x => x == this);
    }

    void Update () {
        if (castDelay > 0) {
            castDelay -= Time.deltaTime;
            if (castDelay <= 0) {
                castDelay = 0;
                if (isServer) OnCastComplete();
            }
        }
        if (cooldownDelay > 0) {
            cooldownDelay -= Time.deltaTime;
            if (cooldownDelay <= 0) {
                cooldownDelay = 0;
                if (isServer) OnCooldownComplete();
            }
        }
    }


    public void Use(Unit unit) {
        if (castDelay == 0 && cooldownDelay == 0) {
            this.unit = unit;
            target = unit.focus;
            OnUse();
        }
    }

	/// <summary>
	/// вызываемый при успешном использовании навыка
	/// </summary>
	protected virtual void OnUse() {
        if (isServer) {
            RpcOnUse(_skillIndex, unit.gameObject, target != null ? target.gameObject : null);
            if (castTime > 0) castDelay = castTime;
            else OnCastComplete();
        } else {
            if (castTime > 0) castDelay = castTime;
        }
    }

	/// <summary>
	/// вызываемый после окончания прочтения навыка
	/// </summary>
	protected virtual void OnCastComplete() {
        if (isServer) {
            RpcOnCastComplete(_skillIndex);
            if (cooldown > 0) cooldownDelay = cooldown;
            else OnCooldownComplete();
        } else {
            if (cooldown > 0) cooldownDelay = cooldown;
        }
    }

	/// <summary>
	/// вызываемый, когда закончится задержка навыка после срабатывания
	/// </summary>
	protected virtual void OnCooldownComplete() {
        if (isServer) RpcOnCooldownComplete(_skillIndex);
    }

    [ClientRpc]
    void RpcOnUse(int skillIndex, GameObject unitGo, GameObject targetGo) {
        _skills[skillIndex].unit = unitGo.GetComponent<Unit>();
        if (targetGo != null) _skills[skillIndex].target = targetGo.GetComponent<Interactable>();
        _skills[skillIndex].OnUse();
    }

    [ClientRpc]
    void RpcOnCastComplete(int skillIndex) {
        _skills[skillIndex].OnCastComplete();
    }

    [ClientRpc]
    void RpcOnCooldownComplete(int skillIndex) {
        _skills[skillIndex].OnCooldownComplete();
    }
}
