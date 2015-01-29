using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GooglePlusOneButton : MonoBehaviour {

    private List<AN_PlusButton> Abuttons = new List<AN_PlusButton>();
    private AN_PlusButton PlusButton = null;
    
    void Start()
    {
        //GooglePlayManager.instance.ShowRequestsAccepDialog();
        //GooglePlayManager.ActionAvaliableDeviceAccountsLoaded += ActionAvaliableDeviceAccountsLoaded;
        //GooglePlayManager.instance.RetriveDeviceGoogleAccounts();
        //GooglePlayManager.instance.LoadTocken();
        GooglePlayConnection.ActionConnectionResultReceived += ActionConnectionResultReceived;
        if (GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED)
        {
            AndroidMessage.Create("", "Already connected to Play Service...");
            CreateGooglePlusOneButton();
        }
        else
        {
            ConnectToGoogleService();
        }
        
    }

    private void ActionConnectionResultReceived(GooglePlayConnectionResult result)
    {
        if (result.IsSuccess)
        {
           //AndroidMessage.Create("", "Connected!");
           CreateGooglePlusOneButton();
        }
        else
        {
            //AndroidMessage.Create("", "Connection failed with code: " + result.code.ToString());
            CreateGooglePlusOneButton();
        }
    }

    public void CreateGooglePlusOneButton()
    {
        if (PlusButton == null)
        {
            PlusButton = new AN_PlusButton("", AN_PlusBtnSize.SIZE_STANDARD, AN_PlusBtnAnnotation.ANNOTATION_BUBBLE);
            PlusButton.SetPosition(Screen.width / 2, Screen.height / 2);
            PlusButton.Show();
        }
    }


    //private void OnGoogleConnect()
    //{
    //    GooglePlayConnection.instance.connect(GooglePlayManager.instance.deviceGoogleAccountList[0]);
    //}

    private void ConnectToGoogleService()
    {
        //AndroidMessage msg = AndroidMessage.Create("", "Trying connecting to Play Service...");
        //msg.OnComplete += OnGoogleConnect;
        GooglePlayConnection.instance.connect();
    }

    //void ButtonClicked()
    //{
    //    AndroidMessage.Create("Click Detected", "Plus Button Click Detected");
    //}

    //private void DisconnectFromGService()
    //{
    //    GooglePlayConnection.instance.disconnect();
    //    AndroidMessage.Create("Start", "Disconnecting from Play Service...");
    //}

    //private void ActionAvaliableDeviceAccountsLoaded(List<string> accounts)
    //{
    //    string msg = "Device contains following google accounts:" + "\n";
    //    foreach (string acc in GooglePlayManager.instance.deviceGoogleAccountList)
    //    {
    //        msg += acc + "\n";
    //    }
        
    //    AndroidDialog dialog = AndroidDialog.Create("Accounts Loaded", msg, "Sign With Fitst one", "Do Nothing");
    //    dialog.OnComplete += SighDialogComplete;
    //}

    //private void SighDialogComplete(AndroidDialogResult res)
    //{
    //    if (res == AndroidDialogResult.YES)
    //    {
    //        GooglePlayConnection.instance.connect(GooglePlayManager.instance.deviceGoogleAccountList[0]);
    //    }
    //}


    //private void ActionOAuthTockenLoaded(string token)
    //{
    //    AN_PoupsProxy.showMessage("Toekn Loaded", GooglePlayManager.instance.loadedAuthTocken);
    //}

    //private void RetriveToken()
    //{
    //    GooglePlayManager.instance.LoadTocken();
    //}

    private void OnDestroy()
    {
        if (PlusButton != null)
        {
            if (PlusButton.IsShowed) PlusButton.Hide();
            PlusButton = null;
        }

        if (!GooglePlayConnection.IsDestroyed)
        {
            GooglePlayConnection.ActionConnectionResultReceived -= ActionConnectionResultReceived;
        }

        //if (!GooglePlayManager.IsDestroyed)
        //{
        //    GooglePlayManager.ActionAvaliableDeviceAccountsLoaded -= ActionAvaliableDeviceAccountsLoaded;
        //    GooglePlayManager.ActionOAuthTockenLoaded -= ActionOAuthTockenLoaded;
        //}

    }



}
