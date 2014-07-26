using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;

public class Loader : MonoBehaviour {
    public float radius = 1.0f;
    public GUIController guiController = null;
    private List<GameObject> gameObjects = new List<GameObject>();

    private byte[] LoadBytes(string path) {
		FileStream fs = new FileStream(path, FileMode.Open);
		using(BinaryReader bin = new BinaryReader(fs)){
    		byte[] result = bin.ReadBytes((int)bin.BaseStream.Length);
	        return result;
        }
	}

    private Texture2D CreateTexture(string path) {
        Texture2D tex = new Texture2D(0, 0);
		tex.LoadImage(LoadBytes(path));
        return tex;
    }

    private GameObject CreateImagePlane(Texture2D tex) {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Plane);

        obj.AddComponent("MeshRenderer");
        obj.renderer.material.mainTexture = tex;

        // adjust size
        obj.transform.localScale = new Vector3(0.05f,0.05f,0.05f);

        // locate on a circle
        float r = radius + Random.Range(0,1.0f);
        float x = Random.Range(- r, r);
        float z = Mathf.Sqrt(r - x * x);
        float y = Random.Range(0.5f,2.0f);
        int sign = Random.Range(0,2) == 0 ? -1 : 1;
        z *= sign;
        obj.transform.position   = new Vector3(x, y, z);

        // rotate to camera pos
        Vector3 v = new Vector3(x,0,z) - Vector3.zero;

        float angle = Vector3.Angle(v, obj.transform.forward);
        Vector3 up = Vector3.Cross(v, obj.transform.forward);
        if(up.y > 0) {
            angle *= -1;
        }
        obj.transform.Rotate(new Vector3(90,angle,180));
        return obj;
    }

	void Start () {
        string path = "/Users/mzp/Pictures/yuriyuri";

        string[] xs = System.IO.Directory.GetFiles (@path, "*.jpg");
        this.gameObjects = xs.Select(x => CreateImagePlane(CreateTexture(x))).ToList();
	}

    private void FixedUpdate() {
        foreach(var x in gameObjects) {
            x.transform.RotateAround(Vector3.zero, Vector3.up, this.guiController.AngleVelocity * Time.deltaTime);
        }
    }

}
