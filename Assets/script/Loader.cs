using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net;

public class Loader : MonoBehaviour {
    public float radius = 1.0f;
    public GUIController guiController = null;
    private List<GameObject> gameObjects = new List<GameObject>();
    private PixivAPI pixivAPI = new PixivAPI();

    private string prevPath = null;
    private string prevKeywords = null;

    private byte[] LoadBytesFromPath(string path) {
		FileStream fs = new FileStream(path, FileMode.Open);
		return ReadBinaryData(fs);
	}

    private byte[] ReadBinaryData(Stream st) {
        byte[] buf = new byte[32768]; // 一時バッファ

        using (MemoryStream ms = new MemoryStream()) {

          while (true) {
            // ストリームから一時バッファに読み込む
            int read = st.Read(buf, 0, buf.Length);

            if (read > 0) {
              // 一時バッファの内容をメモリ・ストリームに書き込む
              ms.Write(buf, 0, read);
            } else {
              break;
            }
          }
          // メモリ・ストリームの内容をバイト配列に格納
          return ms.ToArray();
        }
    }

    private byte[] LoadBytesFromUrl(string url){
        Debug.Log(url);
        HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
        HttpWebResponse res = (HttpWebResponse)req.GetResponse();
        Stream str = res.GetResponseStream();
        return ReadBinaryData(str);
    }

    private GameObject CreateImagePlane(byte[] data) {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Plane);

        // create image texture
        Texture2D tex = new Texture2D(0, 0);
		tex.LoadImage(data);
        obj.renderer.material.mainTexture = tex;

        // adjust size
        obj.transform.localScale = new Vector3(0.05f,0.05f,0.05f);

        // locate on a circle
        float r = radius + Random.Range(0,0.1f);
        float x = Random.Range(- r, r);
        float z = Mathf.Sqrt(r*r - x * x);
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

    private void FixedUpdate() {
        foreach(var x in gameObjects) {
            x.transform.RotateAround(Vector3.zero, Vector3.up,
                                     this.guiController.AngleVelocity * Time.deltaTime);
        }
    }

    private void UpdateGameObjects(IEnumerable<byte[]> xs){
        this.gameObjects.ForEach(x => GameObject.Destroy(x));
        this.gameObjects = new List<GameObject>();
        this.gameObjects = xs.Select(x => CreateImagePlane(x)).ToList();
    }

    private void Update() {
        if(guiController.isMenu) { return; }
        if(!System.String.IsNullOrEmpty(guiController.Path)) {
            if(prevPath == guiController.Path) { return; }
            prevPath = guiController.Path;

            var xs = System.IO.Directory.GetFiles(@guiController.Path, "*.jpg").
                       Select(x => LoadBytesFromPath(x));
            UpdateGameObjects(xs);
        } else if(!System.String.IsNullOrEmpty(guiController.Keywords)){
            if(prevKeywords == guiController.Keywords) { return; }
            prevKeywords = guiController.Keywords;

            var xs = pixivAPI.Search(guiController.Keywords).
                       Select(x => LoadBytesFromUrl(x));
            UpdateGameObjects(xs);
        }
    }

}
