using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CachingDownload : MonoBehaviour
{
    string assetBundleName = "opening";

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InstantiateObject());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator InstantiateObject()
    {
        string url = "file:///" + Application.dataPath + "/AssetBundles/" + assetBundleName;
        url = "file:///D:/GGAME2022/AssetBundles/" + assetBundleName;
        //url = "http://www.g-gameplay.com/patch_back/" + assetBundleName;
        Debug.Log(url);
        UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequestAssetBundle.GetAssetBundle(url, 0);
        yield return request.SendWebRequest();
        AssetBundle bundle = UnityEngine.Networking.DownloadHandlerAssetBundle.GetContent(request);

        for (int i = 1; i < 8; i++)
        {
            gameManager.pics[i - 1] = bundle.LoadAsset<Texture2D>("op_" + i);
        }

        gameManager.GetComponent<GameManager>().openingStart();
    }
}
