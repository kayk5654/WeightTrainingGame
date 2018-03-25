using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JsonArrayHelper {

	// convert json to array of objects
	public static T[] FromJson<T>(string json){
		Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>> (json);
		return wrapper.Items;
	}

	// convert array of objects to json
	public static string ToJson<T>(T[] array){
		Wrapper<T> wrapper = new Wrapper<T> ();
		wrapper.Items = array;
		return JsonUtility.ToJson (wrapper);
	}

	// wrapper class
	[Serializable]
	private class Wrapper<T>{
		public T[] Items;	
	}
}
