using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {


    [SerializeField] float rcsThrust = 100f; //unity can see and change this value, but other script cannot change from other script
    [SerializeField] float thrustPower = 10f;
    Rigidbody rigidbody;
    AudioSource audiosource;
	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        Rotate();
        Thrust();
    }

    private void Rotate()
    {
        rigidbody.freezeRotation = true;//take manual control of rotation
        
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            
            //print("Rotating Left");
            transform.Rotate(Vector3.forward* rotationThisFrame);
        }
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            
            //print("Rotating Right");
            transform.Rotate(Vector3.back*rotationThisFrame);
        }
        rigidbody.freezeRotation = false;//resume physics control of rotation

    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))//Can thrust while rotating
        {
            //print("Thrusting");
            rigidbody.AddRelativeForce(Vector3.up* thrustPower);

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audiosource.Play();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            audiosource.Pause();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                {
                    print("OK");
                    break;
                }
            case "Fuel":
                {
                    print("Fuel");
                    break;
                }
            default:
                {
                    print("Dead");
                    break;
                }
        }
    }
}
