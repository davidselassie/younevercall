using UnityEngine;
using System.Collections;

public abstract class CharacterComponent : MonoBehaviour {
	public IslandComponent island;

	public float turnSpeed = 4.0f;
	public float moveSpeed = 10.0f;
	public float positionTolerance = 0.4f;
	public string label;

	private Quaternion facing;
	private Vector3 towardTarget;
	private Vector3 newPosition;



	public void MoveToIsland(IslandComponent newIsland) {
		this.island = newIsland;
		transform.LookAt (this.island.transform.position);
	}

	
	// Update is called once per frame
	void Update () {
		towardTarget = this.island.transform.position - transform.position;

		if (Mathf.Abs (towardTarget.magnitude) > positionTolerance){

			transform.position += towardTarget.normalized * moveSpeed * Time.deltaTime;
		}
	}

	public abstract void TurnUpdate (WorldState state);
}
