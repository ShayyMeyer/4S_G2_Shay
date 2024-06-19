using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    
    private float movingSpeed = 6f;
    [Header("Main Values For Moving")]
    [SerializeField] private float defaultSpeed = 6f;
    [SerializeField] private float sprintingSpeed = 6f;
    
    private Vector2 inputVector;
    private Rigidbody rb;
    
    [Header("Ground Check")]
    [SerializeField] private Transform transformRayStart;
    [SerializeField] private float rayLength = 0.5f;
    [SerializeField] private LayerMask layerGroundCheck;
    
    [Header("Slope Check")]
    [SerializeField] private float raySlopeLength = 0.1f;
    [SerializeField] private float maxAngleSlope = 20f;
    private bool sliding = false;
    
    [Header("Camera Controls")]
    [SerializeField] private Transform transformCamerFollow;
    [SerializeField] private float rotateSensivity = 1f;
    private float cameraPitch = 0f;
    private float cameraRoll = 0f;
    [SerializeField] private float maxCameraPitch = 60f;
    [SerializeField] private bool invertCameraPitch = false;
    
    [Header("Character Rotation")]
    [SerializeField] private Transform characterBody;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        
        movingSpeed = defaultSpeed;
        
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        
        
        if (invertCameraPitch)
        {
            cameraPitch = cameraPitch - mouseDelta.y * rotateSensivity;
        }
        else
        {
            cameraPitch = cameraPitch + mouseDelta.y * rotateSensivity;
        }
        
        cameraPitch = Mathf.Clamp(cameraPitch, -maxCameraPitch, maxCameraPitch);
        
        cameraRoll = cameraRoll + mouseDelta.x * rotateSensivity;
        
        transformCamerFollow.localEulerAngles = new Vector3(cameraPitch, cameraRoll, 0);
    }
    
    private void FixedUpdate()
    {
        if (SlopeCheck())
        {
            Vector3 movementDirection = new Vector3(inputVector.x * movingSpeed, rb.velocity.y,
                inputVector.y * movingSpeed);
            
            movementDirection =
                Quaternion.AngleAxis(transformCamerFollow.eulerAngles.y, Vector3.up) * movementDirection;
            
            rb.velocity = movementDirection;
            
            if (movementDirection != Vector3.zero)
            {
                Vector3 lookdirection = movementDirection;
                lookdirection.y = 0f;
                characterBody.rotation = Quaternion.LookRotation(lookdirection);
            }
        }
    }
    
    void OnMove(InputValue inputValue)
    {
        inputVector = inputValue.Get<Vector2>();
    }
    
    void OnSprint(InputValue inputVal)
    {
        float isSprinting = inputVal.Get<float>();
        
        if (Mathf.RoundToInt(isSprinting) == 1)
        {
            movingSpeed = sprintingSpeed;
        }
        else
        {
            movingSpeed = defaultSpeed;
        }
    }
    
    bool GroundCheck()
    {
        return Physics.Raycast(transformRayStart.position, Vector3.down,
            rayLength, layerGroundCheck);
    }
    
    bool SlopeCheck()
    {
        RaycastHit hit;
        
        Debug.DrawRay(transformRayStart.position, Vector3.down * raySlopeLength, Color.red,0.1f);
        
        Physics.Raycast(transformRayStart.position, Vector3.down, out hit,
            raySlopeLength, layerGroundCheck);
        
        if (hit.collider != null)
        {
            float angle = Vector3.Angle(Vector3.up, hit.normal);
            //Debug.Log(angle);
            if (angle > maxAngleSlope)
            {
                return false;
            }
        }
        
        return true;
    }
    
  
}
