﻿using UnityEngine;
using UnityEngine.Networking;

public class NetUnitSetup : NetworkBehaviour {

    [SerializeField] MonoBehaviour[] disableBehaviours;

	void Awake() {
        for (int i = 0; i < disableBehaviours.Length; i++) {
            disableBehaviours[i].enabled = false;
        }
	}
    public override void OnStartServer() {
        for (int i = 0; i < disableBehaviours.Length; i++) {
            disableBehaviours[i].enabled = true;
        }
    }
}
