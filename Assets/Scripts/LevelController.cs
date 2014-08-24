using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelController : MonoBehaviour
{
    private int bridgeDestroyedDuringTurnCount = 0;
    private IslandComponent mouseDownIsland = null;
    private GameObject newBridge;
    public GameObject bridgeFire;
    public GameObject bridgePrefab;
    public float maxBridgeLength = 6.0f;

    // Use this for initialization
    void Start ()
    {
        WorldState state = this.FindState ();
        this.UpdateGameObjectsToReflectAbstractState (state);
        foreach (IslandComponent a in state.islands) {
            foreach (IslandComponent b in state.islands) {
                BuildBridge (a, b);
            }
        }
        this.bridgeDestroyedDuringTurnCount = 0;
    }

    WorldState FindState ()
    {
        HashSet<BridgeComponent> bridges = new HashSet<BridgeComponent> (FindObjectsOfType (typeof(BridgeComponent)) as BridgeComponent[]);
        HashSet<CharacterComponent> characters = new HashSet<CharacterComponent> (FindObjectsOfType (typeof(CharacterComponent)) as CharacterComponent[]);
        HashSet<IslandComponent> islands = new HashSet<IslandComponent> (FindObjectsOfType (typeof(IslandComponent)) as IslandComponent[]);

        return new WorldState (bridges, characters, islands);
    }

    void UpdateGameObjectsToReflectAbstractState (WorldState state)
    {
        foreach (CharacterComponent character in state.characters) {
            character.MoveToIsland (character.island);
        }
    }

    void TurnUpdateComponents (WorldState state)
    {
        foreach (CharacterComponent character in state.characters) {
            character.TurnUpdate (state);
        }
        this.bridgeDestroyedDuringTurnCount = 0;
    }

    private bool CanAdvanceTurn ()
    {
        bool enoughBridgesDestroyed = bridgeDestroyedDuringTurnCount >= 1;
        if (!enoughBridgesDestroyed) {
            Debug.Log ("Can't advance turn: not enough bridges destroyed. Destroyed ==" + bridgeDestroyedDuringTurnCount);
        }
        return enoughBridgesDestroyed;
    }

    private GameObject ClickedObject ()
    {
        //This checks to see what you clicked on
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast (ray, out hit, 100)) {
            return hit.transform.gameObject;
        }
        //Returns an arbitrary GameObject -- the Main Camera, because there will always be one
        //This avoid a NullReferenceException that would be triggered by returning Null
        return GameObject.FindGameObjectWithTag ("MainCamera");
    }

    private void DestroyBridge (BridgeComponent bridge)
    {
        Debug.Log ("Destroying bridge from " + bridge.islandA.ToString () + " to " + bridge.islandB.ToString ());
        GameObject rubble = Instantiate (bridgeFire, bridge.transform.position, bridge.transform.rotation) as GameObject;
        rubble.transform.localScale = bridge.transform.localScale;
        Destroy (bridge.gameObject);
        this.bridgeDestroyedDuringTurnCount++;
    }

    private void BuildBridge (IslandComponent island1, IslandComponent island2)
    {
        //and if those two islands were close enough together but not the same island
        if ((island1.transform.position - island2.transform.position).magnitude <= maxBridgeLength
            && island1 != island2) {

            //and make sure there isn't a bridge connecting those two already
            IList<BridgeComponent> allBridges = FindObjectsOfType (typeof(BridgeComponent)) as BridgeComponent[];
            int lengthOfAllBridges = allBridges.Count;
            bool bridgeAlreadyExists = false;
            
            for (int i = 0; i < lengthOfAllBridges; i++) {
                if ((allBridges [i].islandA == island1 && allBridges [i].islandB == island2)
                    || (allBridges [i].islandA == island2 && allBridges [i].islandB == island1)) {
                    bridgeAlreadyExists = true;
                }
            }

            if (!bridgeAlreadyExists) {
            
                //make a new bridge connecting them
                newBridge = Instantiate (bridgePrefab, (island2.transform.position + island1.transform.position) / 2, Quaternion.identity) as GameObject;
                this.bridgeDestroyedDuringTurnCount--;
                
                //the bridge prefab is an empty game object, so its child the cylinder is what we want to play with
                BridgeComponent newBridgeComponents = newBridge.GetComponentInChildren <BridgeComponent> ();
                
                //initialize the bridge with its two islands
                newBridgeComponents.islandA = island1;
                newBridgeComponents.islandB = island2;
                
                //rotate it to the correct orientation and make it the right size
                newBridge.transform.LookAt (island1.transform.position);
                newBridge.transform.localScale = new Vector3 (1, 1, (island1.transform.position - island2.transform.position).magnitude * 0.85f);
                
                //announce what the two islands were that you're connecting
                Debug.Log ("Making bridge from " + island1 + " to " + island2);
            }
        }
    }
        
    // Update is called once per frame
    void Update ()
    {
        bool spacePressed = Input.GetKeyDown (KeyCode.Space);
        if (spacePressed && this.CanAdvanceTurn ()) {
            WorldState state = this.FindState ();
            this.TurnUpdateComponents (state);
            this.UpdateGameObjectsToReflectAbstractState (state);
        }

        if (Input.GetMouseButtonDown (0)) {
            GameObject clicked = this.ClickedObject ();
            BridgeComponent clickedBridge = clicked.GetComponent<BridgeComponent> ();
            if (clickedBridge) {
                this.DestroyBridge (clickedBridge);
            }

            IslandComponent clickedIsland = clicked.GetComponent<IslandComponent> ();
            if (clickedIsland) {
                this.mouseDownIsland = clickedIsland;
            } else {
                this.mouseDownIsland = null;
            }
        }

        if (Input.GetMouseButtonUp (0)) {
            GameObject clicked = this.ClickedObject ();
            IslandComponent clickedIsland = clicked.GetComponent<IslandComponent> ();

            //if we clicked on one island and let go of another, BuildBridge between them
            if (clickedIsland && this.mouseDownIsland != null) {
                BuildBridge (clickedIsland, this.mouseDownIsland);
            }
        }
    }

    public int CurrentScore (WorldState state)
    {
        return state.Census().Select(islandChars => islandChars.Value.Count).Max();
    }

    public void OnGUI ()
    {
        WorldState state = this.FindState ();
        GUI.Box (new Rect (10, 10, 200, 25), String.Format ("Current Score: {0}", this.CurrentScore (state)));
    }
}
