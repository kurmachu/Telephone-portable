using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelephoneSpinner : MonoBehaviour
{
    public Rigidbody telephoneBody;

    public float dragHeld = 0.05f;
    private float drag;
    // Start is called before the first frame update
    void Start()
    {
        drag = telephoneBody.angularDrag;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = Vector3.zero;
        if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit, 10f, LayerMask.GetMask("Spinorb")))
            {
                position = hit.point;
            }
        }
        else if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit, 10f, LayerMask.GetMask("Spinorb")))
            {
                position = hit.point;
            }
        }

        if (position != Vector3.zero)
        {
            if (!isHeld)
            {
                telephoneBody.angularDrag = dragHeld;
                //telephoneBody.isKinematic = true;
                isHeld = true;
                initial = Quaternion.LookRotation(new Vector3(-position.x, -position.y, -position.z), Vector3.up);
                //initialangle = telephoneBody.rotation;

            }
            lastangle = telephoneBody.rotation;
            //telephoneBody.AddTorque(new Vector3(position.y,-position.x,0), ForceMode.VelocityChange);
            Quaternion change = Quaternion.Euler(Quaternion.LookRotation(new Vector3(-position.x, -position.y, -position.z), Vector3.up).eulerAngles - initial.eulerAngles);
            //telephoneBody.rotation = change * initialangle;
            telephoneBody.AddTorque(-change.eulerAngles*Time.deltaTime,ForceMode.Acceleration);

            initial = Quaternion.LookRotation(new Vector3(-position.x, -position.y, -position.z), Vector3.up);
        }
        else
        {
            if (isHeld)
            {
                isHeld = false;
                telephoneBody.angularDrag = drag;


                //Quaternion deltaRotation = telephoneBody.rotation * Quaternion.Inverse(lastangle);

                //deltaRotation.ToAngleAxis(out var angle, out var axis);

                //angle *= Mathf.Deg2Rad;

                //telephoneBody.angularVelocity = (1.0f / Time.deltaTime) * angle * axis;

                //telephoneBody.isKinematic = false;
            }
        }
    }

    private bool isHeld;
    private Quaternion initial = Quaternion.Euler(0, 0, 0);
    private Quaternion initialangle = Quaternion.Euler(0, 0, 0);
    private Quaternion lastangle = Quaternion.Euler(0, 0, 0);
}
