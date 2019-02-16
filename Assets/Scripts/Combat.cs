using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(UnitStats))]
public class Combat : NetworkBehaviour {

    [SerializeField] float attackSpeed = 1f;

    UnitStats myStats;
    float attackCooldown = 0f;

    public delegate void CombatDenegate();
    [SyncEvent] public event CombatDenegate EventOnAttack;

    void Start() {
        myStats = GetComponent<UnitStats>();
	}

    private void Update() {
        if (attackCooldown > 0) attackCooldown -= Time.deltaTime;
    }

    public bool Attack(UnitStats targetStats) {
        if (attackCooldown <= 0) {
            targetStats.TakeDamage(myStats.damage.GetValue());
            EventOnAttack();
            attackCooldown = 1f / attackSpeed;
            return true;
        }
        return false;
    }
}
