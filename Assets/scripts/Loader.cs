using UnityEngine;
using System.Collections;
using System.IO;

public class Loader : MonoBehaviour {
    public float x_left   = -8.0f;
    public float x_right  = 8.0f;
    public float y_bottom = 1.0f;
    public float y_top    = 5.0f;
    public float z        = 10f;


    private byte[] LoadBytes(string path) {
		FileStream fs = new FileStream(path, FileMode.Open);
		using(BinaryReader bin = new BinaryReader(fs)){
    		byte[] result = bin.ReadBytes((int)bin.BaseStream.Length);
	        return result;
        }
	}

    private GameObject createFromPath(string path) {
		Texture2D tex = new Texture2D(0, 0);
		tex.LoadImage(LoadBytes(path));

        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Plane);

        obj.AddComponent("MeshRenderer");
        obj.renderer.material.mainTexture = tex;

        obj.transform.Rotate(90,180,0);
        obj.transform.localScale = new Vector3(0.05f,0.05f,0.05f);
        obj.transform.position   = new Vector3(Random.Range(x_left, x_right),
                                               Random.Range(y_bottom, y_top),
                                               z + Random.Range(0,0.1f));
        obj.transform.RotateAround(Vector3.zero,Vector3.up,Random.Range(-180,180));
        obj.transform.LookAt(new Vector3(0,1.0f,0));
        obj.transform.Rotate(90,0,0);
        return obj;
    }

	void Start () {
        string path = "/Users/mzp/Pictures/yuriyuri";

        string[] xs = System.IO.Directory.GetFiles (@path, "*.jpg");
        Debug.Log(path);
        foreach(string x in xs) {
            Debug.Log(x);
            createFromPath(x);
        }
	}

	void Update () {

	}

}
