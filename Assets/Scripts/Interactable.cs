using UnityEngine;
using UnityEngine.Networking;

public class Interactable : NetworkBehaviour {

    public Transform interactionTransform;
    [SerializeField] float radius = 2f;
    
    bool _hasInteract = true;
    public bool hasInteract {
        get { return _hasInteract; }
        set { _hasInteract = value; }
    }

    public virtual float GetInteractDistance(GameObject user) {
        return radius;
    }

    public virtual bool Interact(GameObject user) {
        // override interaction
        return false;
    }

    protected virtual void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }
}
