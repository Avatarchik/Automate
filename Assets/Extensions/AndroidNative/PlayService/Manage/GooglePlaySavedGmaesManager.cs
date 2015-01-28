using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GooglePlaySavedGmaesManager :  SA_Singleton<GooglePlaySavedGmaesManager> {


	//Events

	public const string NEW_GAME_SAVE_REQUES   			= "new_game_save_reques";
	public const string GAME_SAVE_RESULT   				= "game_save_result";

	public const string GAME_SAVE_LOADED  	 			= "new_game_save_reques";
	public const string CONFLICT  			 			= "conflict";

	public const string AVALIABLE_GAME_SAVES_LOADED   	= "avaliable_game_saves_loaded";




	
	//Actions
	public static Action ActionNewGameSaveRequest 	= delegate {};
	public static Action<GooglePlayResult> ActionGameSaveResult 	= delegate {};
	public static Action<GooglePlayResult> ActionAvaliableGameSavesLoaded 	= delegate {};

	public static Action<GP_SpanshotLoadResult> ActionGameSaveLoaded 	= delegate {};
	public static Action<GP_SnapshotConflict> ActionConflict 	= delegate {};

	private List<GP_SnapshotMeta> _AvaliableGameSaves = new List<GP_SnapshotMeta>();



	//--------------------------------------
	// INITIALIZE
	//--------------------------------------


	void Awake() {
		DontDestroyOnLoad(gameObject);
	}


	//--------------------------------------
	// PUBLIC API CALL METHODS
	//--------------------------------------

	public void ShowSavedGamesUI(string title, int maxNumberOfSavedGamesToShow)  {
		if (!GooglePlayConnection.CheckState ()) { return; }

		AN_GooglePlayProxy.ShowSavedGamesUI_Bridge(title, maxNumberOfSavedGamesToShow);
	}


	public void CreateNewSpanShot(string name, string description, Texture2D coverImage, string spanshotData)  {
		CreateNewSpanShot(name, description, coverImage, GetBytes(spanshotData));
	}


	public void CreateNewSpanShot(string name, string description, Texture2D coverImage, byte[] spanshotData)  {
		string mdeia = string.Empty;

		if(coverImage != null) {
			byte[] val = coverImage.EncodeToPNG();
			mdeia = System.Convert.ToBase64String (val);
		}  else {
			Debug.LogWarning("GooglePlaySavedGmaesManager::CreateNewSpanShot:  coverImage is null");
		}

		string data = System.Convert.ToBase64String (spanshotData);

		AN_GooglePlayProxy.CreateNewSpanShot_Bridge(name, description, mdeia, data);
	}


	public void LoadSpanShotByName(string name) {
		AN_GooglePlayProxy.OpenSpanshotByName_Bridge(name);
	}

	public void LoadAvaliableSavedGames() {
		AN_GooglePlayProxy.LoadSpanshots_Bridge();
	}


	
	//--------------------------------------
	// GET / SET
	//--------------------------------------

	public List<GP_SnapshotMeta> AvaliableGameSaves {
		get {
			return _AvaliableGameSaves;
		}
	}


	//--------------------------------------
	// PRIVATE  METHODS
	//--------------------------------------

	private static byte[] GetBytes(string str) {
		byte[] bytes = new byte[str.Length * sizeof(char)];
		System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
		return bytes;
	}


	private static string GetString(byte[] bytes) {
		char[] chars = new char[bytes.Length / sizeof(char)];
		System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
		return new string(chars);
	}

	
	//--------------------------------------
	// EVENTS
	//--------------------------------------

	private void OnLoadSnapshotsResult(string data) {
		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);
		
		GooglePlayResult result = new GooglePlayResult (storeData [0]);
		if(result.isSuccess) {
			
			_AvaliableGameSaves.Clear ();
			
			for(int i = 1; i < storeData.Length; i+=4) {
				if(storeData[i] == AndroidNative.DATA_EOF) {
					break;
				}

				GP_SnapshotMeta meta = new GP_SnapshotMeta();
				meta.Title = storeData[i];
				meta.LastModifiedTimestamp = System.Convert.ToInt64(storeData [i + 1]);
				meta.Description = storeData[i + 2];
				meta.CoverImageUrl = storeData[i + 3];
	

				_AvaliableGameSaves.Add(meta);
				
			}
			
			Debug.Log ("Loaded: " + _AvaliableGameSaves.Count + " Snapshots");
		}
		
		ActionAvaliableGameSavesLoaded(result);
		dispatch (AVALIABLE_GAME_SAVES_LOADED, result);
	}



	private void OnSavedGamePicked(string data) {


		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);


		GP_SpanshotLoadResult result = new GP_SpanshotLoadResult (storeData [0]);
		if(result.isSuccess) {
			string Title = storeData [1];
			long LastModifiedTimestamp = System.Convert.ToInt64(storeData [2]) ;
			string Description = storeData [3];
			string CoverImageUrl = storeData [4];
			byte[] decodedFromBase64 = System.Convert.FromBase64String(storeData [5]);
			
			
			GP_Snapshot  Snapshot =  new GP_Snapshot();
			Snapshot.meta.Title 					= Title;
			Snapshot.meta.Description 				= Description;
			Snapshot.meta.CoverImageUrl 			= CoverImageUrl;
			Snapshot.meta.LastModifiedTimestamp 	= LastModifiedTimestamp;

			Snapshot.bytes 							= decodedFromBase64;
			Snapshot.stringData 					= GetString(decodedFromBase64);

			result.SetSnapShot(Snapshot);
		
		}


		dispatch(GAME_SAVE_LOADED, result);
		ActionGameSaveLoaded(result);

	}

	private void OnSaveResult(string code) {
		GooglePlayResult result = new GooglePlayResult (code);
		ActionGameSaveResult(result);
		dispatch(GAME_SAVE_RESULT, result);
	}

	private void OnConflict(string data)  {
		string[] storeData;
		storeData = data.Split(AndroidNative.DATA_SPLITTER [0]);
		
		string Title = storeData [0];
		long LastModifiedTimestamp = System.Convert.ToInt64(storeData [1]) ;
		string Description = storeData [2];
		string CoverImageUrl = storeData [3];
		byte[] decodedFromBase64 = System.Convert.FromBase64String(storeData [4]);

		GP_Snapshot  Snapshot1 =  new GP_Snapshot();
		Snapshot1.meta.Title 					= Title;
		Snapshot1.meta.Description 			= Description;
		Snapshot1.meta.CoverImageUrl 			= CoverImageUrl;
		Snapshot1.meta.LastModifiedTimestamp 	= LastModifiedTimestamp;

		Snapshot1.bytes 					= decodedFromBase64;
		Snapshot1.stringData 			= GetString(decodedFromBase64);


		Title = storeData [5];
		LastModifiedTimestamp = System.Convert.ToInt64(storeData [6]) ;
		Description = storeData [7];
		CoverImageUrl = storeData [8];
		decodedFromBase64 = System.Convert.FromBase64String(storeData [9]);

		GP_Snapshot  Snapshot2 =  new GP_Snapshot();
		Snapshot2.meta.Title 					= Title;
		Snapshot2.meta.Description 			= Description;
		Snapshot2.meta.CoverImageUrl 			= CoverImageUrl;
		Snapshot2.meta.LastModifiedTimestamp 	= LastModifiedTimestamp;

		Snapshot2.bytes 					= decodedFromBase64;
		Snapshot2.stringData 			= GetString(decodedFromBase64);


		GP_SnapshotConflict result =  new GP_SnapshotConflict(Snapshot1, Snapshot2);

		dispatch(CONFLICT, result);
		ActionConflict(result);

	}

	private void OnLoadResult(string code) {
		GooglePlayResult result = new GooglePlayResult (code);
		ActionGameSaveResult(result);
		dispatch(GAME_SAVE_RESULT, result);
	}

	private void OnNewGameSaveRequest(string data) {
		dispatch(NEW_GAME_SAVE_REQUES);
		ActionNewGameSaveRequest();

	}































}
