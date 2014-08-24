using UnityEngine;
using System.Collections;

public abstract class CharacterComponent : MonoBehaviour {
	public IslandComponent island;

	public float turnSpeed = 4.0f;
	public float moveSpeed = 10.0f;
	public float positionTolerance = 0.4f;
	public string label;
    public float characterFeetHeight = 2.0f;

	private Quaternion facing;
	private Vector3 towardTarget;
	private Vector3 newPosition;
    
//    //uses the characterFeetHeight value from LevelController to set the y position
//    float yPos = GameObject.FindGameObjectWithTag ("LevelController").GetComponent<LevelController> ().characterFeetHeight;

    
	public void MoveToIsland(IslandComponent newIsland) {
		this.island = newIsland;
		transform.LookAt (this.island.transform.position);
	}
    
	// Update is called once per frame
	void Update () {
		towardTarget = this.island.transform.position - transform.position;

		if (Mathf.Abs (towardTarget.magnitude) > positionTolerance){
            
			newPosition = transform.position + towardTarget.normalized * moveSpeed * Time.deltaTime;
            transform.position = new Vector3 (newPosition.x, characterFeetHeight, newPosition.z);
		}
	}

	public abstract void TurnUpdate (WorldState state);
}
