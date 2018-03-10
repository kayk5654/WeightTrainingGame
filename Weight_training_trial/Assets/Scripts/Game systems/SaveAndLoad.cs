using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveAndLoad {

	// In the future update, the save data will be stored in a server.
	static private string filepath = Application.persistentDataPath + Path.DirectorySeparatorChar;

	public static bool exists(int _exerciseId){
		if (File.Exists (filepath + _exerciseId + "_saveData.json")) {
			return true;
		} else {
			return false;
		}
	}

	public static void save (SavedData _data, int _exerciseId) {
		string json = JsonUtility.ToJson (_data);
		File.WriteAllText (filepath + _exerciseId + "_saveData.json", json);
	}
	
	public static SavedData load (int _exerciseId) {
		if (!File.Exists (filepath + _exerciseId + "_saveData.json")) {
			return null;
		}

		string json = File.ReadAllText (filepath + _exerciseId + "_saveData.json");
		SavedData data = JsonUtility.FromJson<SavedData>(json);
		return data;
	}

	public static void reset(){
		// delete all files in a directory
		string[] filePaths = Directory.GetFiles(filepath);
		foreach (string path in filePaths) {
			File.Delete (path);
		}
	}
}
