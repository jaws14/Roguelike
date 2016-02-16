using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour
{

	public int hitPoints = 2;
	public Sprite damagedWallSprite;
	private SpriteRenderer spriteRenderer;

	void Start ()
	{
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	public void DamageWall (int damageReceived)
	{
		hitPoints -= damageReceived;
		spriteRenderer.sprite = damagedWallSprite;
		if (hitPoints <= 0) {
			gameObject.SetActive (false);
		}
	}
	


}
