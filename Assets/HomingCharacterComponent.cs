using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HomingCharacterComponent : CharacterComponent {
	public string targetIsland;

	override public void TurnUpdate (WorldState state) {
		IslandComponent home = state.IslandWithLabel(this.targetIsland);
		try {
			List<IslandComponent> foundPath = state.ShortestBridgePath(this.island, home);
			if (foundPath.Count > 0) {
				this.MoveToIsland(foundPath[0]);
			}
		} catch (WorldState.NoPathException) {
			// Do nothing, no path...
		}
	}
}
