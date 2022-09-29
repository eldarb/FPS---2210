using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CharacterController playerController;

    [SerializeField] float playerSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float gravityValue;

    [SerializeField] int jumpsMax;

    
    
    private int timesJumped;
    Vector3 playerVelocity;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerMove();
        playerJump();
    }

    void playerMove()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float zAxis = Input.GetAxis("Vertical");

        Vector3 move = transform.right * xAxis + transform.forward * zAxis;
        playerController.Move(move * playerSpeed * Time.deltaTime);
    }

    void playerJump()
    {
        if (playerController.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            timesJumped = 0;
        }

        if (Input.GetButtonDown("Jump") && timesJumped < jumpsMax)
        {
            timesJumped++;
            playerVelocity.y += jumpHeight;
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        playerController.Move(playerVelocity * Time.deltaTime);
    }
}
