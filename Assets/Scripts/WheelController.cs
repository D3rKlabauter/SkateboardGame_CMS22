using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using extOSC;
using TMPro;
using UnityEngine.SceneManagement;

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
    public AudioClip deathSound1;
    public AudioClip deathSound2;
    public AudioClip finishSound;

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


    [Header("Component")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI gameOver;
    public TextMeshProUGUI finishText;

    [Header("TimerSettings")]
    public float currentTime;
    public bool countDown;

    [Header("limit Settings")]
    public bool hasLimit;
    public float timerLimit;

    private bool flag = true;


    void Update()
    {
        if (flag)
        {
            currentTime = countDown ? currentTime -= Time.deltaTime : currentTime += Time.deltaTime;
            if(hasLimit && (countDown && currentTime <= timerLimit || (!countDown && currentTime >= timerLimit )))
            {
                currentTime = timerLimit;
                SetTimerText();
                timerText.color = Color.red;
                enabled = false;

            }

            SetTimerText();
            
        }
    }

    private void SetTimerText()
    {
        timerText.text = currentTime.ToString("0.0");
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        flag = true;

        // Initialize OSC
        OSCReceiver receiver = gameObject.AddComponent<OSCReceiver>();
        receiver.LocalPort = oscPortNumber;
        receiver.Bind("/ZIGSIM/" + oscDeviceUUID + "/quaternion", OnMoveOSC);
        

        count = 0;
        maxCount = GameObject.FindGameObjectsWithTag("Collectables").Length;
        
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

            count++;

            int dollar = count * 10;
            countText.text = dollar + " $";
            Debug.Log("counter = " + count);

        }

        if (other.gameObject.CompareTag("Finish"))
        {
            audioSource.PlayOneShot(finishSound);
            flag = false;
            accelaration = 0f;
            finishText.text = "Free Trial Expired\n Contact  $K@TeRZ.at for more  action";
            backgroundMusic.Stop();

            Invoke("BackToMenu", 5.0f);



        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        accelaration = 0f;
        flag = false;

        audioSource.PlayOneShot(deathSound1);
        audioSource.PlayOneShot(deathSound2);

        int dollar = count * 10;
        gameOver.text = "Game Over\n" +
            dollar + " | " + maxCount + "0 $";

        backgroundMusic.Stop();
       



        // Go back to menu and so on...
        Invoke("BackToMenu", 5.0f);
    }

    private void BacktoMenu()
    {
        SceneManager.LoadScene(0);

    }

}