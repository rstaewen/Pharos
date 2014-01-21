using UnityEngine;
using System.Collections;

public class sceneTrigger : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		Application.LoadLevel("mirrorRoom");


	}
}