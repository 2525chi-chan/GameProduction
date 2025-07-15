using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
using static System.Net.WebRequestMethods;
using System.Reflection;
using Unity.EditorCoroutines.Editor;
using Unity.Plastic.Newtonsoft.Json;
using System.Security.Cryptography;
using System.Runtime.Remoting.Messaging;
using System;

public class SpreadSheetDataFetcher : EditorWindow
{
    public string jsonUrl = "https://script.google.com/macros/s/AKfycbyiDwvmP8zr3EQKNVwpVs-TyzdAnTV9x1sqHjHH1py73nUGH8CI0CJZAAXRyscUZgdoOw/exec";
    private bool applyAllSheets = true;
    private string targetSheetName = "";

    [MenuItem("Tools/Sheet Data Applier")]
    public static void ShowWindow()
    {
        GetWindow<SpreadSheetDataFetcher>("Sheet Data Applier");
    }

    private void OnGUI()
    {
        jsonUrl=EditorGUILayout.TextField("JSON URL", jsonUrl);
        applyAllSheets=EditorGUILayout.Toggle("Apply All Sheets",applyAllSheets);

        if (!applyAllSheets)
        {
            targetSheetName = EditorGUILayout.TextField("Target Sheet", targetSheetName);

        }

        if(GUILayout.Button("Fetch and Apply"))
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(FetchandApply());
        }
    }
  private IEnumerator FetchandApply()
    {
        using UnityWebRequest www=UnityWebRequest.Get(jsonUrl);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            yield break;
        }
        var json = www.downloadHandler.text;
        var allSheets = JsonConvert.DeserializeObject<Dictionary<string, List<List<object>>>>(json);
        if (applyAllSheets)
        {
            foreach (var sheet in allSheets)
            {
                ApplySheetData(sheet.Key, sheet.Value);
            }
        }
        else
        {
            if (allSheets.ContainsKey(targetSheetName))
            {
                ApplySheetData(targetSheetName, allSheets[targetSheetName]);
            }
            else
            {
                Debug.LogWarning("Sheet Not Found");
            }
        }
        Debug.Log("“K—pŠ®—¹");
    }


    private void ApplySheetData(string sheetName, List<List<object>> rows)
    {
        if (rows.Count < 2) return;

        List<string> headers = new List<string>();

        foreach (var cell in rows[0])
        {

            headers.Add(cell.ToString().Trim());
        }
        
        Type scriptType =GetTypeByName(sheetName);


        if (scriptType == null)
        {
            Debug.LogWarning(sheetName + "Not Found");
            return;
        }
        for (int i = 1; i < rows.Count; i++)
        {
            var row = rows[i];
            if (row.Count < 1) continue;

            string objName = row[0].ToString().Trim();
            GameObject obj = GameObject.Find(objName);
            if (obj != null)
            {
                ApplyToObject(obj, scriptType, headers, row);
                continue;
            }

            string[] guids = AssetDatabase.FindAssets($"{objName} t:Prefab");
    
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
               
                if (prefab == null)
                {
                    Debug.Log("prefab Not Found"); continue;
                }

                 ApplyToObject(prefab, scriptType, headers, row);
               
                PrefabUtility.SaveAsPrefabAsset(prefab, path);
             
                break;


            }
        }

    }
    private void ApplyToObject(GameObject obj, Type scriptType, List<string> headers, List<object> row)
    {
        var compornent = obj.GetComponent(scriptType) ?? obj.AddComponent(scriptType);

        for (int j = 1; j < headers.Count; j++)
        {
            string fieldName = headers[j];
            string rawValue = row[j]?.ToString() ?? "";


            FieldInfo field = scriptType.GetField(fieldName);

            if (field == null)
            {
                PropertyInfo property = scriptType.GetProperty(fieldName,BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic);

                if (property != null && property.CanWrite)
                {
                    object p_converted = ConvertValue(rawValue, property.PropertyType);
                    property.SetValue(compornent, p_converted);
                    continue;

                }
                else
                {
                    Debug.Log("ha?");
                }
            }
            else
            {
                object f_converted = ConvertValue(rawValue, field.FieldType);
                field.SetValue(compornent, f_converted);
            }

           

        }
        EditorUtility.SetDirty(obj);
    }
    private Type GetTypeByName(string typeName)
    {
        foreach(var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach(var type in asm.GetTypes())
            {
                if(type.Name == typeName) return type;  
            }
        }
        return null;
    }
    private object ConvertValue(string raw,Type type)
    {
        try
        {
            if(type.IsEnum)return Enum.Parse(type, raw,ignoreCase:true);
            if (type == typeof(float)) return float.Parse(raw);
            if (type == typeof(int)) return int.Parse(raw);
            if (type == typeof(bool)) return raw.ToLower() == "true";

            return raw;
        }
        catch
        {
            return type.IsValueType?Activator.CreateInstance(type) : null;
        }
    }
    }

   

    

