using Core.Server;
using UnityEngine;
using System.Collections;

public class AddTopBar : MonoBehaviour {
    public GameObject realTopBar;
    public float realTopBarHeight;
    public GameObject funTopBar;
    public float funTopBarHeight;

	void Awake () {
	    GameObject prefab = SessionData.Instance.IsFun ? funTopBar : realTopBar;
        float height = SessionData.Instance.IsFun ? funTopBarHeight : realTopBarHeight;

        var topBar = NGUITools.AddChild(gameObject, prefab);
        UIWidget widget = topBar.GetComponent<UIWidget>();
	    widget.SetAnchor(gameObject);
        widget.bottomAnchor.Set(1f, -height);
        widget.UpdateAnchors();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
