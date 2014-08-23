using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LevelController : MonoBehaviour {

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
	}

	// Update is called once per frame
	void Update () {
		bool spacePressed = Input.GetKeyDown(KeyCode.Space);
		if (spacePressed) {
			WorldState state = this.FindState();
			this.TurnUpdateComponents(state);
			this.UpdateGameObjectsToReflectAbstractState(state);
		}
	}
}
