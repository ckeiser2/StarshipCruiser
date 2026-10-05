using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerMovement : MonoBehaviour
{
    [SerializeField] 
    private float movementSpeed = 5f;

    private Vector2 movementInput;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ReadMovementInput();
        Move();
    }

    private void ReadMovementInput()
    {
        float horizontalInput = Keyboard.current.dKey.ReadValue() - Keyboard.current.aKey.ReadValue();
        float verticalInput = Keyboard.current.wKey.ReadValue() - Keyboard.current.sKey.ReadValue();
        movementInput = new Vector2(horizontalInput, verticalInput);
        movementInput = Vector2.ClampMagnitude(movementInput, maxLength:1f);
    }

    private void Move()
    {
        Vector3 movementDirection = new Vector3(movementInput.x, movementInput.y, 0f);
        transform.position += movementDirection * movementSpeed * Time.deltaTime;
        
        
    }
}
