using UnityEngine;
using System.Collections;

public class InfoController : MonoBehaviour {

    int currentPage = 0;

    public SpriteRenderer[] Pages;

    void ResetPages () {
        foreach (var p in Pages) { 
            p.enabled = false;
        }
    }
	
    void OnEnable() { 
        ResetPages ();
        Pages [0].enabled = true;
    }

	void UpdatePages () {
        ResetPages ();
        Pages [currentPage].enabled = true;
	}

    public void Prev()  {
        currentPage--;

        if (currentPage < 0)
            currentPage = Pages.Length - 1;

        UpdatePages ();
    }

    public void Next() { 
        currentPage++;

        if (currentPage > Pages.Length - 1)
            currentPage = 0;

        UpdatePages ();
    }

    public void Exit() { 
        ResetPages ();
        currentPage = 0;
        gameObject.SetActive (false);
    }

    public void SpinMessage() { 
        Exit ();
    }
}
