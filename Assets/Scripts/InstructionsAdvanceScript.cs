using UnityEngine;
using System.Collections;

public class InstructionsAdvanceScript : MonoBehaviour {
	public float timer = 2f;
	public string levelToLoad = "IslandHouseTown";
	

	// Use this for initialization
	void Start () {
		StartCoroutine ("DisplayScene");
	
	}
	IEnumerator DisplayScene() {
		yield return new WaitForSeconds( timer );
		Application.LoadLevel ( levelToLoad );
}
}