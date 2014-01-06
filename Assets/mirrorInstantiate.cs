using UnityEngine;
using System.Collections;

public class mirrorInstantiate : MonoBehaviour {
	
	public GameObject mirrorSpawn;
	public GameObject mirror;
	public GameObject camera;
	//public AudioClip puzzleComplete;
	private GameObject Pillar1;
	private GameObject Pillar2;
	private GameObject Pillar3;
	private bool instantiated = false;
	
	
	private RotatablePillar RotatablePillar1;
	private RotatablePillar RotatablePillar2;
	private RotatablePillar RotatablePillar3;
	
	
	
	
	
	
	
	

	// Use this for initialization
	void Start () {
		Pillar1=GameObject.FindGameObjectWithTag("pillar1");
		Pillar2=GameObject.FindGameObjectWithTag("pillar2");
		Pillar3=GameObject.FindGameObjectWithTag("pillar3");
		RotatablePillar1 = (RotatablePillar)Pillar1.gameObject.GetComponent("RotatablePillar");
		RotatablePillar2 = (RotatablePillar)Pillar2.gameObject.GetComponent("RotatablePillar");
		RotatablePillar3 = (RotatablePillar)Pillar3.gameObject.GetComponent("RotatablePillar");
			
			
	}
	
	// Update is called once per frame
	void Update () {
		//2 3 1
		int one = RotatablePillar1.rotationCount();
		
		int two = RotatablePillar2.rotationCount();
		
		int three = RotatablePillar3.rotationCount();
		
		if (one == 2 && two == 3 && three == 1 && instantiated == false)
		{
			
			Instantiate(mirror,mirrorSpawn.transform.position, Quaternion.identity);
			instantiated = true;
			
			//audio.PlayOneShot(puzzleComplete);
			
			Instantiate(camera,camera.transform.position, camera.transform.rotation);
			
			
		}
		
	}
}
