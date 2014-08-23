using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelController : MonoBehaviour {
	private IList<BridgeComponent> bridges;
	private IList<CharacterComponent> characters;
	private IList<IslandComponent> islands;

	// Use this for initialization
	void Start () {
		this.bridges = FindObjectsOfType(typeof(BridgeComponent)) as BridgeComponent[];
		this.characters = FindObjectsOfType(typeof(CharacterComponent)) as CharacterComponent[];
		this.islands = FindObjectsOfType(typeof(IslandComponent)) as IslandComponent[];

		this.CleanUpCharacters();
	}

	void CleanUpCharacters () {
		foreach (CharacterComponent character in this.characters) {
			character.MoveToIsland(character.island);
		}
	}

	public void PerformTurnUpdate () {
		foreach (CharacterComponent character in this.characters) {
			IList<IslandComponent> nextIslands = this.AllIslandsAccessibleFromIsland(character.island);
			if (nextIslands.Count > 0) {
				IslandComponent nextIsland = nextIslands[0];
				character.MoveToIsland(nextIsland);
			}
		}
	}

	void UpdateCharacter (HomingCharacterComponent character) {

	}

	IList<IslandComponent> ShortestBridgePath (IslandComponent from, IslandComponent to) {
//		if (to == from) {
//			return new List<IslandComponent>(0);
//		} else if () {
//
//		}
		return null;
	}

	IList<BridgeComponent> AllBridgesTouchingIsland (IslandComponent island) {
		IList<BridgeComponent> touching = new List<BridgeComponent>(this.bridges.Count);
		foreach (BridgeComponent bridge in this.bridges) {
			if (bridge.Touches(island)) {
				touching.Add(bridge);
			}
		}
		return touching;
	}

	IList<IslandComponent> AllIslandsAccessibleFromIsland (IslandComponent from) {
		IList<BridgeComponent> bridgesTouching = this.AllBridgesTouchingIsland(from);
		IList<IslandComponent> accessible = new List<IslandComponent>(bridgesTouching.Count);
		foreach (BridgeComponent bridge in bridgesTouching) {
			accessible.Add(bridge.LeadsTo(from));
		}
		return accessible;
	}

	// Update is called once per frame
	void Update () {
		bool spacePressed = Input.GetKeyDown(KeyCode.Space);
		if (spacePressed) {
			this.PerformTurnUpdate();
		}
	}
}
