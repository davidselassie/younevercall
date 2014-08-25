using UnityEngine;
using System.Collections;

public class PacingCharacterComponent : CharacterComponent
{
    public IslandComponent pingIsland;
    public IslandComponent pongIsland;
    private IslandComponent currentTarget;

    void Start ()
    {
        this.currentTarget = this.island == this.pingIsland ? this.pongIsland : this.pingIsland;
    }

    public override void TurnUpdate (WorldState state)
    {
        if (this.island == this.currentTarget) {
            this.currentTarget = this.currentTarget == this.pingIsland ? this.pongIsland : this.pingIsland;
        }
        this.MoveTowardsIsland (state, this.currentTarget);
    }
}
