using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to the player and allows
/// them to fire the Blaster projectile.
/// </summary>

public class FireBlaster : MonoBehaviour {

	// Variables Start____________________

	// The blaster projectile is attached to this in the
	// inspector
	public GameObject blaster;

	// Quick references.
	private Transform myTransform;
	private Transform cameraHeadTransform;

	// The position at which the projectile should be
	// instantiated.
	private Vector3 launchPosition = new Vector3();

	// Variables End______________________

	// Use this for initialization
	void Start () 
	{
		myTransform = transform;
		cameraHeadTransform = myTransform.FindChild ("CameraHead");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetButton("Fire Weapon"))
		{
			// The launch position of the projectile will be just in front
			// of the CameraHead.

			launchPosition = cameraHeadTransform.TransformPoint(0, 0, 0.2f);

			// Create the blaster projectile at the launchPosition and tilt its angle
			// so that its horizontal using the angle eulerAngles.x + 90.
			Instantiate(blaster, launchPosition, Quaternion.Euler(cameraHeadTransform.eulerAngles.x + 90, 
			                                                      myTransform.eulerAngles.y, 0));
		}
	}
}
