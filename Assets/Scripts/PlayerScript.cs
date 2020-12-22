using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
	private Rigidbody2D rb2d;
	public Rigidbody2D footRB;
	
	public bool canJump = true;
	
	public GameObject shadow, cantJumpMarker, slowDownMarker;
	
	public float playerMoveSpeed;
	public float jumpForce;
	public float kickForce;
	
	public string horizontalAxisName;
	public string jumpButtonName;
	public string kickButtonName;
	

	
	
	public LayerMask ground;
	
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D> ();
    }

    // Update is called once per frame
    void Update()
    {
		bool isGrounded = rb2d.IsTouchingLayers(ground);
		
		if (Input.GetButtonDown(jumpButtonName) && isGrounded && canJump)
		{
			rb2d.AddForce(transform.up * jumpForce);
		}
		if (Input.GetButtonDown(kickButtonName))
		{
			footRB.AddTorque(kickForce);
		}
		shadow.transform.position = new Vector3(transform.position.x, -2.8f, 0f);
		shadow.transform.rotation = Quaternion.identity;
    }
	
	void FixedUpdate()
	{
		
		float h = Input.GetAxis (horizontalAxisName);
		
		rb2d.velocity = new Vector2 (playerMoveSpeed * h, rb2d.velocity.y);
		
	}
}
