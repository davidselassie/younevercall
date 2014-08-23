﻿using UnityEngine;
using System.Collections;

public class BridgeComponent : MonoBehaviour {
	public IslandComponent islandA;
	public IslandComponent islandB;

	// Use this for initialization
	void Start () {

	}

	public bool Touches (IslandComponent island) {
		return island == this.islandA || island == this.islandB;
	}

	public IslandComponent LeadsTo (IslandComponent from) {
		if (from == this.islandA) {
			return this.islandB;
		} else if (from == this.islandB) {
			return this.islandA;
		}
		throw new MissingReferenceException();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}