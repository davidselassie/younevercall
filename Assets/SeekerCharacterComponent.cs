using UnityEngine;

public class SeekerCharacterComponent : CharacterComponent
{
    public CharacterComponent target;
    
    override public void TurnUpdate (WorldState state)
    {
        this.MoveTowardsIsland (state, target.island);
    }
}
