using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(StatsManager), typeof(PlayerProgress), typeof(NetworkIdentity))]
public class Player : MonoBehaviour {

    [SerializeField] Character _character;
    [SerializeField] PlayerProgress _progress;
    [SerializeField] Inventory _inventory;
    [SerializeField] Equipment _equipment;

    [SerializeField] StatsManager _statsManager;

    public Character character { get { return _character; } }
    public PlayerProgress progress { get { return _progress; } }
    public Inventory inventory { get { return _inventory; } }
    public Equipment equipment { get { return _equipment; } }

    public void Setup(Character character, Inventory inventory, Equipment equipment, bool isLocalPlayer) {
        _progress = GetComponent<PlayerProgress>();
        _statsManager = GetComponent<StatsManager>();
        _character = character;
        _inventory = inventory;
        _equipment = equipment;
        _character.player = this;
        _inventory.player = this;
        _equipment.player = this;
        _statsManager.player = this;

        if (GetComponent<NetworkIdentity>().isServer) {
            UserAccount account = AccountManager.GetAccount(GetComponent<NetworkIdentity>().connectionToClient);
            _character.stats.Load(account.data);
            _progress.Load(account.data);
            _inventory.Load(account.data);
            _equipment.Load(account.data);
            _character.stats.manager = _statsManager;
            _progress.manager = _statsManager;
        }

        if (isLocalPlayer) {
            InventoryUI.instance.SetInventory(_inventory);
            EquipmentUI.instance.SetEquipment(_equipment);
            StatsUI.instance.SetManager(_statsManager);
        }
    }
}
