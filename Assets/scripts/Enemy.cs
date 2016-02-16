using UnityEngine;
using System.Collections;

public class Enemy : MovingObject
{
	public bool isEnemyStrong;
	public int attackDamage;
	public AudioClip enemySound1;
	public AudioClip enemySound2;
	private bool skipCurrentMove;
	private Transform player;
	private Animator animator;

	protected override void Start ()
	{
		GameController.Instance.AddEnemyToList (this);
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		skipCurrentMove = true;
		animator = GetComponent<Animator> ();
		base.Start ();
	}

	public void MoveEnemy ()
	{

		if (skipCurrentMove) {
			if (isEnemyStrong) {
				int chanceToMove = Random.Range (1, 4);
				if (chanceToMove > 1) {
					skipCurrentMove = false;
					return;
				}
			} else {
				skipCurrentMove = false;
				return;
			}
		}
		int xAxis = 0;
		int yAxis = 0;

		float xAxisDistance = Mathf.Abs (player.position.x - transform.position.x);
		float yAxisDistance = Mathf.Abs (player.position.y - transform.position.y);

		if (xAxisDistance > yAxisDistance) {
			if (player.position.x > transform.position.x) {
				xAxis = 1;
			} else {
				xAxis = -1;
			}
		} else {
			if (player.position.y > transform.position.y) {
				yAxis = 1;
			} else {
				yAxis = -1;
			}
		}
		Move<Player> (xAxis, yAxis);
		skipCurrentMove = true;
	}

	protected override void HandleCollision<T> (T component)
	{
		Player player = component as Player;
		player.TakeDamage (attackDamage);
		animator.SetTrigger ("enemyAttack");
		SoundController.Instance.PlaySingle (enemySound1, enemySound2);
	}
}

