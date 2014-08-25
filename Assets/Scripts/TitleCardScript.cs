using UnityEngine;
using System.Collections;

public class TitleCardScript : MonoBehaviour {
	public float timer = 2f;
	public string levelToLoad = "Prologue";
	

	// Use this for initialization
	void Start () {
		StartCoroutine ("DisplayScene");
	
	}
	IEnumerator DisplayScene() {
		yield return new WaitForSeconds( timer );
		Application.LoadLevel ( levelToLoad );
}
}