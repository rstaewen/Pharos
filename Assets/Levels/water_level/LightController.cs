using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour {
	public GameObject[] Lights; 
	private bool[] CheckArray;
	private float starttimer;
	private float currenttime;
	private float allowedtime = 120f;
	private Color startcolor;
	private bool started = false;
	void Start () {
		starttimer = Time.time;
		Debug.Log (starttimer);
		CheckArray = new bool[Lights.Length];
		InitializeCheckArray ();
		Light mylight;
		mylight = Lights[0].GetComponent<Light> ();
		startcolor = mylight.color;
	}

	void Update () {
		currenttime = Time.time;
		if (((currenttime - starttimer) > allowedtime)&& started == true) {
			ResetLights();	

		}

		
	}
	void ResetLights (){
		InitializeCheckArray ();
		for (int i =0; i < Lights.Length; i++) {
			Light mylight;
			mylight = Lights[i].GetComponent<Light> ();	
			mylight.color = startcolor;
		}
		started = false;

	}
	void InitializeCheckArray(){
		for (int i=0; i<CheckArray.Length; i++) {
			CheckArray[i]=false;
		}
	}
	void Change_Color(GameObject light){
		Light mylight;
		mylight = light.GetComponent<Light> ();
		//mylight.enabled = !mylight.enabled;
		mylight.color = Color.white;

	}
	public bool Check ( int index){
		Debug.Log ("Check Entered");
		if (index == 0) {
			starttimer = Time.time;		
			started = true;
		}
		if (index == 0 || CheckArray [index - 1] == true) {
			Change_Color(Lights[index]);
			CheckArray[index] = true;
			return true;
		}

		return false;
	}
}
