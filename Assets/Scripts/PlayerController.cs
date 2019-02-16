using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class PlayerController : NetworkBehaviour {

    [SerializeField] LayerMask movementMask;

    Character character;
    Camera cam;

    private void Awake() {
        cam = Camera.main;
    }

    public void SetCharacter(Character character, bool isLocalPlayer) {
        this.character = character;
        if (isLocalPlayer) cam.GetComponent<CameraController>().target = character.transform;
    }

    private void Update() {
        if (isLocalPlayer) {
            if (character != null && !EventSystem.current.IsPointerOverGameObject()) {
                // при нажатии на правую кнопку мыши пересещаемся в указанную точку
                if (Input.GetMouseButtonDown(1)) {
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 100f, movementMask)) {
                        CmdSetMovePoint(hit.point);
                    }
                }
                // при нажатии на левую кнопку мыши взаимодйствуем с объектами
                if (Input.GetMouseButtonDown(0)) {
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 100f, ~(1 << LayerMask.NameToLayer("Player")))) {
                        Interactable interactable = hit.collider.GetComponent<Interactable>();
                        if (interactable != null) {
                            CmdSetFocus(interactable.GetComponent<NetworkIdentity>());
                        }
                    }
                }
            }
        }
    }

    [Command]
    public void CmdSetMovePoint(Vector3 point) {
        character.SetMovePoint(point);
    }

    [Command]
    public void CmdSetFocus(NetworkIdentity newFocus) {
        character.SetNewFocus(newFocus.GetComponent<Interactable>());
    }
}
