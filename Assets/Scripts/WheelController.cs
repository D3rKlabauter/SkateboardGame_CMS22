using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using extOSC;

public class WheelController : MonoBehaviour
{
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider backLeft;

    [SerializeField] Transform frontRightTransform;
    [SerializeField] Transform frontLeftTransform;
    [SerializeField] Transform backRightTransform;
    [SerializeField] Transform backLeftTransform;

    public AudioClip collectSound;
    public AudioClip deathSound;

<<<<<<< Updated upstream
=======
    public TextMeshProUGUI countText;
    //public TextMeshProUGUI winLooseText;

>>>>>>> Stashed changes
    public AudioSource backgroundMusic;
    private AudioSource audioSource;

    public int oscPortNumber = 10000;
    public string oscDeviceUUID;
    
    public float accelaration = 20f;
    public float breakingForce = 10f;
    public float maxTurnAngle = 15f;
    public float kickForce = 100f;
    public int difficultey = 2;

    private Rigidbody rb;
    
    private float movementX;

    private float currentAcceleration = 0f;
    private float currentBreakForce = 0f;
    private float currentTurnAngle = 0f;

    private int count = 0;
    private int maxCount = 0;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        
        // Initialize OSC
        OSCReceiver receiver = gameObject.AddComponent<OSCReceiver>();
        receiver.LocalPort = oscPortNumber;
        receiver.Bind("/ZIGSIM/" + oscDeviceUUID + "/quaternion", OnMoveOSC);
        

        count = 0;
        maxCount = GameObject.FindGameObjectsWithTag("Collectables").Length;
<<<<<<< Updated upstream
        //SetCountText();
=======
        SetCountText();
>>>>>>> Stashed changes
    }

    
    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        // calculates the rotation for steering
        movementX = movementVector.x;

    }
    

    
    public void OnMoveOSC(OSCMessage message)
    {
        movementX = (float)message.Values[0].FloatValue;

        //Debug.Log("movementX = " + movementX.ToString("F6"));
    }
    

    private void FixedUpdate()
    {
        
        currentAcceleration = accelaration; 

        // Apply accelaration to skateboard
        frontRight.motorTorque = currentAcceleration;
        frontLeft.motorTorque = currentAcceleration;
        backRight.motorTorque = currentAcceleration;
        backLeft.motorTorque = currentAcceleration;

        frontRight.brakeTorque = currentBreakForce;
        frontLeft.brakeTorque = currentBreakForce;
        backRight.brakeTorque = currentBreakForce;
        backLeft.brakeTorque = currentBreakForce;

        // Take care of the steering
        currentTurnAngle = maxTurnAngle * movementX * difficultey;
        frontLeft.steerAngle = currentTurnAngle;
        frontRight.steerAngle = currentTurnAngle;
        backRight.steerAngle = currentTurnAngle * -1;
        backLeft.steerAngle = currentTurnAngle * -1;
        

        // Update wheel meshes
        UpdateWheel(frontLeft, frontLeftTransform);
        UpdateWheel(frontRight, frontRightTransform);
        UpdateWheel(backLeft, backLeftTransform);
        UpdateWheel(backRight, backRightTransform);

    }

    void UpdateWheel(WheelCollider col, Transform trans)
    {
        // get wheel collider state.
        Vector3 position;
        Quaternion rotation;

        col.GetWorldPose(out position, out rotation);

        // Set wheel transform state.
        trans.position = position;
        trans.rotation = rotation;
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Collectables"))
        {
            audioSource.PlayOneShot(collectSound);

            other.gameObject.SetActive(false);
<<<<<<< Updated upstream

            count++;
            Debug.Log("counter = " + count);
            //SetCountText();


        }
}
=======

            count++;

            SetCountText();

            Debug.Log("counter = " + count);
            //SetCountText();


        }
    }
>>>>>>> Stashed changes

    private void OnCollisionEnter(Collision collision)
    {
        accelaration = 0f;

        backgroundMusic.Stop();
        audioSource.PlayOneShot(deathSound);

        //winLooseText.text = "G A M E  O V E R";

        // Go back to menu and so on...
        //Invoke("BackToMenu", 5.0f);
    }

    private void SetCountText()
    {
        //countText.text = "Count: " + count.ToString() + " | " + maxCount.ToString();
        countText.text = "Count:";
    }
}