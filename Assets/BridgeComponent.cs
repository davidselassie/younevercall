using UnityEngine;
using System.Collections;

public class BridgeComponent : MonoBehaviour {
	public IslandComponent islandA;
	public IslandComponent islandB;

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

	void OnMouseDown() {
		Debug.Log("Destroying bridge from " + this.islandA.ToString() + " to " + this.islandB.ToString());
		Object.Destroy(this.gameObject);
	}
}
