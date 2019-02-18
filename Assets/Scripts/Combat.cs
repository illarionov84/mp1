using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(UnitStats))]
public class Combat : NetworkBehaviour {

    [SerializeField] float attackSpeed = 1f;

    public float attackDistance = 0f;

    UnitStats myStats;
    float attackCooldown;

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
