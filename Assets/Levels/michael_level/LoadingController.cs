using UnityEngine;
using System.Collections;

public class LoadingController : MonoBehaviour {

	private UISprite loadingLogoSprite;
	
	void Start() {
		loadingLogoSprite = GetComponent<UISprite>();
		enablePauseLogo(false);
	}
	
	void Update() {
		if(Application.isLoadingLevel) {
			enablePauseLogo(true);
		} else {
			enablePauseLogo(false);
		}
	}

	void enablePauseLogo(bool value) {
		if(loadingLogoSprite != null) {
			loadingLogoSprite.enabled = value;
		}
	}
}
