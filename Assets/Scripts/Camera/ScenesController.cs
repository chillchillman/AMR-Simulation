using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenesController : MonoBehaviour
{
    public Camera cam;
    public OrbitCamera controlScript;

    // Start is called before the first frame update
    void Start()
    {
        controlScript = cam.GetComponent<OrbitCamera>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Fun1Rotation()
    {
        controlScript.Reset(0, 0, 1000);
    }
    public void Fun2Rotation()
    {
        controlScript.Reset(45,45,3000);
    }

}
