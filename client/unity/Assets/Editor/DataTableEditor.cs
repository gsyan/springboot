// ------------------------------------------------------------------------------
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DataTableManager))]
public class DataTableEditor : Editor
{
    private DataTableManager dataTable;

    void OnEnable()
    {
        dataTable = (DataTableManager)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update(); // 직렬화된 객체 업데이트

        DrawDefaultInspector(); // 기본 Inspector 표시


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Module Data Editor", EditorStyles.boldLabel);

        //// modules의 직렬화된 속성 접근
        //SerializedProperty modulesProp = serializedObject.FindProperty("modules");
        //if (modulesProp != null)
        //{
        //    SerializedProperty keysProp = modulesProp.FindPropertyRelative("keys");
        //    SerializedProperty valuesProp = modulesProp.FindPropertyRelative("values");

        //    if (keysProp != null && valuesProp != null)
        //    {
        //        int keyCount = keysProp.arraySize;
        //        int valueCount = valuesProp.arraySize;
        //        int minCount = Mathf.Min(keyCount, valueCount);

        //        for (int i = 0; i < minCount; i++)
        //        {
        //            EModuleType key = (EModuleType)keysProp.GetArrayElementAtIndex(i).intValue;
        //            EditorGUILayout.LabelField($"Type: {key}", EditorStyles.boldLabel);

        //            SerializedProperty valueListProp = valuesProp.GetArrayElementAtIndex(i);
        //            EditorGUI.BeginChangeCheck();
        //            EditorGUILayout.PropertyField(valueListProp, new GUIContent($"Modules for {key}"), true);
        //            if (EditorGUI.EndChangeCheck())
        //            {
        //                serializedObject.ApplyModifiedProperties();
        //                EditorUtility.SetDirty(dataTable);
        //            }

        //            if (GUILayout.Button($"Add New Module to {key}"))
        //            {
        //                dataTable.modules[key].Add(new ModuleData { m_type = key });
        //                EditorUtility.SetDirty(dataTable);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        EditorGUILayout.HelpBox("Could not find keys or values properties.", MessageType.Warning);
        //    }
        //}
        //else
        //{
        //    EditorGUILayout.HelpBox("Modules property not found.", MessageType.Error);
        //}


        




        if (GUILayout.Button("Add Body Module"))
        {
            ModuleData newModule = new ModuleData { m_type = EModuleType.Body };
            dataTable.AddModule(newModule);
            EditorUtility.SetDirty(dataTable);
        }

        if (GUILayout.Button("Add Weapon Module"))
        {
            ModuleData newModule = new ModuleData { m_type = EModuleType.Weapon };
            dataTable.AddModule(newModule);
            EditorUtility.SetDirty(dataTable);
        }

        if (GUILayout.Button("Add Engine Module"))
        {
            ModuleData newModule = new ModuleData { m_type = EModuleType.Body };
            dataTable.AddModule(newModule);
            EditorUtility.SetDirty(dataTable);
        }


        if (GUILayout.Button("Export to JSON"))
        {
            string json = dataTable.ExportToJson();
            string path = EditorUtility.SaveFilePanel("Export DataTable as JSON", "", "DataTable.json", "json");
            if (!string.IsNullOrEmpty(path))
            {
                File.WriteAllText(path, json);
                Debug.Log("JSON exported to: " + path);
            }
        }

        if (GUILayout.Button("Import from JSON"))
        {
            string path = EditorUtility.OpenFilePanel("Import JSON to DataTable", "", "json");
            if (!string.IsNullOrEmpty(path))
            {
                string json = File.ReadAllText(path);
                dataTable.ImportFromJson(json);
                EditorUtility.SetDirty(dataTable); // 변경 사항 저장
                Debug.Log("JSON imported from: " + path);
            }
        }

        serializedObject.ApplyModifiedProperties(); // 모든 변경 사항 적용
    }
}