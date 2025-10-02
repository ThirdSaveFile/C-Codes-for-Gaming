using Unity.VisualScripting;
using UnityEngine;

public class MoveGodDanmitt : MonoBehaviour
{
    private CharacterController characterController;
    [SerializeField] float speed = 5;
    [SerializeField] float jumpSpeed;
    [SerializeField] float teleport = 10;
    [SerializeField] float gravitySpeed;
    [SerializeField] float rotationSpeed = 10;
    public bool isSprinting = false;
    [SerializeField] float sprintMultiplier;
    private float originalStepOffset;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;       
    }
    void Update()
    {
        gravitySpeed += Physics.gravity.y * Time.deltaTime;

        float horizontalInput = Input.GetAxis("Horizontal");    //get control to walk horizontaly "A,D,left and right"
        float verticalInput = Input.GetAxis("Vertical");        //Get controls to walk verticaly "W,S,up and down"
        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
        float magnitude = Mathf.Clamp01(moveDirection.magnitude) * speed;
        Vector3 velocity = moveDirection * magnitude;
        velocity.y = gravitySpeed;
        characterController.Move(velocity * Time.deltaTime);
        moveDirection.Normalize();

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            characterController.enabled = false;
            transform.position += transform.forward * teleport;
            characterController.enabled = true;
        }

        if (characterController.isGrounded)
        {
            gravitySpeed = -0.5f;
            if (Input.GetButtonDown("Jump"))
            {
                characterController.stepOffset = originalStepOffset;
                gravitySpeed = jumpSpeed;
            }
        }
        else
        {
            characterController.stepOffset = 0;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;
            speed = 15;
        }
        else
        {
            isSprinting = false;
            speed = 5;
        }

        
        
        
    }
}
