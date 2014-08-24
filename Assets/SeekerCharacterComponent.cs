using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeekerCharacterComponent : CharacterComponent
{
    public string targetCharacter;
    
    override public void TurnUpdate (WorldState state)
    {
        IslandComponent aimFor = state.CharacterWithLabel (this.targetCharacter).island;
        try {
            List<IslandComponent> foundPath = state.ShortestBridgePath (this.island, aimFor);
            if (foundPath.Count > 0) {
                this.MoveToIsland (foundPath [0]);
            }
        } catch (WorldState.NoPathException) {
            // Do nothing, no path...
        }
    }
}
