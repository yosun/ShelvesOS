using UnityEngine;
using System.Collections;

public class ThePointer : MonoBehaviour {

	void OnTriggerEnter(Collider col){
		HoverMe(col.gameObject,new Color(1,1,1,1));	
		TheShelfMan.SelectMe(col.gameObject);
	}
	
	void OnTriggerExit(Collider col){
		TheShelfMan.DeselectIfMe(col.gameObject);
		DeselectAll(col.gameObject);	
	}
	
	public static void HoverMe(GameObject g,Color col){
		if(g.transform.parent==null)return;
		DeselectAll(g);
		
		OutlineGlowRenderer ren = g.GetComponent<OutlineGlowRenderer>();
		ren.DrawOutline=true;
		ren.OutlineColor=col;
	}
	
	public static void DeselectAll(GameObject g){
		if(g.transform.parent==null)return;
		Transform parent = g.transform.parent.transform; 
		foreach(GameObject go in parent){
			if(g!=go){
				OutlineGlowRenderer ogr = go.GetComponent<OutlineGlowRenderer>();	
				ogr.DrawOutline=false;
			}
		}		
	}	
	
}
