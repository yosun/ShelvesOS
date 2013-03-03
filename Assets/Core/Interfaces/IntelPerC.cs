using UnityEngine;
using System.Collections;

public class IntelPerC : MonoBehaviour {
	
	// v0.12
	// gesture has finger GetWorldPositionThumb...Pinky
	// voice - dictation/resetdictation 
	// gesture - returns conf, openness, worldposition, normal
	// face - rect
	
	public Texture2D 			m_Texture;
	private PXCUPipeline 		pp;
    private int[] 				size=new int[2]{0,0};
	private PXCUPipeline.Mode 	mode=PXCUPipeline.Mode.GESTURE | PXCUPipeline.Mode.FACE_LOCATION | PXCUPipeline.Mode.VOICE_RECOGNITION;
	
	public GameObject goTextureCube;
	
	int openness=-1; int confidence=0; int indexConfidence=0;
	Vector3 worldPosition=Vector3.zero; 
	Vector3 worldPositionThumb=Vector3.zero; Vector3 worldPositionIndex=Vector3.zero; Vector3 worldPositionMiddle=Vector3.zero; Vector3 worldPositionRing=Vector3.zero; Vector3 worldPositionPinky=Vector3.zero;
	Vector3 normal=Vector3.zero;
	
	uint faceConfidence=0;
	PXCMFaceAnalysis.Detection.ViewAngle faceViewAngle=PXCMFaceAnalysis.Detection.ViewAngle.VIEW_ANGLE_OMNI;
	Vector2 faceRectCenter=Vector2.zero;
	
	bool cameraFound=false;
	
	int[] labels=new int[3]{0,256,256};
	byte[] labelmap;
	
	string dictation="DUNNO";
	
	public bool GetCameraStatus(){
		return cameraFound;	
	}
	public uint GetFaceConfidence(){
		return faceConfidence;	
	}
	public Vector2 GetFaceRectCenter(){
		return faceRectCenter;	
	}
	
	public bool GetClosedCertain(){
		if(openness<5&&openness>=0&&confidence>80)return true;
		else return false;
	}
	public bool GetOpenCertain(){
		if(confidence>80&&openness>80)return true;
		else return false;
	}
	public bool GetOpenCertainish(){
		if(confidence>80&&openness>60)return true;
		else return false;
	}	
	public int GetOpenness(){
		return openness;
	}
	public int GetConfidence(){
		return confidence;	
	}
	public Vector3 GetWorldPosition(){
		return worldPosition;	
	}
	public Vector3 GetWorldPositionThumb(){
		return worldPositionThumb;	
	}
	public Vector3 GetWorldPositionIndex(){
		return worldPositionIndex;	
	}	
	public Vector3 GetWorldPositionMiddle(){
		return worldPositionMiddle;	
	}	
	public Vector3 GetWorldPositionRing(){
		return worldPositionRing;	
	}	
	public Vector3 GetWorldPositionPinky(){
		return worldPositionPinky;	
	}		
	public Vector3 GetNormal(){
		return normal;	
	}
	public string GetDictation(){
		return dictation;	
	}
	public void ResetDictation(){
		dictation="DUNNO";	
	}
	
	
	void Start () {
		pp=new PXCUPipeline();
		//shm=GetComponent<ShadowHandMod>();
		
		//transform.localScale = new Vector3(Screen.width/420f,Screen.height/420f,0.01f);
		
		if (!pp.Init(mode)) {
			print("Unable to initialize the PXCUPipeline");
			cameraFound=false;
			return;
		}else 
			cameraFound=true;
		
        if (pp.QueryLabelMapSize(size))
	        print("LabelMap: width=" + size[0] + ", height=" + size[1]);
		
		if (size[0]>0) {
			m_Texture = new Texture2D (size[0], size[1], TextureFormat.ARGB32,false);
	        goTextureCube.renderer.material.mainTexture = m_Texture;
			
			labelmap=new byte[size[0]*size[1]];
			
			//shm.ZeroImage(m_Texture);
		}	
		
		string[] strVoiceCommands=new string[3];
		strVoiceCommands[0]="Hello";
		strVoiceCommands[1]="Tetrahedron";
		strVoiceCommands[2]="Mega";
		pp.SetVoiceCommands(strVoiceCommands);
	}
	
