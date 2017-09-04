using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour
{
    private SteamVR_TrackedObject _trackedObj;
    public GameObject LaserPrefab;
    private GameObject _laser;
    private Transform _laserTransform;
    public float Thickness = 0.002f;
    public Color Color;
    private Vector3 _hitPoint;
    private SteamVR_Controller.Device Controller { get { return SteamVR_Controller.Input((int)_trackedObj.index); } }

    void Awake()
    {
        _trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Use this for initialization
    void Start()
    {
        _laser = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _laser.transform.parent = this.transform;
        _laser.transform.localScale = new Vector3(Thickness, Thickness, 0f);
        _laser.transform.localPosition = new Vector3(0f, 0f, 0f);
        _laser.transform.localRotation = Quaternion.identity;
        _laser.SetActive(true);

        Material newMaterial = new Material(Shader.Find("Unlit/Color"));
        newMaterial.SetColor("_Color", Color);
        _laser.GetComponent<MeshRenderer>().material = newMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        //{
            _laser.SetActive(true);

            Ray raycast = new Ray(_trackedObj.transform.position, _trackedObj.transform.forward);
            RaycastHit hit;
            bool bHit = Physics.Raycast(raycast, out hit);

            if (bHit)
            {
                _laser.transform.localScale = new Vector3(Thickness, Thickness, hit.distance);
                _laser.transform.localPosition = new Vector3(0f, 0f, hit.distance / 2f);
            }
        //}
        //else
        //{
        //    laser.SetActive(false);
        //}
    }
}
