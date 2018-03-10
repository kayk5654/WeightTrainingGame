using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveAndLoad {

	// In the future update, the save data will be stored in a server.
	static private string filepath = Application.persistentDataPath + "saveData.json";

	public static bool exists(){
		if (File.Exists (filepath)) {
			return true;
		} else {
			return false;
		}
	}

	public static void save (SavedData _data, int _exerciseId) {
		SavedData[] dataArray = new SavedData[3];

		// to overwrite previous data, existing data is loaded
		if (exists()) {
			string data = File.ReadAllText (filepath);
			dataArray = JsonArrayHelper.FromJson<SavedData> (data);
		}

		dataArray [_exerciseId] = _data;

		string json = JsonArrayHelper.ToJson (dataArray);
		File.WriteAllText (filepath, json);
	}
	
	public static SavedData load (int _exerciseId) {
		if (!File.Exists (filepath)) {
			return null;
		}

		string data = File.ReadAllText (filepath);
		SavedData[] dataArray = JsonArrayHelper.FromJson<SavedData> (data);
		return dataArray[_exerciseId];
	}

	public static void reset(){
		File.Delete (filepath);
	}
}
