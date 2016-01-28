using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

	public static Instance;

	void Awake () {
		if (instance != null && instance != this) {
			{
				Destroy(gameObject); 
				return;
			}

		Instance = this;
		DontDestroyOnLoad (gameObject);
	}
	

	void Update () {
	
	}
}
