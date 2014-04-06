using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlFire : MonoBehaviour {

	public GameObject target;
	public int count;

	// Use this for initialization
	void Start () {
		count = 0;
		//for (int i = 0;i < 10; i++) {
		//instantiatedObjects.Clear ();
		InvokeRepeating("spawnobject",2,3600);
		//}
		//CancelInvoke ("spawnobject");
	}

	[HideInInspector] public static List<GameObject> instantiatedObjects = new List<GameObject>();
	// Update is called once per frame
	void spawnobject() {
		float x = Random.Range(408.0f,456.0f);
		float z = Random.Range(182.0f,291.0f);

		//target.transform.Rotate (new Vector3 (270, 0, 0));//* Time.deltaTime);
		GameObject fire_instance = GameObject.Instantiate (target, new Vector3 (x, 19, z), Quaternion.identity) as GameObject;


		instantiatedObjects.Add (fire_instance);

		//count = count + 1;

	}



	void update(){
		//count = count + 1;
		//if (count > 15) {
		//	CancelInvoke ("spawnobject");
		//}
	}
}
