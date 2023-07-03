using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float modelRotationSpeed = 1000f;
    public float verticalSensitivity = 1000f;
    public float horizontalSensitivity = 1000f;


    private CharacterController _controller;
    private Transform _modelTransform;
    private Camera _playerCamera;

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        _modelTransform = transform.GetChild(0);
        _playerCamera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        FpsMove();
        FpsCamera();
    }

    void TpsMove()
    {
        float horizontalMovementInput = Input.GetAxis("Horizontal");
        float verticalMovementInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalMovementInput, 0, verticalMovementInput);
        movementDirection = _playerCamera.transform.TransformDirection(movementDirection);
        float movementMagnitude = Mathf.Clamp01(movementDirection.magnitude) * speed;
        movementDirection.Normalize();

        _controller.SimpleMove(movementDirection * movementMagnitude);
        _animator.SetFloat("speed", movementMagnitude);

        if (movementDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            _modelTransform.rotation =
                Quaternion.RotateTowards(_modelTransform.rotation, rotation, Time.deltaTime * modelRotationSpeed);
        }
    }

    void FpsMove()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float horizontalMovementInput = Input.GetAxis("Horizontal");
        float verticalMovementInput = Input.GetAxis("Vertical");


        Vector3 moveDirection = Vector3.zero;

        moveDirection = (forward * (verticalMovementInput * speed)) + (right * (horizontalMovementInput * speed));

        _controller.SimpleMove(moveDirection);
        _animator.SetFloat("speed", Math.Abs(verticalMovementInput + horizontalMovementInput));
    }

    void FpsCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * horizontalSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * verticalSensitivity;
        
        if (_playerCamera.transform.rotation.eulerAngles.x - mouseY > 90 && _playerCamera.transform.rotation.eulerAngles.x - mouseY < 270)
        {
            mouseY = 0;
        }
        _playerCamera.transform.Rotate(Vector3.left * mouseY);
        transform.Rotate(Vector3.up * mouseX);
    }

}