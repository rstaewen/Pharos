using UnityEngine;
using System.Collections;

public class LevelSwitch : MonoBehaviour {

	void OnTriggerEnter(Collider other){

		Application.LoadLevel ("mirrorRoom");}
}
