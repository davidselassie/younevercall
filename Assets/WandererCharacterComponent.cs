using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WandererCharacterComponent : CharacterComponent
{
    private HashSet<IslandComponent> visitedIslands = new HashSet<IslandComponent> ();
    private IslandComponent targetIsland;

    override public void TurnUpdate (WorldState state)
    {
        if (!this.targetIsland || this.targetIsland == this.island) {
            this.targetIsland = this.NextUnvisitedIsland (state);
        }
        if (this.targetIsland) {
            this.MoveTowardsIsland (state, this.targetIsland);
        }
        this.visitedIslands.Add (this.island);
    }

    private IslandComponent NextUnvisitedIsland (WorldState state)
    {
        HashSet<IslandComponent> unvisited = new HashSet<IslandComponent> (state.islands);
        unvisited.ExceptWith (this.visitedIslands);
        if (unvisited.Count > 0) {
            IEnumerator<IslandComponent> e = unvisited.GetEnumerator ();
            e.MoveNext ();
            return e.Current;
        } else {
            return null;
        }
    }
}
