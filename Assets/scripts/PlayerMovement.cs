using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 1000f;
    
    private CharacterController controller;
    private Transform modelTransform;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        modelTransform = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput= Input.GetAxis("Vertical");
        
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
        float magnitude = Mathf.Clamp01(direction.magnitude) * speed;
        direction.Normalize();

        controller.SimpleMove(direction * magnitude);
        animator.SetFloat("speed", magnitude);

        if (direction != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            modelTransform.rotation = Quaternion.RotateTowards( modelTransform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
    }
}
