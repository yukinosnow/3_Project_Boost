using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidbody;
    AudioSource audiosource;
	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        ProcessInput();
	}

    private void ProcessInput()
    {
        if(Input.GetKey(KeyCode.A)&&!Input.GetKey(KeyCode.D))
        {
            print("Rotating Left");
            transform.Rotate(Vector3.forward);
        }
        if(Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            print("Rotating Right");
            transform.Rotate(Vector3.back);
        }
        if(Input.GetKey(KeyCode.Space))//Can thrust while rotating
        {
            //print("Thrusting");
            rigidbody.AddRelativeForce(Vector3.up);
            
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            audiosource.Play();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            audiosource.Pause();
        }
    }
}
