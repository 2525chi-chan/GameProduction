using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
using static System.Net.WebRequestMethods;
using Unity.VisualScripting;
using Unity.EditorCoroutines.Editor;

public class BazuriShotScoreFetcher : EditorWindow
{
    public string csvUrl = "https://docs.google.com/spreadsheets/d/e/2PACX-1vTsgWZ09KP4ZxnOoNj2Qu7eNISVDDBzsQsxRuvdcGWvbyK57dODnluo8q8DIow6MNXlMM9BRD95VAZR/pub?output=csv";

    [MenuItem("Tools/Bazuri Sheet Live Importer")]
    public static void ShowWindow()
    {
        GetWindow<BazuriShotScoreFetcher>("Bazuri Live Sheet");
    }

    private void OnGUI()
    {
        csvUrl=EditorGUILayout.TextField("CSV URL",csvUrl);

        if(GUILayout.Button("Fetch and Apply"))
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(FetchandApply(csvUrl));
        }
    }
  private IEnumerator FetchandApply(string url)
    {
        using UnityWebRequest www=UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            yield break;
        }

        ApplyBazuriCSV(www.downloadHandler.text);
    }
    

    private void ApplyBazuriCSV(string csv)
    {
        var lines = csv.Split("\n");
        for(int i=1;i<lines.Length; i++) { 
        var cells = lines[i].Split(',');
            if(cells.Length < 3) {
                continue;
            }
            string objName = cells[0].Trim();
            string tag = cells[1].Trim();
            string score = cells[2].Trim();


            if(!System.Enum.TryParse(tag,out BazuriTag tagValue)) { continue; }
            if(!float.TryParse(score,out float scoreValue)) {  continue; }
        
            var obj=GameObject.Find(objName);
            if (obj == null)
            {
                Debug.LogWarning(objName+"–¢ŒŸo");
                continue;
            }

            var scorable=obj.GetComponent<BazuriShotData>()??obj.AddComponent<BazuriShotData>();

            scorable.tag = tagValue;
            scorable.score = scoreValue;
            EditorUtility.SetDirty(obj);
        }
        Debug.Log("“K—pŠ®—¹");
    }

    
}
