using UnityEngine;
using System;
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
			this.UpdateCharacter(character);
		}
	}

	void UpdateCharacter (CharacterComponent character) {
		IslandComponent home = GameObject.FindGameObjectWithTag("HomeIsland").GetComponent<IslandComponent>();
		try {
			IList<IslandComponent> foundPath = this.ShortestBridgePath(character.island, home);
			if (foundPath.Count > 0) {
				character.MoveToIsland(foundPath[0]);
			}
		} catch (NoPathException) {
			// Do nothing, no path...
		}
	}

	List<IslandComponent> ShortestBridgePath (IslandComponent from, IslandComponent to) {
		return this.ShortestBridgePath(from, to, new List<IslandComponent>(0));
	}

	private class NoPathException : Exception {
		public IslandComponent from;
		public IslandComponent to;

		public NoPathException(IslandComponent from, IslandComponent to) {
			this.from = from;
			this.to = to;
		}
	}

	private List<IslandComponent> ShortestBridgePath (IslandComponent from, IslandComponent to, List<IslandComponent> pathToFrom) {
		List<IslandComponent> foundPath = new List<IslandComponent>(pathToFrom);
		if (to == from) {
			// Muffin. foundPath is already correct.
		} else if (this.AllIslandsAccessibleFromIsland(from).Contains(to)) {
			foundPath.Add(to);
		} else {
			List<List<IslandComponent>> paths = new List<List<IslandComponent>>();
			foreach (IslandComponent possibleNext in this.AllIslandsAccessibleFromIsland(from)) {
				if (!pathToFrom.Contains(possibleNext)) {
					try {
						List<IslandComponent> pathToPossibleNext = new List<IslandComponent>(pathToFrom);
						pathToPossibleNext.Add(possibleNext);
						paths.Add(this.ShortestBridgePath(possibleNext, to, pathToPossibleNext));
					} catch (NoPathException) {
						// That was a dead end.
					}
				}
			}
			if (paths.Count > 0) {
				int minLength = this.islands.Count + 1;
				foreach (List<IslandComponent> path in paths) {
					if (path.Count < minLength) {
						foundPath = path;
						minLength = path.Count;
					}
				}
			} else {
				throw new NoPathException(from, to);
			}
		}
		return foundPath;
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
