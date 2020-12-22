using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallScript : MonoBehaviour
{
	public GameObject player1, player2;
	public GameObject shadow;
	
	public float powerUpEffectTime = 10.0f;
	
	public static bool canScore = true;
	
	public Text P1ScoreText;
	public Text P2ScoreText;
	
	public static int p1Score = 0;
	public static int p2Score = 0;
	
	private AudioSource ballAS;
	public AudioClip ballBounce, whistle, powerUP, powerDown;
	public AudioClip[] goalScream;
	
	public Animator ballAnim;
	
	private Rigidbody2D rb2D; // Variável que armazena o rigdbody da bolinha
		
	public float kickForce; //Variável que armazena o impulso inicial da bolinha
	
	public Transform initialPosition; // Variável que inicia a posição inicial da bolinha
	
    // Start is called before the first frame update
    void Start()
    {
		rb2D = GetComponent<Rigidbody2D>();
		ballAnim = GetComponent<Animator>();
		ballAS = GetComponent<AudioSource>();
		
		rb2D.isKinematic = true;

		p1Score = 0;
		p2Score = 0;
		
		StartCoroutine(SpawnBall());
		
    }
	
	void Update()
	{
		shadow.transform.position = new Vector3(transform.position.x, -2.8f, 0f);
		shadow.transform.rotation = Quaternion.identity;
	}
	
	IEnumerator SpawnBall()
	{
		yield return new WaitForSeconds(1.0f);
		ballAS.PlayOneShot(whistle);
		if (!GameManager.gameFinished)
		{
			canScore = true;
		}
		transform.position = initialPosition.transform.position;
		rb2D.velocity = Vector2.zero;
		rb2D.angularVelocity = 0.0f;
		rb2D.isKinematic = true;
		yield return new WaitForSeconds(2.0f);
		rb2D.isKinematic = false;
		float angle = Random.Range (-0.75f, 0.75f);
		Vector2 initialAngle = new Vector2(angle, 1);
		rb2D.AddForce(initialAngle * kickForce);
	}
	
	private void OnCollisionEnter2D(Collision2D collision)
	{
		ballAS.PlayOneShot(ballBounce);
		if(collision.gameObject.CompareTag("P1"))
		{
			gameObject.tag = "P1";
		}
		
		if(collision.gameObject.CompareTag("P2"))
		{
			gameObject.tag = "P2";
		}
		
	}
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Goal1") && canScore)
		{
			//ballAnim.SetTrigger("FadeOut");	
			p2Score++;
			SetScore(p2Score, P2ScoreText);
			StartCoroutine(SpawnBall());
		}
		
		if (collision.CompareTag("Goal2")&& canScore)
		{
			//ballAnim.SetTrigger("FadeOut");
			p1Score++;
			SetScore(p1Score, P1ScoreText);
			StartCoroutine(SpawnBall());
		}
		
		//PowerUps
		if(collision.CompareTag("cantJump"))
		{
			if (gameObject.tag ==  "P1")
			{
				StartCoroutine(CantJump(player2));
				Destroy(collision.gameObject);
			}
			if (gameObject.tag ==  "P2")
			{
				StartCoroutine(CantJump(player1));
				Destroy(collision.gameObject);
			}
		}
		
		if(collision.CompareTag("slowDown"))
		{
			if (gameObject.tag ==  "P1")
			{
				StartCoroutine(SlowDown(player2));
				Destroy(collision.gameObject);
			}
			if (gameObject.tag ==  "P2")
			{
				StartCoroutine(SlowDown(player1));
				Destroy(collision.gameObject);
			}
		}
		
		if(collision.CompareTag("grownUp"))
		{
			if (gameObject.tag ==  "P1")
			{
				StartCoroutine(GrownUp(player1));
				Destroy(collision.gameObject);
			}
			if (gameObject.tag ==  "P2")
			{
				StartCoroutine(GrownUp(player2));
				Destroy(collision.gameObject);
			}
		}
		
	}
	
	void SetScore(int score, Text scoreText)
	{
		int i = Random.Range(0, goalScream.Length);
		ballAS.PlayOneShot(goalScream[i]);
		canScore = false;
		scoreText.text = score.ToString();		
	}
	
	IEnumerator CantJump(GameObject player)
	{
		ballAS.PlayOneShot(powerDown);
		player.GetComponent<PlayerScript>().canJump = false;
		player.GetComponent<PlayerScript>().cantJumpMarker.SetActive(true);
		yield return new WaitForSeconds(powerUpEffectTime);
		player.GetComponent<PlayerScript>().canJump = true;
		player.GetComponent<PlayerScript>().cantJumpMarker.SetActive(false);
	}
	
	IEnumerator GrownUp(GameObject player)
	{
		ballAS.PlayOneShot(powerUP);
		player.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
		yield return new WaitForSeconds(powerUpEffectTime);
		player.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
	}
	
	IEnumerator SlowDown(GameObject player)
	{
		ballAS.PlayOneShot(powerDown);
		player.GetComponent<PlayerScript>().playerMoveSpeed *= 0.5f;
		player.GetComponent<PlayerScript>().slowDownMarker.SetActive(true);
		yield return new WaitForSeconds(powerUpEffectTime);
		player.GetComponent<PlayerScript>().playerMoveSpeed *= 2f;
		player.GetComponent<PlayerScript>().slowDownMarker.SetActive(false);
	}
	
}
