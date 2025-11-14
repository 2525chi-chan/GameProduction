using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;




[CustomEditor(typeof( LineSpawner))]
public class PrefabLineSpawner : Editor
{
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        LineSpawner spawner = (LineSpawner)target;
        if (GUILayout.Button("Spawn Lines"))
        {
            for (int i = 0; i < spawner.count; i++)
            {
                float t = (spawner.count == 1) ?0.5f:(float)i/(spawner.count-1);
                Vector3 pos = Vector3.Lerp(spawner.startPoint.position, spawner.endPoint.position, t);
                Quaternion rote = spawner.endPoint.rotation;
                var obj = (GameObject)PrefabUtility.InstantiatePrefab(spawner.linePrefab, spawner.transform);
                obj.transform.position = pos;
               obj.transform.rotation =rote;
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
