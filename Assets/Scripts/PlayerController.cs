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

    [SerializeField] int damage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDistance;

    bool isShooting;
    private int timesJumped;
    Vector3 playerVelocity;


    // Update is called once per frame
    void Update()
    {
        playerMove();
        StartCoroutine(shoot());
    }

    void playerMove()
    {
        if (playerController.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            timesJumped = 0;
        }

        float xAxis = Input.GetAxis("Horizontal");
        float zAxis = Input.GetAxis("Vertical");

        Vector3 move = transform.right * xAxis + transform.forward * zAxis;
        playerController.Move(move * playerSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && timesJumped < jumpsMax)
        {
            timesJumped++;
            playerVelocity.y += jumpHeight;
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        playerController.Move(playerVelocity * Time.deltaTime);
    }

    IEnumerator shoot()
    {
        if(Input.GetButton("Shoot") && !isShooting)
        {
            isShooting = true;
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDistance))
            {
                if (hit.collider.GetComponent<IDamage>() != null)
                    hit.collider.GetComponent<IDamage>().takeDamage(damage);
            }
            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
    }
}
