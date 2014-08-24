using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class CharacterComponent : MonoBehaviour
{
    public IslandComponent island;
    public float turnSpeed = 4.0f;
    public float moveSpeed = 10.0f;
    public float positionTolerance = 0.4f;
    public float characterFeetHeight = 2.0f;
    private Quaternion facing;
    private Vector3 towardTarget;
    private Vector3 newPosition;

    public void MoveToIsland (IslandComponent newIsland)
    {
        this.island = newIsland;
        transform.LookAt (this.island.transform.position);
    }

    public void MoveTowardsIsland (WorldState state, IslandComponent target)
    {
        try {
            List<IslandComponent> foundPath = state.ShortestBridgePath (this.island, target);
            if (foundPath.Count > 0) {
                this.MoveToIsland (foundPath [0]);
            }
        } catch (WorldState.NoPathException) {
            // Do nothing, no path...
        }
    }

    // Update is called once per frame
    void Update ()
    {
        towardTarget = this.island.transform.position - transform.position;

        if (Mathf.Abs (towardTarget.magnitude) > positionTolerance) {
            
            newPosition = transform.position + towardTarget.normalized * moveSpeed * Time.deltaTime;
            transform.position = new Vector3 (newPosition.x, characterFeetHeight, newPosition.z);

        }
    }

    public abstract void TurnUpdate (WorldState state);
}
