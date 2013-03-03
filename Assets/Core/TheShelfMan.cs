using UnityEngine;
using System.Collections;

public class TheShelfMan : MonoBehaviour {
	
	public GameObject goTheApps;
	
	Placer placer;
	Rearranger rearranger;
	
	public GameObject gotemp;
	
	void Awake(){
		placer = GetComponent<Placer>();
		rearranger = GetComponent<Rearranger>();
	}
	
	void Start(){
		placer.PlaceOnShelf(GoManips.ChildrenToArray(goTheApps));
	}
	
	void OnGUI(){
		if(GUI.Button (new Rect(10,10,50,50),"test ra"))
			rearranger.RearrangeObj(gotemp);
	}
	
}
