using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class LaserScript : MonoBehaviour {
    
//    private SteamVR_TrackedObject trackedObj;
    public GameObject laserPrefab;
    public float thickness = 0.002f;
    private GameObject laser;
    private Transform laserTransform;
    private Vector3 hitPoint; 

//    private SteamVR_Controller.Device Controller
//    {
//        get { return SteamVR_Controller.Input((int)trackedObj.index); }
//    }

    void Awake()
    {
//        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }
    
    private void ShowLaser(RaycastHit hit)
    {
        laser.SetActive(true);
        laserTransform.position = Vector3.Lerp(this.transform.position, hitPoint, .5f);
        laserTransform.LookAt(hitPoint); 
        laserTransform.localScale = new Vector3(thickness, thickness, hit.distance);
    }
    
	// Use this for initialization
	void Start () {
	    laser = Instantiate(laserPrefab);
	    laserTransform = laser.transform;
	    
	    //GetComponent<VRTK_ControllerEvents>().GripPressed += new ControllerInteractionEventHandler(DoTouchpadPressed);
	    //GetComponent<VRTK_ControllerEvents>().GripReleased += new ControllerInteractionEventHandler(DoTouchpadReleased);
	}
    
//    void DoTouchpadPressed(object sender, ControllerInteractionEventArgs e)
//    {
//        
//    }
//    
//    void DoTouchpadReleased(object sender, ControllerInteractionEventArgs e)
//    {
//        
//    }
	
	// Update is called once per frame
	void Update ()
	{
//	    laser.SetActive(true);
//	    laserTransform.position = Vector3.Lerp(transform.position, transform.forward * 100, 0.5f);
//	    laserTransform.LookAt(transform.forward);
//	    laserTransform.localScale = new Vector3(thickness, thickness, 200);
	    
	    RaycastHit hit;
	    if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
	    {
	        hitPoint = hit.point;
	        ShowLaser(hit);
	    }
	    else
	    {
	        laser.SetActive(false);
	    }
//	    if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
//	    {
//	        RaycastHit hit;
//
//	        if (Physics.Raycast(trackedObj.transform.position, transform.forward, out hit, 100))
//	        {
//	            hitPoint = hit.point;
//	            ShowLaser(hit);
//	        }
//	    }
//	    else
//	    {
//	        laser.SetActive(false);
//	    }
	}
}
