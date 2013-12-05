using UnityEngine;
using System.Collections.Generic;

public class ResetTool : MonoBehaviour
{
	private LevelSettings levelSettings;
	void Reset ()
	{
		print("reset?");
		levelSettings = GetComponent<LevelSettings>();
		levelSettings.Set();
	}
}
