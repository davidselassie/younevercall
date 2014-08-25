using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IslandComponent : MonoBehaviour
{
    public List<GameObject> standingPoints;

    public GameObject FindOpenSpot(){

        for (int i = 0; i < standingPoints.Count; i++) {
            if (standingPoints[i].GetComponent<StandingPoint>().occupied == false){
                Debug.Log("found an empty spot." + standingPoints[i]);
                return standingPoints[i];
            }
        }
        Debug.Log("NO EMPTY SPOTS FOUND");
        return this.gameObject;
    }
}
