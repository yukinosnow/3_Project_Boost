using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {


    [SerializeField] float rcsThrust = 100f; //unity can see and change this value, but other script cannot change from other script
    [SerializeField] float thrustPower = 10f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip death;
    [SerializeField] ParticleSystem mainEngineParticle;
    [SerializeField] ParticleSystem successParticle;
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] float levelLoadDelay = 2f;
    bool CollisionAreEnable = true;
    Rigidbody rigidbody;
    AudioSource audiosource;
    enum State { Alive,Transcending,Dying};
    State state = State.Alive;
	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (state == State.Alive)
        {
            RespondToRotateInput();
            RespondToThrustInput();
        }
        if(Debug.isDebugBuild)
            RespondToDebugKey();

    }

    private void RespondToDebugKey()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            CollisionAreEnable = !CollisionAreEnable;
        }
    }

    private void RespondToRotateInput()
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

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))//Can thrust while rotating
        {
            //print("Thrusting");
            ApplyThrust();

        }
        else
        {
            audiosource.Stop();
            mainEngineParticle.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidbody.AddRelativeForce(Vector3.up * thrustPower* Time.deltaTime);
        if (!audiosource.isPlaying)
        {
            audiosource.PlayOneShot(mainEngine);
        }
        mainEngineParticle.Play();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive){  return; }//ignore collision when dead

        switch(collision.gameObject.tag)
        {
            case "Friendly":
                {
                    state = State.Alive;
                    break;
                }
            case "Finish":
                {
                    StartSuccessSequence();
                    break;
                }
            default:
                {
                    if(CollisionAreEnable)
                        StartDeathSequence();
                    break;
                }
        }
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audiosource.Stop();
        audiosource.PlayOneShot(death);
        deathParticle.Play();
        Invoke("ReturnPreviousScene", levelLoadDelay);
    }

    private void StartSuccessSequence()
    {
        audiosource.Stop();
        audiosource.PlayOneShot(success);
        state = State.Transcending;
        successParticle.Play();
        Invoke("LoadNextScene", levelLoadDelay);//parameterise time
    }

    private void ReturnPreviousScene()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        int currentSceneIndex=SceneManager.GetActiveScene().buildIndex;
        int maxSceneIndex = SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene((currentSceneIndex+1)% maxSceneIndex);//allow for more than 2 levels
    }
}
