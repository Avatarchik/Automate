using UnityEngine;
using System.Collections;

public class EnableOnStart : MonoBehaviour {

    public GameObject InnerRoot;

	// Update is called once per frame
	void OnEnable () {
        InnerRoot.SetActive (true);
	}
}
