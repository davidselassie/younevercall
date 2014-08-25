using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IslandComponent : MonoBehaviour
{

    public List<WaypointComponent> waypoints;

    void Start ()
    {
        this.waypoints = new List<WaypointComponent>(GetComponentsInChildren<WaypointComponent>());
    }

    public WaypointComponent UnoccupiedWaypoint ()
    {
        foreach (WaypointComponent waypoint in this.waypoints) {
            if (!waypoint.occupiedBy) {
                return waypoint;
            }
        }
        return null;
    }
}