	void Update () {
		if (!pp.AcquireFrame(false)) return;

		if (pp.QueryLabelMapAsImage(m_Texture)) 
			m_Texture.Apply();	
			
		/*int[] labels=new int[3]{0,256,256};
		pp.QueryLabelMap(labelmap,labels);
		shm.ProcessTexture(m_Texture,labels,labelmap);	*/
	   			
		// face stuff
		int face; ulong timestamp;
		if(pp.QueryFaceID(0,out face,out timestamp)){
			PXCMFaceAnalysis.Detection.Data datafacedetect;
			if(pp.QueryFaceLocationData(face,out datafacedetect)){
				faceConfidence = datafacedetect.confidence;
				faceViewAngle = datafacedetect.viewAngle;
				faceRectCenter = new Vector2(datafacedetect.rectangle.x,datafacedetect.rectangle.y);
			//	print (faceRectCenter+" "+faceConfidence);
			}
		}
		/*
		 * GeoNode 
		 * timeStamp
		 * user
		 * body - LABEL
		 * side - Side	
		 * confidence - 0 to 100
		 * positionWorld - node pos, world coords
		 * positionImage - node pos, image specific (x,y,d)
		 * 
		 * finger tip
		 * radiusWorld - volume of fingertip 3D in meters
		 * radiusImage - volume of fingertip in 2D in pixels
		 * 
		 * hand/palm
		 * normal - vec perp palm center
		 * openness - 0 to 100
		 * opennessState - Openness.LABEL_OPEN,_CLOSE,_OPENNESS_ANY (unknown)
		 		 **/
		
		PXCMGesture.GeoNode[] data;
		data=new PXCMGesture.GeoNode[6];// PXCMGesture.GeoNode[] data;
		
		if(pp.QueryGeoNode(PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY,data)){
			string s="";
			for(int i=0;i<data.Length;i++){
				s+=" ~ " +data[i].positionWorld.x.ToString()+","+data[i].positionWorld.y.ToString()+","+data[i].positionWorld.z.ToString ()	;
			}
			openness = (int)data[0].openness;print (openness);
			confidence = (int)data[0].confidence; normal = new Vector3(data[0].normal.x,-data[0].normal.y,data[0].normal.z);
			
			worldPosition = new Vector3(data[0].positionWorld.x,-data[0].positionWorld.y,data[0].positionWorld.z);
			worldPositionThumb = new Vector3(data[1].positionWorld.x,-data[1].positionWorld.y,data[1].positionWorld.z);
			worldPositionIndex = new Vector3(data[2].positionWorld.x,-data[2].positionWorld.y,data[2].positionWorld.z);
			worldPositionMiddle = new Vector3(data[3].positionWorld.x,-data[3].positionWorld.y,data[3].positionWorld.z);
			worldPositionRing = new Vector3(data[4].positionWorld.x,-data[4].positionWorld.y,data[4].positionWorld.z);
			worldPositionPinky = new Vector3(data[5].positionWorld.x,-data[5].positionWorld.y,data[5].positionWorld.z);
			
			print (s);
		}
		
		/*PXCMGesture.GeoNode gnode;
		if (pp.QueryGeoNode(PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY|PXCMGesture.GeoNode.Label.LABEL_FINGER_INDEX,out gnode)){
			openness = (int)gnode.openness;
			worldPosition = new Vector3(gnode.positionWorld.x,-gnode.positionWorld.y,gnode.positionWorld.z);
			normal = new Vector3(gnode.normal.x,gnode.normal.y,gnode.normal.z);
			confidence = (int)gnode.confidence;
			print (worldPosition+ " "+confidence);
		}*/
		/*if (pp.QueryGeoNode(PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY,out gnode)){
			worldPositionIndex = new Vector3(gnode.positionWorld.x,-gnode.positionWorld.y,gnode.positionWorld.z);
			indexConfidence = (int)gnode.confidence;
			print (worldPositionIndex+ " "+indexConfidence);
		}*/		
		
		PXCMVoiceRecognition.Recognition rdata;
		if (pp.QueryVoiceRecognized(out rdata)){
			print ("voice rec (label="+rdata.label+","+rdata.confidence+",size="+rdata.dictation.Length+", dictation="+rdata.dictation+")");
			dictation = rdata.dictation;
		}
		
		pp.ReleaseFrame();		
	}
	
    void OnDisable() {
		pp.Close();
		pp.Dispose();
	}
	
	public void CloseScene(){
		pp.ReleaseFrame();
		pp.Close();
		pp.Dispose ();
	}
	
}
