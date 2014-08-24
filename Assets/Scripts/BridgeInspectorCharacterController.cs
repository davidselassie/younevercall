using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BridgeInspectorCharacterController : CharacterComponent
{
    public override void TurnUpdate (WorldState state)
    {
        List<BridgeComponent> bridgesByAge = state.BridgesNewestToOldest ();
        if (bridgesByAge.Count > 0) {
            BridgeComponent lastBridgeBuilt = bridgesByAge [0];

            try {
                List<IslandComponent> pathToA = state.ShortestBridgePath (this.island, lastBridgeBuilt.islandA);
                List<IslandComponent> pathToB = state.ShortestBridgePath (this.island, lastBridgeBuilt.islandB);
                List<IslandComponent> shortestPath = pathToA.Count < pathToB.Count ? pathToA : pathToB;
                shortestPath.Insert (0, this.island);
                IslandComponent crossBridgeIsland = shortestPath [shortestPath.Count - 1] == lastBridgeBuilt.islandA ? lastBridgeBuilt.islandB : lastBridgeBuilt.islandA;
                shortestPath.Add (crossBridgeIsland);

                // Start along that path. We added the current island at 0, so 1 will be the other side of the bridge.
                this.MoveToIsland (shortestPath [1]);
            } catch (WorldState.NoPathException) {
                // If there's no path to the new bridge, do nothing.
            }
        }
    }
}
