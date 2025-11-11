using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using Unity.EditorCoroutines.Editor;
using Unity.Plastic.Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;

public class BuzzRankDataImporter : EditorWindow
{
    public string jsonUrl = "https://script.google.com/macros/s/AKfycbyiDwvmP8zr3EQKNVwpVs-TyzdAnTV9x1sqHjHH1py73nUGH8CI0CJZAAXRyscUZgdoOw/exec";
    private string targetName = "GoodSystem";
    private GameObject targetObject; // BuzuriRankがアタッチされたオブジェクト
    

    [MenuItem("Tools/BuzzRank Data Importer")]
    public static void ShowWindow()
    {
        GetWindow<BuzzRankDataImporter>("BuzzRank Data Importer");
    }

    private void OnGUI()
    {
        targetObject=GameObject.Find(targetName);

        jsonUrl = EditorGUILayout.TextField("JSON URL", jsonUrl);
        targetObject = (GameObject)EditorGUILayout.ObjectField("ターゲット", targetObject, typeof(GameObject), true);

        if (GUILayout.Button("Fetch and Apply"))
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(FetchAndApply());
        }
    }

    private IEnumerator FetchAndApply()
    {
        using UnityWebRequest www = UnityWebRequest.Get(jsonUrl);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("データ取得失敗：" + www.error);
            yield break;
        }

        var json = www.downloadHandler.text;
        var allSheets = JsonConvert.DeserializeObject<Dictionary<string, List<List<object>>>>(json);

        if (!allSheets.ContainsKey("BuzzRankList") || !allSheets.ContainsKey("Comments"))
        {
            Debug.LogError("必要なシートが見つかりません");
            yield break;
        }

        var rankScript = targetObject.GetComponent<BuzuriRank>();
        if (rankScript == null)
        {
            Debug.LogError("BuzuriRankが見つかりません");
            yield break;
        }

        var buzzRanksField = typeof(BuzuriRank).GetField("buzzRanks", BindingFlags.Public|BindingFlags.NonPublic | BindingFlags.Instance);

        var buzzRanks = buzzRanksField.GetValue(rankScript) as IList;
        if (buzzRanks == null)
        {
           var listType=typeof(List<>).MakeGenericType(typeof(BuzzRank));
            buzzRanks = (IList)Activator.CreateInstance(listType);
        }

        ApplyBuzzRankList(allSheets["BuzzRankList"], buzzRanks);
        ApplyBuzzComments(allSheets["Comments"], buzzRanks);

        buzzRanksField.SetValue(rankScript, buzzRanks);
        EditorUtility.SetDirty(targetObject);
        Debug.Log("BuzzRankデータ適用完了");
    }

    private void ApplyBuzzRankList(List<List<object>> rows, IList buzzRanks)
    {
        if (rows.Count < 2) return;
        var headers = rows[0].Select(cell => cell.ToString().Trim()).ToList();

        for (int i = 1; i < rows.Count; i++)
        {
            var row = rows[i];
            string rankName = row[headers.IndexOf("name")].ToString().Trim();

            var existing = buzzRanks.Cast<object>().FirstOrDefault(r =>
            {
                var nameField = r.GetType().GetField("name",BindingFlags.Public| BindingFlags.NonPublic | BindingFlags.Instance);
                return nameField?.GetValue(r)?.ToString() == rankName;
            });

            var rank = existing ?? Activator.CreateInstance(typeof(BuzzRank));

            for (int j = 0; j < headers.Count; j++)
            {
                var key = headers[j];
                var raw = row[j]?.ToString() ?? "";

                var field = typeof(BuzzRank).GetField(key, BindingFlags.Public|BindingFlags.NonPublic | BindingFlags.Instance);
                if (field == null) continue;

                object converted = ConvertValue(raw, field.FieldType);
                field.SetValue(rank, converted);
            }

            if (existing == null) buzzRanks.Add(rank);

        }
    }

    private void ApplyBuzzComments(List<List<object>> rows, IList buzzRanks)//コメントをスプレッドシートに反映させるメソッド
    {
        if (rows.Count < 2) return;

        // 既存データをクリア
        foreach (var r in buzzRanks.Cast<object>())
        {
            var commentField = r.GetType().GetField("commentContents", BindingFlags.NonPublic | BindingFlags.Instance);
            if (commentField != null)
            {
                var list = commentField.GetValue(r) as IList;
                list?.Clear();
            }
        }

        var lastCommentContentByRank = new Dictionary<string, CommentContent>();

        for (int i = 1; i < rows.Count; i++)
        {
            var row = rows[i];
            if (row.Count < 3) continue;

            string rankNamesRaw = row[0].ToString().Trim();
            string commentType = row[1].ToString().Trim();
            string commentText = row[2].ToString().Trim();



            List<string> targets = rankNamesRaw.ToUpper() == "ALL"//対応するバズリランクを取得
                ? buzzRanks.Cast<object>().Select(r =>
                    r.GetType().GetField("name", BindingFlags.NonPublic | BindingFlags.Instance)
                     ?.GetValue(r)?.ToString()
                  ).ToList()
                : rankNamesRaw.Split(',').Select(s => s.Trim()).ToList();

            foreach (var rankName in targets)
            {
                var rank = buzzRanks.Cast<object>().FirstOrDefault(r =>
                {
                    var nameField = r.GetType().GetField("name", BindingFlags.NonPublic | BindingFlags.Instance);
                    return nameField?.GetValue(r)?.ToString() == rankName;
                });
                if (rank == null) { Debug.Log("bbb"); continue; }

                var commentField = rank.GetType().GetField("commentContents", BindingFlags.Public | BindingFlags.Instance);
                if (commentField == null) {Debug.Log("aaa"); continue;
            }

                var commentList = commentField.GetValue(rank) as IList;
                if (commentList == null)
                {
                    var listType = typeof(List<>).MakeGenericType(typeof(CommentContent));
                    commentList = (IList)Activator.CreateInstance(listType);
                    commentField.SetValue(rank, commentList);
                }

                if (commentType.Equals("Main", StringComparison.OrdinalIgnoreCase))//タイプがメインの場合
                {
                  
                    var newCommentContent = Activator.CreateInstance(typeof(CommentContent));

                   
                    var mainCommentField = typeof(CommentContent).GetField("mainComment", BindingFlags.NonPublic | BindingFlags.Instance);
                    mainCommentField.SetValue(newCommentContent, commentText);

                   

                    commentList.Add(newCommentContent);
                    lastCommentContentByRank[rankName] = (CommentContent)newCommentContent;
                }
                else if (commentType.Equals("Connect", StringComparison.OrdinalIgnoreCase))//タイプが関連コメントの場合。
                {
                    if (lastCommentContentByRank.TryGetValue(rankName, out var lastCommentContent))
                    {
                        var conectCommentField = typeof(CommentContent).GetField("conectComment", BindingFlags.NonPublic | BindingFlags.Instance);
                        var connectList = conectCommentField.GetValue(lastCommentContent) as IList;
                        connectList?.Add(commentText);
                    }
                }
            }
        }
    }


    private object ConvertValue(string raw, Type type)
    {
        try
        {
            if (type == typeof(float)) return float.Parse(raw);
            if (type == typeof(int)) return int.Parse(raw);
            if (type == typeof(bool)) return raw.ToLower() == "true";
            if (type == typeof(Color)) return ColorUtility.TryParseHtmlString(raw, out var c) ? c : Color.white;
            return raw;
        }
        catch
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}
