using UnityEngine;
using System.Collections;

public class DogCharacterComponent : CharacterComponent
{
    public IslandComponent butcher;

    public override void TurnUpdate (WorldState state)
    {
        CharacterComponent chaseCharacter = this.CharacterLastAtButcher (state);
        if (chaseCharacter) {
            this.MoveTowardsIsland (state, chaseCharacter.island);
        }
    }

    private CharacterComponent CharacterLastAtButcher (WorldState state)
    {
        CharacterComponent mostRecentlyAtButcherCharacter = null;
        int? turnsAgoAtButcher = null;
        foreach (CharacterComponent character in state.characters) {
            for (int i = 0; i < character.islandHistory.Count; i++) {
                IslandComponent island = character.islandHistory [i];
                if (island == this.butcher && (turnsAgoAtButcher == null || i < turnsAgoAtButcher)) {
                    turnsAgoAtButcher = i;
                    mostRecentlyAtButcherCharacter = character;
                }
            }
        }
        return mostRecentlyAtButcherCharacter;
    }
}
