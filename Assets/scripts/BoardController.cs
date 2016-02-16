using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardController : MonoBehaviour
{

	public int columns;
	public int rows;
	public GameObject[] floors;
	public GameObject[] outerWalls;
	public GameObject[] wallObstacles;
	public GameObject[] foodItems;
	public GameObject[] enemies;
	public GameObject exit;
	private Transform gameBoard;
	private List<Vector3> obstaclesGrid;

	void Awake ()
	{
		obstaclesGrid = new List<Vector3> ();
	}

	void Update ()
	{
	
	}

	private void InitializeObstaclePositions ()
	{
		obstaclesGrid.Clear ();
		for (int x = 2; x<columns - 2; x++) {
			for (int y = 2; y<rows - 2; y++) {
				obstaclesGrid.Add (new Vector3 (x, y, 0f));
			}
		}
	}

	private void setupGameBoard ()
	{
		gameBoard = new GameObject ("Game Board").transform;

		for (int x = 0; x < columns; x++) {
			for (int y = 0; y < rows; y++) {
				GameObject selectedTile;
				if (x == 0 || y == 0 || x == columns - 1 || y == rows - 1) {
					selectedTile = outerWalls [Random.Range (0, outerWalls.Length)];
				} else {
					selectedTile = floors [Random.Range (0, floors.Length)];
				}
				GameObject floorTile = (GameObject)Instantiate (selectedTile, new Vector3 (x, y, 0f), Quaternion.identity);
				floorTile.transform.SetParent (gameBoard);
			}
		}
	}

	private void SetRandomObstaclesOnGrid (GameObject[] obstaclesArray, int minimum, int maximum)
	{
		int obstacleCount = Random.Range (minimum, maximum + 1);
		if (obstacleCount > obstaclesGrid.Count) {
			obstacleCount = obstaclesGrid.Count;
		}

		for (int index = 0; index < obstacleCount; index++) {
			//Generate a random position an use it to generat a random obstacle tile

			GameObject selectedObstacle = obstaclesArray [Random.Range (0, obstaclesArray.Length)];
			Instantiate (selectedObstacle, SelectGridPosition (), Quaternion.identity);

		}
	}

	private Vector3 SelectGridPosition ()
	{
		int randomIndex = Random.Range (0, obstaclesGrid.Count);
		Vector3 randomPosition = obstaclesGrid [randomIndex];
		obstaclesGrid.RemoveAt (randomIndex);
		return randomPosition;
	}

	public void setupLevel (int currentLevel)
	{
		InitializeObstaclePositions ();
		setupGameBoard ();
		SetRandomObstaclesOnGrid (wallObstacles, 3, 9);
		SetRandomObstaclesOnGrid (foodItems, 1, 5);
		int enemyCount = (int)Mathf.Log (currentLevel, 2);
		SetRandomObstaclesOnGrid (enemies, enemyCount, enemyCount);
		Instantiate (exit, new Vector3 (columns - 2, rows - 2, 0f), Quaternion.identity);
	}
}
































































