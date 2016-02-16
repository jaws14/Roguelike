using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

	public static GameController Instance;
	public bool isPlayerTurn;
	public bool areEnemiesMoving;
	public int playerCurrentHealth = 50;
	public AudioClip gameOverSound;
	private BoardController boardController;
	private List<Enemy> enemies;
	private GameObject levelImage;
	private Text levelText;
	private bool settingUpGame;
	private int secondsUntilLevelStart = 2;
	private int currentLevel = 1;

	void Awake ()
	{
		if (Instance != null && Instance != this) {
			DestroyImmediate (gameObject);
			return;
		}
		Instance = this;
		DontDestroyOnLoad (gameObject);
		boardController = GetComponent<BoardController> ();
		enemies = new List<Enemy> ();
	}
	
	void Start ()
	{
		InitializeGame ();
	}

	private void InitializeGame ()
	{
		settingUpGame = true;
		levelImage = GameObject.Find ("Level Image");
		levelText = GameObject.Find ("Level Text").GetComponent<Text> ();
		levelText.text = "Day " + currentLevel;
		levelImage.SetActive (true);
		enemies.Clear ();
		boardController.setupLevel (currentLevel);
		Invoke ("DisableLevelImage", secondsUntilLevelStart);
	
	}

	private void DisableLevelImage ()
	{
		levelImage.SetActive (false);
		settingUpGame = false;
		isPlayerTurn = true;
		areEnemiesMoving = false;
	}

	private void OnLevelWasLoaded (int levelLoaded)
	{
		currentLevel++;
		InitializeGame ();
	}

	void Update ()
	{
		if (isPlayerTurn || areEnemiesMoving || settingUpGame) {
			return;
		}
		StartCoroutine (MoveEnemies ());
	}

	private IEnumerator MoveEnemies ()
	{
		areEnemiesMoving = true;

		yield return new WaitForSeconds (0.2f);

		foreach (Enemy enemy in enemies) {
			enemy.MoveEnemy ();
			yield return new WaitForSeconds (enemy.moveTime);
		}

		areEnemiesMoving = false;
		isPlayerTurn = true;
	}

	public void AddEnemyToList (Enemy enemy)
	{
		enemies.Add (enemy);
	}

	public void GameOver ()
	{
		isPlayerTurn = false;
		SoundController.Instance.music.Stop ();
		SoundController.Instance.PlaySingle (gameOverSound);
		levelText.text = "You starved after " + currentLevel + " days...";
		levelImage.SetActive (true);
		enabled = false;
	}
}
