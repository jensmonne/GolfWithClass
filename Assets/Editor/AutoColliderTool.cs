using UnityEditor;
using UnityEngine;

public class AutoColliderTool : EditorWindow
{
    private GameObject target;
    private int gridResolution = 4;

    [MenuItem("Tools/Auto Collider Tool")]
    static void Init()
    {
        GetWindow<AutoColliderTool>("Auto Colliders");
    }

    void OnGUI()
    {
        target = (GameObject)EditorGUILayout.ObjectField("Target Object", target, typeof(GameObject), true);
        gridResolution = EditorGUILayout.IntSlider("Resolution", gridResolution, 1, 10);

        if (GUILayout.Button("Generate Colliders"))
        {
            GenerateColliders();
        }
    }

    void GenerateColliders()
    {
        if (!target) return;

        MeshFilter mf = target.GetComponent<MeshFilter>();
        if (!mf)
        {
            Debug.LogError("Target needs a MeshFilter");
            return;
        }

        Bounds b = mf.sharedMesh.bounds;
        Vector3 step = b.size / gridResolution;

        for (int x = 0; x < gridResolution; x++)
        for (int y = 0; y < gridResolution; y++)
        for (int z = 0; z < gridResolution; z++)
        {
            Vector3 offset = new Vector3(
                step.x * (x + 0.5f),
                step.y * (y + 0.5f),
                step.z * (z + 0.5f)
            );

            var child = new GameObject("AutoCollider");
            child.transform.SetParent(target.transform);
            child.transform.localPosition = b.min + offset;
            var bc = child.AddComponent<BoxCollider>();
            bc.size = step;
        }

        Debug.Log($"Generated colliders for {target.name}");
    }
}