using UnityEngine;
using System.Collections;

public class Rearranger : MonoBehaviour {
	
	// rearranges children in gameobject with this one in-between left or right
	public void RearrangeObj(GameObject goapp){
		GameObject[] goorder = new GameObject[goapp.transform.parent.transform.childCount];
		goorder = Reorder (goorder);
		string goappname = Parse.USV (goapp.transform.name)[1];
		
		bool reordered=false;
		
		Vector3 ray = goapp.transform.right;
		RaycastHit hit;
		if(Physics.Raycast (goapp.transform.position,ray,out hit,5f))
		{
			if(hit.transform!=null){
				GameObject go = hit.transform.gameObject;
				int next = Mathf2.String2Int (Parse.USV(go.name)[0]);
				
				goapp.transform.name = next.ToString()+"_"+goappname;
				SetAppNameInParent (next - 1,hit.transform.gameObject);
				reordered=true;
			}
		}
		
		goorder = Reorder(goorder);
		
		print (goorder.Length);
		for(int i = 0;i<goorder.Length;i++)
			print (goorder[i].name); // not in right order...
		
		if(Physics.Raycast (goapp.transform.position,-ray,out hit,5f))
		{
			if(hit.transform==null||reordered)return;
			
			GameObject go = hit.transform.gameObject;
			int prev = Mathf2.String2Int (Parse.USV(go.name)[0]);
			
			
		}		
		
		
		
	}
	
	GameObject[] Reorder(GameObject[] g){
		GameObject[] goorder = new GameObject[g.Length];
		for(int i=0;i<goorder.Length;i++)
			goorder[i] = g[i];	
		
		return goorder;
	}
	
	public void SetAppNameInParent(int i,GameObject g){
		string[] s = Parse.USV (g.name);string name="";
		if(s.Length>0){
			name = s[s.Length-1];
		}else{
			name = s[0];
		}
		g.name = i.ToString()+"_"+name;
	}
	
}
