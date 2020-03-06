using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelephoneSpinnerNew : MonoBehaviour
{
    public Rigidbody telephoneBody;
    public Transform cameraTransform;

    public float dragHeld = 0.05f;
    private float drag;
    //public float maxDrag = 1;
    public float strength;
    public float increaseRate;
    public float cameraSpeedMobile = 0.3f;
    public float cameraSpeedDesktop = 0.3f;

    public Vector2 cameraMinMax;
    public float cameraReturnSeconds;


    // Start is called before the first frame update
    void Start()
    {
        drag = telephoneBody.angularDrag;
        telephoneBody.maxAngularVelocity = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        bool held = false;
        Vector3 position = Vector3.zero;
        if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch(0);
            position = touch.position;
            held = true;
        }
        else if (Input.touchCount<1&&Input.GetMouseButton(0))
        {
            position = Input.mousePosition;
            held = true;
        }

        if (held)
        {
            if (!lastHeld)
            {
                lastHeld = true;
                lastpos = position;
            }
            if (lastHeld)
            {
                //telephoneBody.angularDrag = Mathf.Lerp(dragHeld, drag, Vector3.Distance(lastpos, position) / maxDrag);
                //Debug.Log(telephoneBody.angularDrag);
                telephoneBody.angularDrag = dragHeld;
            }
            Vector3 toad = position - lastpos;
            telephoneBody.AddTorque(new Vector3(toad.y, -toad.x, 0)*Time.deltaTime*strength*(1+increaseRate*HypotenuseLength(telephoneBody.angularVelocity.x,telephoneBody.angularVelocity.y)), ForceMode.Acceleration);
            lastpos = position;
        }
        else
        {
            telephoneBody.angularDrag = drag;
            lastHeld = false;
        }

        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            float distance = Vector3.Distance(touch1.position, touch2.position);

            if (!pinching)
            {
                pinching = true;
                lastdist = distance;
            }

            float change = distance - lastdist;

            cameraTransform.position = new Vector3(0, 0, cameraTransform.position.z + (change*cameraSpeedMobile));

            lastdist = distance;
        }
        else
        {
            pinching = false;
        }

        if(Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            cameraTransform.position = new Vector3(0, 0, cameraTransform.position.z + (Input.GetAxis("Mouse ScrollWheel") *10f* cameraSpeedDesktop));
        }

        //if (!pinching && Input.GetAxis("Mouse ScrollWheel") == 0)
        if(true)
        {
            if (-cameraTransform.position.z > cameraMinMax.y)
            {
                cameraTransform.position = new Vector3(0, 0, cameraTransform.position.z + ((-cameraTransform.position.z - cameraMinMax.y)/(2/(Time.deltaTime*cameraReturnSeconds))));
            }
            if (-cameraTransform.position.z < cameraMinMax.x)
            {
                cameraTransform.position = new Vector3(0, 0, cameraTransform.position.z - ((cameraMinMax.x - -cameraTransform.position.z)/(2/(Time.deltaTime*cameraReturnSeconds))));
            }
        }


    }

    float HypotenuseLength(float sideALength, float sideBLength)
    {
        return Mathf.Sqrt(sideALength * sideALength + sideBLength * sideBLength);
    }

    private Vector3 lastpos;
    private bool lastHeld;

    private bool pinching;
    private float lastdist;
}
