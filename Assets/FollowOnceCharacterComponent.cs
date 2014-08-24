using UnityEngine;
using System.Collections;

public class FollowOnceCharacterComponent : CharacterComponent {
    private CharacterComponent lastTouched = null;

    public override void TurnUpdate (WorldState state) {
        if (!this.lastTouched) {
            foreach (CharacterComponent character in state.characters) {
                if (character.name != this.name && character.island == this.island) {
                    this.lastTouched = character;
                }
            }
        } else {
            this.MoveToIsland(this.lastTouched.island);
            this.lastTouched = null;
        }
    }
}
