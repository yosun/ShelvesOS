using UnityEngine;
using System.Collections;

public class Parse : MonoBehaviour {

	public static string[] USV(string s){
		return s.Split ("_"[0]);
	}
}
