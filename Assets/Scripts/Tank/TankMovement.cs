using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public int PlayerNumber = 1;
    public float Speed = 12f;
    public float TurnSpeed = 180f;
    public AudioSource movementSource;
    public AudioClip EngineIdle;
    public AudioClip EngineDrive;
    public float pitchRange = 0.2f;


    private string movementAxisName;
    private string turnAxisName;
    private Rigidbody body;
    private float movementValue;
    private float turnValue;
    private float originalPitch;
    
     


    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }


    private void OnEnable ()
    {
        body.isKinematic = false;
        movementValue = 0;
        turnValue = 0;
    }


    private void OnDisable ()
    {
        body.isKinematic = true;
    }


    private void Start()
    {
        movementAxisName = "Vertical" + PlayerNumber;
        turnAxisName = "Horizontal" + PlayerNumber;

        originalPitch = movementSource.pitch;
    }
    

    private void Update()
    {
        // Store the player's input and make sure the audio for the engine is playing.
        movementValue = Input.GetAxis(movementAxisName);
        turnValue = Input.GetAxis(turnAxisName);

        EngineAudio();
    }


    private void EngineAudio()
    {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.

        if( Mathf.Abs( movementValue)  < 0.1f && Mathf.Abs(turnValue) < 0.1f)
        {
            if(movementSource.clip == EngineDrive)
            {
                movementSource.clip = EngineIdle;
                movementSource.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
                movementSource.Play();
            }
        }
        else
        {
            if (movementSource.clip == EngineIdle)
            {
                movementSource.clip = EngineDrive;
                movementSource.pitch = Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);
                movementSource.Play();
            }
        }
    }


    private void FixedUpdate()
    {
        // Move and turn the tank.
        Move();
        Turn();
    }


    private void Move()
    {
        // Adjust the position of the tank based on the player's input.
        Vector3 dir = transform.forward * movementValue * Speed * Time.deltaTime;
        body.MovePosition(dir+ transform.position);
    }


    private void Turn()
    {
        // Adjust the rotation of the tank based on the player's input.
        float turn = TurnSpeed * turnValue * Time.deltaTime;

        Quaternion rotation = Quaternion.Euler(0, turn, 0);
        body.MoveRotation(transform.rotation * rotation);

    }
}