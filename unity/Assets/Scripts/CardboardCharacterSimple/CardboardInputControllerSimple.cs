using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Require a character controller to be attached to the same game object
[RequireComponent(typeof(CharacterMotor))]
[AddComponentMenu("Character/Cardboard Input Controller Simple")]
public class CardboardInputControllerSimple : MonoBehaviour {
    
	public float creepThresholdAngle = 0f;
	public float jumpThresholdAngle = 0f;

	[HideInInspector] public Vector3 directionVector;
	[HideInInspector] public bool touchActivated = false;

	private const float RIGHT_ANGLE = 90f; 
	private CharacterMotor motor;
	private CardboardHead head = null;
	private CharacterController characterController;

    void Awake() {
        motor = GetComponent<CharacterMotor>();
		head = Camera.main.GetComponent<StereoController>().Head;
		characterController = GetComponent<CharacterController>();
	}

    void Update() {
		// Get the input vector from kayboard or analog stick
		#if UNITY_ANDROID
			if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) { 
				//directionVector = Quaternion.Euler(new Vector3 (0f, -transform.rotation.eulerAngles.y, 0f)) * head.transform.forward;
				touchActivated = true;
			} else if (Input.touchCount < 1 || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)) {
				directionVector = new Vector3(0f, 0f, 0f);
				touchActivated = false;
			}

			if (touchActivated) { 
				directionVector = Quaternion.Euler(new Vector3 (0f, -transform.rotation.eulerAngles.y, 0f)) * head.transform.forward;
			}
		#endif

		#if UNITY_STANDALONE_OSX || UNITY_EDITOR
			directionVector = head.transform.localRotation * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		#endif

        if (directionVector != Vector3.zero) { 
            // Get the length of the directon vector and then normalize it
            // Dividing by the length is cheaper than normalizing when we already have the length anyway
            float directionLength = directionVector.magnitude;
            directionVector = directionVector / directionLength;

            // Make sure the length is no bigger than 1
            directionLength = Mathf.Min(1.0f, directionLength);

            // Make the input vector more sensitive towards the extremes and less sensitive in the middle
            // This makes it easier to control slow speeds when using analog sticks
            directionLength = directionLength * directionLength;

            // Multiply the normalized direction vector by the modified length
            directionVector = directionVector * directionLength;
        }

        // Apply the direction to the CharacterMotor
        motor.inputMoveDirection = transform.rotation * directionVector; 
        motor.inputJump = Input.GetButton("Jump");
	}

}