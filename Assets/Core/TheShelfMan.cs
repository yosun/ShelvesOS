using UnityEngine;
using System.Collections;

public class TheShelfMan : MonoBehaviour {
	
	public GameObject goTheApps;
	
	static GameObject curSelected;
	
	Placer placer;
	Rearranger rearranger;
	
	IntelPerC ipc;
	
	public GameObject gotemp;
	
	float timeLastOpened=0f;
	
	public static void SelectMe(GameObject g){
		curSelected = g;	
	}
	
	public static void DeselectIfMe(GameObject g){
		if(curSelected==g)
			g=null;
	}
	
	void Awake(){
		placer = GetComponent<Placer>();
		rearranger = GetComponent<Rearranger>();
	}
	
	void Start(){
		placer.PlaceOnShelf(GoManips.ChildrenToArray(goTheApps));
		ipc = GetComponent<IntelPerC>();
	}
	
	void OnGUI(){
		if(GUI.Button (new Rect(10,10,50,50),"test ra"))
			rearranger.RearrangeObj(gotemp);
	}
	
	void Update(){
		if(curSelected!=null){
			if(TheInputMode.im == TheInputMode.InputModes.IntelPerC){
				if(ipc.GetClosedCertain ())
					if((timeLastOpened-Time.timeSinceLevelLoad)<0.5f)
						SelectConfirm (curSelected);	
				else if(ipc.GetOpenCertainish ())
					timeLastOpened=Time.timeSinceLevelLoad;
			}
		}
	}
	
	void SelectConfirm(GameObject g){
		
	}
	
}
