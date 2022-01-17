using UnityEngine;
using UnityEditor;

public class AbBuild : MonoBehaviour
{

    [MenuItem("Bundles/Build AssetBundles")]
    public static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.Android);
        //출처: https://itmining.tistory.com/55?category=640760 [IT 마이닝]
    }


}
