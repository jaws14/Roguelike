using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MovingObject
{

	public Text healthText;
	public AudioClip movementSound1;
	public AudioClip movementSound2;
	public AudioClip chopSound1;
	public AudioClip chopSound2;
	public AudioClip fruitSound1;
	public AudioClip fruitSound2;
	public AudioClip sodaSound1;
	public AudioClip sodaSound2;
	private int attackPower = 1;
	private int playerHealth;
	private Animator animator;
	private int healthPerFruit = 5;
	private int healthPerSoda = 10;
	private int secondsUntilNextLevel = 1;

	protected override void Start ()
	{
		base.Start ();
		animator = GetComponent<Animator> ();
		playerHealth = GameController.Instance.playerCurrentHealth;
		healthText.text = "Health: " + playerHealth;
	}

	private void OnDisable ()
	{
		GameController.Instance.playerCurrentHealth = playerHealth;
	}

	void Update ()
	{
	
		if (!GameController.Instance.isPlayerTurn) {
			return;
		}
		CheckIfGameOver ();
		int xAxis = 0;
		int yAxis = 0;

		xAxis = (int)Input.GetAxisRaw ("Horizontal");
		yAxis = (int)Input.GetAxisRaw ("Vertical");

		if (xAxis != 0) {
			yAxis = 0;
		}


		if (xAxis != 0 || yAxis != 0) {
			playerHealth--;
			healthText.text = "Health: " + playerHealth;
			SoundController.Instance.PlaySingle (movementSound1, movementSound2);
			Move<Wall> (xAxis, yAxis);

			GameController.Instance.isPlayerTurn = false;
		}
	}

	private void OnTriggerEnter2D (Collider2D objectPlayerCollideWith)
	{
		if (objectPlayerCollideWith.tag == "Exit") {
			Invoke ("LoadNewLevel", secondsUntilNextLevel);
			enabled = false;
		} else if (objectPlayerCollideWith.tag == "Fruit") {
			playerHealth += healthPerFruit;
			healthText.text = "+" + healthPerFruit + " Health\n" + "Health: " + playerHealth;
			objectPlayerCollideWith.gameObject.SetActive (false);
			SoundController.Instance.PlaySingle (fruitSound1, fruitSound2);
		} else if (objectPlayerCollideWith.tag == "Soda") {
			playerHealth += healthPerSoda;
			healthText.text = "+" + healthPerSoda + " Health\n" + "Health: " + playerHealth;
			objectPlayerCollideWith.gameObject.SetActive (false);
			SoundController.Instance.PlaySingle (sodaSound1, sodaSound2);
		}

	}

	private void LoadNewLevel ()
	{
		Application.LoadLevel (Application.loadedLevel);
	}

	protected override void HandleCollision<T> (T component)
	{
		Wall wall = component as Wall;
		animator.SetTrigger ("playerAttack");
		SoundController.Instance.PlaySingle (chopSound1, chopSound2);
		wall.DamageWall (attackPower);
	}

	public void TakeDamage (int damageReceived)
	{
		playerHealth -= damageReceived;
		healthText.text = "-" + damageReceived + " Health\n" + "Health: " + playerHealth;
		animator.SetTrigger ("playerHurt");
	}

	private void CheckIfGameOver ()
	{
		if (playerHealth <= 0) {
			GameController.Instance.GameOver ();
		}
	}
}
