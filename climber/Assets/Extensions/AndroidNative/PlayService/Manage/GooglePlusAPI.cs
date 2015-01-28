using UnityEngine;
using System.Collections;

public class GooglePlusAPI : SA_Singleton<GooglePlusAPI> {


	void Awake() {
		DontDestroyOnLoad(gameObject);
	}


	

	public void clearDefaultAccount() {
		if (!GooglePlayConnection.CheckState ()) { return; }

		AN_GMSGeneralProxy.clearDefaultAccount();
		GooglePlayConnection.instance.disconnect();
	}
}
