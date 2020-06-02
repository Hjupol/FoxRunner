using System;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;

    float currentSpeed = 20f;
    float minSpeed;
    float maxSpeed = 60f;
    public float accelerationTime = 70f;
    float time;
    public float sideSpeed;
    public float jumpForce = 10;
    public float rollForce = 50;

    private bool rollExecuted=false;
    private bool isOnFloor;

    public GameObject leftText;
    public GameObject rightText;

    float[] positions;

    [SerializeField]
    PlayerPosition currentPosition;

    private void Awake()
    {
        SwipeDetector.OnSwipe += InputSwipe;
    }

    private void InputSwipe(SwipeData data)
    {
        switch (data.Direction)
        {
            case SwipeDirection.Up:
                Jump();
                break;
            case SwipeDirection.Down:
                Roll();
                break;
            case SwipeDirection.Left:
                MoveToExactPosition(ScreenPosition.Left);
                break;
            case SwipeDirection.Right:
                MoveToExactPosition(ScreenPosition.Right);
                break;
            default:
                break;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        time = 0;
        minSpeed = currentSpeed;

        float leftPositionX = -5f;
        float middlePositionX = 0f;
        float rightPositionX = 5f;

        positions = new float[] { leftPositionX, middlePositionX, rightPositionX };

        SetInitialPosition();


    }

    void Update()
    {
        IncreasePlayerSpeed();
        InputManager();
    }

    void InputManager()
    {
        if(Input.GetKeyDown(KeyCode.A) )
        {
            MoveToExactPosition(ScreenPosition.Left);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveToExactPosition(ScreenPosition.Right);
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Roll();
        }
    }

  

    void SetInitialPosition()
    {
        currentPosition = PlayerPosition.Middle;

        transform.position = new Vector3(positions[(int)currentPosition], transform.position.y, transform.position.z);
    }

    public void Jump()
    {
        if (rb != null)
        {
            if (isOnFloor)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                rollExecuted = false;
            }
        }
    }

    public void Roll()
    {
        if (rb != null)
        {
            if (!isOnFloor && !rollExecuted)
            {
                rb.AddForce(Vector3.down * rollForce, ForceMode.Impulse);
                rollExecuted = true;
            }
        }
    }

    public void MoveToExactPosition(ScreenPosition screenPosition)
    {
        if (screenPosition == ScreenPosition.Left)
        {
            switch (currentPosition)
            {
                case PlayerPosition.Left:
                    currentPosition = PlayerPosition.Left;
                    break;
                case PlayerPosition.Middle:
                    currentPosition = PlayerPosition.Left;
                    break;
                case PlayerPosition.Right:
                    currentPosition = PlayerPosition.Middle;
                    break;
            }
        }
        else if (screenPosition == ScreenPosition.Right)
        {
            switch (currentPosition)
            {
                case PlayerPosition.Left:
                    currentPosition = PlayerPosition.Middle;
                    break;
                case PlayerPosition.Middle:
                    currentPosition = PlayerPosition.Right;
                    break;
                case PlayerPosition.Right:
                    currentPosition = PlayerPosition.Right;
                    break;
            }
        }
        if (this != null)
        {
            transform.DOMoveX(positions[(int)currentPosition], sideSpeed * Time.deltaTime);
        }
    }

    void IncreasePlayerSpeed()
    {
        currentSpeed = Mathf.SmoothStep(minSpeed, maxSpeed, time / accelerationTime);
        transform.position += transform.forward * currentSpeed * Time.deltaTime;
        time += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Plataform"))
        {
            rb.velocity = Vector3.zero;
            currentSpeed = 0;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            Restart.pause = true;
        }

        if (collision.gameObject.CompareTag("Plataform"))
        {
            isOnFloor = true;
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Plataform"))
        {
            isOnFloor = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Plataform"))
        {
            isOnFloor = false;
        }
    }

}

public enum ScreenPosition
{
    Left,
    Right
}

enum PlayerPosition
{
    Left,
    Middle,
    Right
}