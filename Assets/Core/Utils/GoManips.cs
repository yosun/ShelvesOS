using UnityEngine;
using System.Collections;

public class GoManips : MonoBehaviour {

	public static GameObject[] ChildrenToArray(GameObject go){
		int totalchildren = go.transform.GetChildCount ();
		GameObject[] gos = new GameObject[totalchildren]; 
		for(int i=0;i<totalchildren;i++){
			gos[i] = go.transform.GetChild (i).gameObject;
		}
		return gos;
	}
}
