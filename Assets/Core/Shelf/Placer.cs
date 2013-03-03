using UnityEngine;
using System.Collections;

public class Placer : MonoBehaviour {
	
	public GameObject goShelfBoard;
	float shelfHeight=3f; float shelfVertOffset=-2f;
	int maxPerShelf=5;
	
	/* creates shelves and places goArray of *gameobjects* on shelves
	 * use GoManips.ChildrenToArray to convert children object to gameobject[]
	 */
	public void PlaceOnShelf(GameObject[] goarray){
		int totalobjs = goarray.Length;
		int totalrows = Mathf.CeilToInt ((float)totalobjs/(float)maxPerShelf); 
		float shelfsize = goShelfBoard.transform.Find ("Shelf").gameObject.transform.localScale.x;
		float shelfspaceoffset = shelfsize * 0.5f * 0.8f;
		float shelfspacing =  shelfsize/(float)maxPerShelf ; 
		
		int k=0;
		for(int j=0;j<totalrows;j++){
			float currentshelfheight = j*shelfHeight + shelfVertOffset;
			GameObject g = Instantiate(goShelfBoard,new Vector3(0,currentshelfheight,0),Quaternion.identity) as GameObject;
			g.name = "Shelf_"+j.ToString();
			for(int i=0;i<maxPerShelf;i++){
				k++;
				if(k>totalobjs)return;
				Vector3 pos = new Vector3(i*shelfspacing - shelfspaceoffset,currentshelfheight,0);
				goarray[k-1].transform.position = pos;
			}
		}
	}
	
	
}
