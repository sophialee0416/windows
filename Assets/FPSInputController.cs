using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Require a character controller to be attached to the same game object
[RequireComponent(typeof(CharacterMotor))]
[AddComponentMenu("Character/FPS Input Controller")]

public class FPSInputController : MonoBehaviour
{
	private CharacterMotor motor;
	public bool checkAutoWalk = false;
	
	// Use this for initialization
	void Awake()
	{
		motor = GetComponent<CharacterMotor>();
	}
	
	// Update is called once per frame
	void Update()
	{
        float pointer_x = Input.GetAxis("Mouse X");
        float pointer_y = Input.GetAxis("Mouse Y");
        if (Input.touchCount > 0)
        {
            pointer_x = Input.touches[0].deltaPosition.x;
            pointer_y = Input.touches[0].deltaPosition.y;
        }
        var distance = (float)2.0;
		// Get the input vector from keyboard or analog stick
		Vector3 directionVector;
		if (!checkAutoWalk) { 
			//directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            directionVector = new Vector3(pointer_x, 0, pointer_y);
		} else { 
			directionVector = new Vector3(0, 0, 1);
		}

		if (directionVector != Vector3.zero)
		{
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
            //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);
            //transform.forward = Camera.main.transform.forward;
            transform.position = transform.position + Camera.main.transform.forward * distance * Time.deltaTime;
        }
        
        // Apply the direction to the CharacterMotor
        motor.inputMoveDirection = transform.rotation * directionVector;
		//motor.inputJump = Input.GetButton("Jump");
	}
}