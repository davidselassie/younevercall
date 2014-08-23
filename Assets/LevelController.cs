using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LevelController : MonoBehaviour {
	private int bridgeDestroyedDuringTurnCount = 0;
	private IslandComponent mouseDownIsland = null;

	// Use this for initialization
	void Start () {
		this.UpdateGameObjectsToReflectAbstractState(this.FindState());
	}

	WorldState FindState () {
		IList<BridgeComponent> bridges = FindObjectsOfType(typeof(BridgeComponent)) as BridgeComponent[];
		IList<CharacterComponent> characters = FindObjectsOfType(typeof(CharacterComponent)) as CharacterComponent[];
		IList<IslandComponent> islands = FindObjectsOfType(typeof(IslandComponent)) as IslandComponent[];
		return new WorldState(bridges, characters, islands);
	}

	void UpdateGameObjectsToReflectAbstractState (WorldState state) {
		foreach (CharacterComponent character in state.characters) {
			character.MoveToIsland(character.island);
		}
	}

	void TurnUpdateComponents (WorldState state) {
		foreach (CharacterComponent character in state.characters) {
			character.TurnUpdate(state);
		}
		this.bridgeDestroyedDuringTurnCount = 0;
	}

	private bool CanAdvanceTurn () {
		bool enoughBridgesDestroyed = bridgeDestroyedDuringTurnCount > 1;
		if (!enoughBridgesDestroyed) {
			Debug.Log("Can't advance turn: not enough bridges destroyed");
		}
		return enoughBridgesDestroyed;
	}

	private GameObject ClickedObject () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, 100)) {
			return hit.transform.gameObject;
		}
		return null;
	}

	private void DestroyBridge (BridgeComponent bridge) {
		Debug.Log("Destroying bridge from " + bridge.islandA.ToString() + " to " + bridge.islandB.ToString());
		Destroy(bridge.gameObject);
		this.bridgeDestroyedDuringTurnCount++;
	}

	// Update is called once per frame
	void Update () {
		bool spacePressed = Input.GetKeyDown(KeyCode.Space);
		if (spacePressed && this.CanAdvanceTurn()) {
			WorldState state = this.FindState();
			this.TurnUpdateComponents(state);
			this.UpdateGameObjectsToReflectAbstractState(state);
		}
		if (Input.GetMouseButtonDown(0)) {
			GameObject clicked = this.ClickedObject();
			BridgeComponent clickedBridge = clicked.GetComponent<BridgeComponent>();
			if (clickedBridge) {
				this.DestroyBridge(clickedBridge);
			}
			IslandComponent clickedIsland = clicked.GetComponent<IslandComponent>();
			if (clickedIsland) {
				this.mouseDownIsland = clickedIsland;
			} else {
				this.mouseDownIsland = null;
			}
		}
		if (Input.GetMouseButtonUp(0)) {
			GameObject clicked = this.ClickedObject();
			IslandComponent clickedIsland = clicked.GetComponent<IslandComponent>();
			if (clickedIsland) {
				Debug.Log ("Making bridge from " + this.mouseDownIsland + " to " + clickedIsland);
				// TODO: Actually make island.
			}
		}
	}
}
