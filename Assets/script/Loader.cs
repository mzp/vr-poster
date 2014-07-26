using UnityEngine;
using System.Collections;
using System.IO;

public class Loader : MonoBehaviour {
    public float radius = 1.0f;

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
        foreach(string x in xs) {
            CreateImagePlane(CreateTexture(x));
        }
	}

    private float Angle(Vector3 x, Vector3 y) {
        Vector3 referenceRight= Vector3.Cross(Vector3.up, x);
        Vector3 newDirection = x;
        float sign = Mathf.Sign(Vector3.Dot(newDirection, referenceRight));

        return sign * Vector3.Angle(x, y);
    }

}
