using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelephoneSpinnerNew : MonoBehaviour
{
    public Rigidbody telephoneBody;

    public float dragHeld = 0.05f;
    private float drag;
    public float maxDrag = 1;
    public float strength;
    // Start is called before the first frame update
    void Start()
    {
        drag = telephoneBody.angularDrag;
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
                telephoneBody.angularDrag = ((telephoneBody.angularDrag+Mathf.Lerp(dragHeld, drag, Vector3.Distance(lastpos, position) / maxDrag))/2);
                Debug.Log(telephoneBody.angularDrag);
            }
            Vector3 toad = position - lastpos;
            telephoneBody.AddTorque(new Vector3(toad.y, -toad.x, 0)*Time.deltaTime*strength, ForceMode.Acceleration);
            lastpos = position;
        }
        else
        {
            telephoneBody.angularDrag = drag;
            lastHeld = false;
        }
    }

    private Vector3 lastpos;
    private bool lastHeld;
}
