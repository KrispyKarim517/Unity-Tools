using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float sprintSpeed = 20.0f;
    public float jumpHeight = 2.0f;
    public float mouseSensitivity = 2.0f;

    private float verticalLookRotation;
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float currentMoveSpeed = moveSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentMoveSpeed = sprintSpeed;
        }

        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput) * currentMoveSpeed;
        characterController.SimpleMove(moveDirection);

        if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded)
        {
            characterController.Move(Vector3.up * jumpHeight);
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);
        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90, 90);
        Camera.main.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }
}
