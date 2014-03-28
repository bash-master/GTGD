using UnityEngine;
using System.Collections;

/// <summary>
/// This Script is attached to the player and it
/// causes the camera to continuously follow the CameraHead.
/// </summary>


public class CameraScript : MonoBehaviour {

	// Variables Start____________________
	private Camera myCamera;
	private Transform cameraHeadTransform;
	// Variables End______________________



	// Use this for initialization
	void Start () 
	{
		myCamera = Camera.main;
		cameraHeadTransform = transform.FindChild ("CameraHead");
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Make the camera follow the player's cameraHeadTransform.
		myCamera.transform.position = cameraHeadTransform.position;
		myCamera.transform.rotation = cameraHeadTransform.rotation;
	}
}
