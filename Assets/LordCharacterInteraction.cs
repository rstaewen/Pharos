using UnityEngine;
using System.Collections;
//using Scripts.CSharp.ControlFire;

public class LordCharacterInteraction : MonoBehaviour {

	//ControlFire csobj;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	//	csobj = GetComponent<instantiatedObjects> ();

	}

	void OnTriggerEnter (Collider other) {
		try{
			if (other.gameObject.tag == "Player") {print ("Player touched the statue!");
						foreach (GameObject fire_instance in ControlFire.instantiatedObjects) {
					print ("in for loop");
				//print ("destroying object: "+fire_instance.name);
					if(fire_instance != null)
				Destroy (fire_instance);
				}print ("out for loop");
				}
		}catch(UnityException e){print("Exception here LordCharacterInteraction"+e);}
	}


}
