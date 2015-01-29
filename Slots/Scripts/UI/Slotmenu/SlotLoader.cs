using UnityEngine;
using System.Collections;
using UnitySlot;

public class SlotLoader : MonoBehaviour {
    AsyncOperation slotMenuLoading;
    bool slotMenuLoaded;
    AsyncOperation slotLoading;
    bool slotLoaded;

    // Use this for initialization
    void Start () {
        Debug.Log ("Start loading scenes");
        Application.LoadLevel ("loading");

        StartCoroutine (Load ());
        StartCoroutine (WaitLoading ());
    }

    IEnumerator WaitLoading () {
        while (!slotMenuLoaded) {
            yield return new WaitForFixedUpdate ();
        }

        Debug.Log (string.Format ("sml - {0}", slotMenuLoaded));

        slotMenuLoading.allowSceneActivation = true;

        yield return new WaitForSeconds (.1f);

        while (!slotLoaded) {
            yield return new WaitForFixedUpdate ();
        }

        Debug.Log (string.Format (" sl - {0}", slotLoaded));
        slotLoading.allowSceneActivation = true;

        yield return new WaitForFixedUpdate ();
    }

    IEnumerator Load () {
        yield return new WaitForSeconds (.21f);
        //Debug.Log ("Loading scene..."); 
        var sm = GetComponent<SlotMenuParams> ();

        slotMenuLoading = Application.LoadLevelAsync ("slotMenu");
        if (slotMenuLoading != null) { 
            slotMenuLoading.allowSceneActivation = false;     
            while (!slotMenuLoading.isDone) {
                Debug.Log ("Loading slotmenu precent - " + Mathf.RoundToInt (slotMenuLoading.progress * 100f).ToString () + "%");   
                if (slotMenuLoading.progress == 0.9f) {
                    //освобождаем память
                    Resources.UnloadUnusedAssets ();
                    Debug.Log ("Loading slotmenu scene...ok");
                    slotMenuLoaded = true;
                    slotMenuLoading.allowSceneActivation = true;
                    break;
                }
                yield return 0; 
            }
        }
        StartCoroutine (LoadSlot ());
    }

    IEnumerator LoadSlot () {
        Debug.Log ("Begin slot loading..."); 
        var sm = GetComponent<SlotMenuParams> ();
        Debug.Log ("Loading file from " + PhoneUtils.GetiPhoneDocumentsPath () + "/" + sm.slotInfo.Id + ".bundle");
        var bundle = AssetBundle.CreateFromFile (PhoneUtils.GetiPhoneDocumentsPath () + "/" + sm.slotInfo.Id + ".bundle"); //(PlayerPrefs.GetString (sm.slotInfo.Id + ".bundle"));

        Debug.Log (bundle);

        slotLoading = Application.LoadLevelAdditiveAsync (GamePrefs.GetString (Constants.SettingsSelectedSlot));
        if (slotLoading != null) { 
            slotLoading.allowSceneActivation = false;     
            while (!slotLoading.isDone) {
                Debug.Log ("Loading slot precent - " + Mathf.RoundToInt (slotLoading.progress * 100f).ToString () + "%");   
                if (slotLoading.progress == 0.9f) {
                    //освобождаем память
                    Resources.UnloadUnusedAssets ();
                    Debug.Log ("Loading slot scene...ok");
                    slotLoaded = true;
                    break;

                }
                yield return 0; 
            }
        }
    }
	
}
