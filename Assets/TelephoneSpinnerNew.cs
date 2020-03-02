using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelephoneSpinnerNew : MonoBehaviour
{
    public Rigidbody telephoneBody;

    public float dragHeld = 0.05f;
    private float drag;
    //public float maxDrag = 1;
    public float strength;
    public float increaseRate;
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
        else if (Input.GetMouseButton(0))
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
    }

    float HypotenuseLength(float sideALength, float sideBLength)
    {
        return Mathf.Sqrt(sideALength * sideALength + sideBLength * sideBLength);
    }

    private Vector3 lastpos;
    private bool lastHeld;
}
