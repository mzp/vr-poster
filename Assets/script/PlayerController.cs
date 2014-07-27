using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float speed = 1.0f;
    private CharacterController controller = null;

    private Vector3 basePos = new Vector3(0,0,0);
    public float rotationSpeed = 1e-10f;


    public void Start() {
        this.controller = GetComponent<CharacterController>();
    }

    public void Update() {
        Vector3 v = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		v = transform.TransformDirection(v);
		v *= speed;
        this.controller.Move(v);

        if (Input.GetMouseButtonDown(0)) {
            this.basePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0)) {
            float x = basePos.y - Input.mousePosition.y;
            float y = basePos.x - Input.mousePosition.x;

            Vector3 rot = new Vector3(x, y, 0);
            transform.Rotate(rot * rotationSpeed);
        }

        if (Input.GetButtonDown("Fire2")) {
            transform.rotation = Quaternion.identity;
        }
    }
}
