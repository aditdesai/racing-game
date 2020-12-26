using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CarController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private float steerAngle;
    
    [Header("Car Mechanics")]
    public Rigidbody rb;
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;
    public Transform frontLeftWheelTransform;
    public Transform frontRightWheelTransform;
    public Transform rearLeftWheelTransform;
    public Transform rearRightWheelTransform;
    public float maxSteeringAngle = 40f;
    public float motorForce = 3500f;

    [Header("Checkpoints")]
    public RaceManager raceManager;
    private float lapTime, bestLapTime;
    public int nextCheckpoint = 0;
    public int currentLap = 0;
    
    
    
    [Header("UI")]
    public UIManager uiManager;

    [Header("AI")]
    public bool isAI;
    public Transform path;
    private List<Transform> nodes;

    private void Start()
    {
        
        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();//gets all transforms including parent
        nodes = new List<Transform>();
        //filter out parent
        for (int i = 0; i < pathTransforms.Length; i++)
        {
            if (pathTransforms[i] != path.transform)
                nodes.Add(pathTransforms[i]);
        }
        
        
    }

    private void FixedUpdate()
    {
        lapTime += Time.deltaTime;
        if (!isAI)
        {
            uiManager.LapTimeUpdate(lapTime);
            GetInput();
        }
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        
        
    }

    private void LateUpdate()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);//restrict rotation in z, freeze rotation not working properly
    }

    private void GetInput()
    {
        horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal");
        verticalInput = CrossPlatformInputManager.GetAxis("Vertical");
        
    }

    private void HandleSteering()
    {
        if(!isAI)
            steerAngle = maxSteeringAngle * horizontalInput;
        else
        {
            //if relative vector from current pos to next checkpoint pos is positive, then right or left
            Vector3 relativeVector = transform.InverseTransformPoint(nodes[nextCheckpoint].position);
            relativeVector /= relativeVector.magnitude;
            steerAngle = maxSteeringAngle * relativeVector.x;
        }
        
        
        frontLeftWheelCollider.steerAngle = steerAngle;
        frontRightWheelCollider.steerAngle = steerAngle;
    }

    private void HandleMotor()
    {
        if (!isAI)
        {
            frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
            frontRightWheelCollider.motorTorque = verticalInput * motorForce;


            if (verticalInput == 0)
            {
                rb.drag = 0.4f;//otherwise the car won't stop when up down keys are released
                Invoke("UpdateDrag", 2.0f);
            }
            else
                rb.drag = 0.05f;
        }
        else
        {
            frontLeftWheelCollider.motorTorque = motorForce;
            frontRightWheelCollider.motorTorque = motorForce;
        }
    }
    
    private void UpdateDrag()
    {
        rb.drag = 1f;
    }

    private void UpdateWheels()
    {
        UpdateWheelPos(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheelPos(frontRightWheelCollider, frontRightWheelTransform);
        UpdateWheelPos(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateWheelPos(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateWheelPos(WheelCollider wheelCollider, Transform trans)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        trans.rotation = rot;
        trans.position = pos;
    }

    public void CheckpointHit(int cpNumber)
    {
        if (cpNumber == nextCheckpoint)
        {
            nextCheckpoint++;
            nextCheckpoint %= 12;//total checkpoints are 12
            
            if (nextCheckpoint == 1)
            {
                currentLap++;
                
                if (!isAI)
                {
                    if (currentLap != 1)
                    {
                        if (lapTime < bestLapTime || bestLapTime == 0)
                            bestLapTime = lapTime;
                        lapTime = 0f;
                        uiManager.BestLapTimeUpdate(bestLapTime);
                    }
                    if (currentLap == raceManager.totalLaps + 1)
                    {
                        raceManager.FinishRace(bestLapTime);
                        /*
                        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();//gets all transforms including parent
                        nodes = new List<Transform>();
                        //filter out parent
                        for (int i = 0; i < pathTransforms.Length; i++)
                        {
                            if (pathTransforms[i] != path.transform)
                                nodes.Add(pathTransforms[i]);
                        }
                        */
                    }
                    else
                        uiManager.LapUpdate(currentLap, raceManager.totalLaps);
                }
                else
                {
                    if (currentLap == raceManager.totalLaps + 1)
                        rb.drag = 0.75f;
                }
            }

            
            
        }
    }

    
}