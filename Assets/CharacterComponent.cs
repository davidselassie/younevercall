using UnityEngine;
using System.Collections;

public abstract class CharacterComponent : MonoBehaviour {
	public IslandComponent island;
	public string label;

	public void MoveToIsland(IslandComponent newIsland) {
		this.island = newIsland;
		this.transform.position = this.island.transform.position;
	}

	public abstract void TurnUpdate (WorldState state);
}
