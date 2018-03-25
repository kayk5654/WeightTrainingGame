using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preferences {

	// general use functions
	public static void save(){
		PlayerPrefs.Save ();
	}

	// set as default
	public static void setDefault(){
		setLanguage (0);
		setAudioVolume (1f);
	}

	// language settings
	public static void setLanguage(int _languageCode){
		// 0 -> English, 1 -> Japanese, 2 -> Chinese (1 and 2 will be added later)
		PlayerPrefs.SetInt("language", _languageCode);
	}

	public static int getLanguage(){
		return PlayerPrefs.GetInt ("language");
	}

	// audio settings
	public static void setAudioVolume(float _volume){
		// _volume is multiplier between 0 and 2. 1 is original setting
		PlayerPrefs.SetFloat ("audioVolume", _volume);
	}

	public static float getAudioVolume(){
		return PlayerPrefs.GetFloat ("audioVolume");
	}

	// graphics settings

}
