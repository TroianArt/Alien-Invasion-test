using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Joystick variableJoystick;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform playerBody;
    [SerializeField] private Animator animator;
    
    public void FixedUpdate()
    {
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        gameObject.transform.position += direction * speed * Time.fixedDeltaTime;
        playerBody.LookAt(playerBody.transform.position + direction);
        animator.SetFloat("Speed_f", direction.magnitude * speed);
    }
}