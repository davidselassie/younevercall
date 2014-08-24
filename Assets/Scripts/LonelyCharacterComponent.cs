using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LonelyCharacterComponent : CharacterComponent
{
    public override void TurnUpdate (WorldState state)
    {
        Dictionary<IslandComponent, HashSet<CharacterComponent>> tally = state.Census ();
        HashSet<CharacterComponent> charactersOnSameIsland;
        tally.TryGetValue (this.island, out charactersOnSameIsland);
        charactersOnSameIsland.Remove (this);

        IslandComponent islandWithMostPeople = tally.OrderByDescending(islandChars => islandChars.Value.Count).First().Key;
        if (islandWithMostPeople) {
            this.MoveTowardsIsland (state, islandWithMostPeople);
        }
    }
}
