using UnityEngine;
using System.Collections;

public class CharacterComponent : MonoBehaviour {
	public IslandComponent island;

	// Use this for initialization
	void Start () {

	}

	public void MoveToIsland(IslandComponent newIsland) {
		this.island = newIsland;
		this.transform.position = this.island.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
