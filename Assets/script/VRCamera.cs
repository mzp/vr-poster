using UnityEngine;
using System.Collections;

public class VRCamera : MonoBehaviour
{
    public GameObject riftCameraPrefab;
    public GUIController guiController;

    private GameObject riftCamera = null;
    private GameObject mainCamera = null;

    void Awake() {
#if UNITY_STANDALONE
        riftCamera = Instantiate(riftCameraPrefab, transform.position, transform.rotation) as GameObject;
        riftCamera.gameObject.SetActive(false);
        riftCamera.name = "Rift Camera";
        riftCamera.transform.parent = gameObject.transform;
        riftCamera.transform.position = new Vector3(0,2.0f,0);

#endif
        mainCamera = GameObject.Find("Player");
    }

    void Update() {
        if(riftCamera){
            riftCamera.SetActive(guiController.isOculusRift);
            mainCamera.SetActive(!guiController.isOculusRift);
        }

  /*      Vector3 cameraPos = new Vector3(transform.position.x,1.5f, transform.position.z);

        transform.position = cameraPos;
        mainCamera.transform.position = cameraPos;
*/
        if (Input.GetButtonDown("Fire2")) {
            OVRDevice.ResetOrientation();
        }
    }
}
