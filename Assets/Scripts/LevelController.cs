using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LevelController : MonoBehaviour
{
    private int maxBridgesToEndTurn;
    private IslandComponent mouseDownIsland = null;
    public GameObject bridgeFire;
    public GameObject bridgePrefab;
    public float maxBridgeLength = 14.0f;
    private string errorMessage;
    private bool gameOver;

    void Start ()
    {
        this.gameOver = false;
        WorldState state = this.FindState ();
        foreach (IslandComponent a in state.islands) {
            foreach (IslandComponent b in state.islands) {
                BuildBridge (state, a, b);
                state = this.FindState ();
            }
        }
        this.errorMessage = null;
        this.maxBridgesToEndTurn = state.bridges.Count - 1;
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

    void PerformEndTurnStateUpdates (WorldState state)
    {
        foreach (CharacterComponent character in state.characters) {
            character.TurnUpdate (state);
        }
        this.maxBridgesToEndTurn--;
    }

    private bool CanAdvanceTurn (WorldState state)
    {
        return state.bridges.Count <= this.maxBridgesToEndTurn;
    }

    private GameObject ClickedObject ()
    {
        //This checks to see what you clicked on
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast (ray, out hit, 100)) {
            return hit.transform.gameObject;
        } else {
            return null;
        }
    }

    private void DestroyBridge (WorldState state, BridgeComponent bridge)
    {
        GameObject rubble = Instantiate (bridgeFire, bridge.transform.position, bridge.transform.rotation) as GameObject;
        rubble.transform.localScale = bridge.transform.localScale;
        Destroy (bridge.gameObject);
        state.bridges.Remove (bridge);
    }

    private void BuildBridge (WorldState state, IslandComponent island1, IslandComponent island2)
    {
        if ((island1.transform.position - island2.transform.position).magnitude > maxBridgeLength) {
            this.errorMessage = "Can't build bridge! Would be too long.";
        } else if (island1 != island2) {
            bool bridgeAlreadyExists = state.AllIslandsAccessibleFromIsland (island1).Contains (island2);
            if (!bridgeAlreadyExists) {
                //make a new bridge connecting them
                GameObject newBridge = Instantiate (bridgePrefab, (island2.transform.position + island1.transform.position) / 2, Quaternion.identity) as GameObject;
                this.errorMessage = null;
                
                //the bridge prefab is an empty game object, so its child the cylinder is what we want to play with
                BridgeComponent newBridgeComponents = newBridge.GetComponentInChildren <BridgeComponent> ();
                
                //initialize the bridge with its two islands
                newBridgeComponents.islandA = island1;
                newBridgeComponents.islandB = island2;
                
                //rotate it to the correct orientation and make it the right size
                newBridge.transform.LookAt (island1.transform.position);
                newBridge.transform.localScale = new Vector3 (1, 1, (island1.transform.position - island2.transform.position).magnitude * 0.85f);
                
                //announce what the two islands were that you're connecting
                //Debug.Log ("Making bridge from " + island1 + " to " + island2);
            } else {
                this.errorMessage = "There's already a bridge there.";
            }
        }
    }

    private bool IsGameOver ()
    {
        return this.maxBridgesToEndTurn < 1;
    }

    private void OnGameOver (WorldState state)
    {
        this.errorMessage = String.Format ("Game over. You got {0} family members together! Space to play again.", this.CurrentScore (state));
    }

    void Update ()
    {
        if (Input.GetKeyDown (KeyCode.Space)) {
            WorldState state = this.FindState ();
            if (this.gameOver) {
                Application.LoadLevel(Application.loadedLevel);
            } else if (this.CanAdvanceTurn (state)) {
                this.PerformEndTurnStateUpdates (state);
                this.UpdateGameObjectsToReflectAbstractState (state);
                if (this.CanAdvanceTurn(state)) {
                    this.errorMessage = "Press space to end turn!";
                } else {
                    this.errorMessage = null;
                }
                if (this.IsGameOver ()) {
                    this.gameOver = true;
                    this.OnGameOver (state);
                }
            } else {
                this.errorMessage = "You need fewer bridges before this turn can end.";
            }
        } else if (Input.GetMouseButtonDown (0) && !this.gameOver) {
            GameObject clicked = this.ClickedObject ();
            if (clicked) {
                BridgeComponent clickedBridge = clicked.GetComponent<BridgeComponent> ();
                IslandComponent clickedIsland = clicked.GetComponent<IslandComponent> ();
                this.mouseDownIsland = null;
                if (clickedBridge) {
                    WorldState state = this.FindState ();
                    this.DestroyBridge (state, clickedBridge);
                    if (this.CanAdvanceTurn (state)) {
                        this.errorMessage = "Press space to end turn!";
                    } else {
                        this.errorMessage = null;
                    }
                } else if (clickedIsland) {
                    this.mouseDownIsland = clickedIsland;
                }
            }
        } else if (Input.GetMouseButtonUp (0) && !this.gameOver) {
            GameObject clicked = this.ClickedObject ();
            if (clicked) {
                IslandComponent clickedIsland = clicked.GetComponent<IslandComponent> ();
                //if we clicked on one island and let go of another, BuildBridge between them
                if (clickedIsland && this.mouseDownIsland != null) {
                    WorldState state = this.FindState ();
                    this.BuildBridge (state, clickedIsland, this.mouseDownIsland);
                    this.mouseDownIsland = null;
                }
            }
        }
    }

    public int CurrentScore (WorldState state)
    {
        return state.Census ().Select (islandChars => islandChars.Value.Count).Max ();
    }

    public void OnGUI ()
    {
        WorldState state = this.FindState ();
        GUI.Box (new Rect (10, 10, 200, 25), String.Format ("Current Bridges: {0}", state.bridges.Count));
        GUI.Box (new Rect (10, 45, 200, 25), String.Format ("Max Bridges to End Turn: {0}", this.maxBridgesToEndTurn));
        if (this.errorMessage != null) {
            GUI.Box (new Rect (10, 80, 500, 25), this.errorMessage);
        }
    }
}
