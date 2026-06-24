using System;
using System.Collections;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float rotationSpeed;
    private Rigidbody2D rb;
    private Vector2 movementInput;
    private Vector2 lookInput;
    private Vector2 smoothMovementInput;
    private Vector2 movementInputSmoothVelo; 
    private Camera _camera;

    [SerializeField] private float dashSpeed;

    public float dashLength = .5f, dashCooldown = 1f;
    private float currentSpeed;

    [SerializeField] private TrailRenderer tr;

    [SerializeField] private float screenBorder;
    private bool isDashing;
    private float dashDuration = 0.5f;
    private bool canDash;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
        currentSpeed = speed;
        canDash = true;
    }

    private void FixedUpdate()
    {
        SetPlayerVelocity();
        RotateInDirectionOfLook();
        //RotateInDirectionOfInput();
    }

    private void SetPlayerVelocity()
    {
        if (isDashing)
        return;
        //smoothMovementInput = Vector2.SmoothDamp(
        smoothMovementInput = Vector2.SmoothDamp(
            smoothMovementInput,
            movementInput,
            ref movementInputSmoothVelo, 0.1f);

        rb.linearVelocity = smoothMovementInput * currentSpeed;

        //PreventPlayerGoingOffScreen();
    }
/*
    private void PreventPlayerGoingOffScreen()
    {
        Vector2 screenPosition = camera.WorldToScreenPoint(transform.position);

        if ((screenPosition.x < screenBorder && rb.linearVelocityX < 0) || (screenPosition.x > camera.pixelWidth - screenBorder && rb.linearVelocity.x > 0))
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
        }

        if ((screenPosition.y < screenBorder && rb.linearVelocityY < 0) || (screenPosition.y > camera.pixelHeight - screenBorder && rb.linearVelocity.y > 0))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, 0);
        }
    }
*/
    private void RotateInDirectionOfLook()
    {
        //Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //transform.up = mousePos = new Vector2(transform.position.x, transform.position.y);
        /* 
        if (lookInput != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(lookInput.y, lookInput.x) * Mathf.Rad2Deg - 90f;

            float angle = Mathf.MoveTowardsAngle(
                rb.rotation,
                targetAngle,
                rotationSpeed * Time.fixedDeltaTime);

            rb.MoveRotation(angle);
        }
        */
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 direction = mousePos - (Vector2)transform.position;

            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

            rb.MoveRotation(targetAngle);
    }

//This is, as it's titled here, based off direction of input. I'd like it to be based off arrow keys.
    private void RotateInDirectionOfInput()
    {
        if (movementInput != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, smoothMovementInput);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            rb.MoveRotation(rotation);
        }
    }
    private void OnMove(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();
    }

    private void OnLook(InputValue inputValue)
    {
        lookInput = inputValue.Get<Vector2>();
    }


    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(
        Mouse.current.position.ReadValue());

        Vector2 dashDirection =
        (mousePos - (Vector2)transform.position).normalized;


        if (dashDirection == Vector2.zero)
            dashDirection = transform.up;

        rb.linearVelocity = dashDirection * dashSpeed;
        Debug.Log("Dash pressed");
        Debug.Log("Direction: " + dashDirection);

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    public void OnDash(InputValue inputValue)
{
    if (canDash)
    {
        tr.enabled = true;
        StartCoroutine(Dash());
        tr.enabled = false;
    }
}
}
