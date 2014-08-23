using UnityEngine;
using System.Collections;

public class HomingCharacterComponent : MonoBehaviour {
	public string HomingIslandTag;

	CharacterComponent character;

	// Use this for initialization
	void Start () {
		this.character = this.GetComponent<CharacterComponent>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
