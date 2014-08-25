using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class CharacterComponent : MonoBehaviour
{
    public IslandComponent island;  // Never set directly other than in Unity inspector, always use Move* functions.
    public WaypointComponent islandWaypoint;
    public List<IslandComponent> islandHistory = new List<IslandComponent> ();
    public float turnSpeed = 4.0f;
    public float moveSpeed = 10.0f;
    public float positionTolerance = 0.4f;
    public float characterFeetHeight = 0.0f;
    private Quaternion facing;
    private Vector3 towardTarget;
    private Vector3 newPosition;

    public void MoveToIsland (IslandComponent newIsland)
    {
        if (newIsland != this.island) {
            // Un-occupy our existing waypoint.
            if (this.islandWaypoint) {
                this.islandWaypoint.occupiedBy = null;
            }
            // Occupy a new one.
            this.islandWaypoint = newIsland.UnoccupiedWaypoint ();
            this.islandWaypoint.occupiedBy = this;
        }
        transform.LookAt (this.islandWaypoint.transform.position);
        this.island = newIsland;
        islandHistory.Add (this.island);
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
        Vector3 towardTarget = this.islandWaypoint.transform.position - transform.position;

        if (Mathf.Abs (towardTarget.magnitude) > positionTolerance) {
            Vector3 newPosition = transform.position + towardTarget.normalized * moveSpeed * Time.deltaTime;
            transform.position = new Vector3 (newPosition.x, characterFeetHeight, newPosition.z);

        }
    }

    public abstract void TurnUpdate (WorldState state);
}
