using UnityEngine;

public class HomingCharacterComponent : CharacterComponent
{

    public IslandComponent target;

    override public void TurnUpdate (WorldState state)
    {
        this.MoveTowardsIsland (state, target);
    }
}
