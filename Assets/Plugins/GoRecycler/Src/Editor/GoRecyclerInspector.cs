using UnityEngine;
using UnityEditor;

namespace GoRecycler.Editor
{
    [CustomEditor(typeof(GoRecyclerUtility))]
    internal class GoRecyclerInspector : UnityEditor.Editor
    {
        SerializedProperty pools;

        private void OnEnable()
        {
            pools = serializedObject.FindProperty("Pools");
        }

        void DrawRemoveBtn(int index)
        {
            if (GUILayout.Button("X", EditorStyles.toolbarButton))
            {
                pools.DeleteArrayElementAtIndex(index);
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField(typeof(GoRecyclerUtility).Name, new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 16,
                fontStyle = FontStyle.Bold
            }, GUILayout.Height(30));


            EditorGUILayout.LabelField("Add your object pools here.", new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 10,
                fontStyle = FontStyle.Normal
            }, GUILayout.Height(16));

            EditorGUILayout.Space();

            serializedObject.Update();

            for (int i = 0; i < pools.arraySize; i++)
            {
                SerializedProperty current = pools.GetArrayElementAtIndex(i);

                SerializedProperty _label = current.FindPropertyRelative("label"); ;

                bool isvalid = !string.IsNullOrEmpty(_label.stringValue);

                GoRecyclerEditorHelper.BeginBox();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                current.isExpanded = EditorGUILayout.Foldout(current.isExpanded, isvalid ? _label.stringValue : "No label");

                GUILayout.FlexibleSpace();

                DrawRemoveBtn(i);

                EditorGUILayout.EndHorizontal();

                if (i < pools.arraySize && i >= 0)
                {
                    SerializedProperty _maxitems = current.FindPropertyRelative("MaxItems"); 

                    SerializedProperty _prefab = current.FindPropertyRelative("GoPrefab"); 

                    SerializedProperty _preload = current.FindPropertyRelative("Preload"); 

                    SerializedProperty _preloadcount = current.FindPropertyRelative("Preloadcount"); 

                    SerializedProperty _PreloadIterationTime = current.FindPropertyRelative("PreloadIterationTime");
                    
                    if (current.isExpanded)
                    {

                        EditorGUILayout.Space();

                        _label.stringValue = EditorGUILayout.TextField("Label", _label.stringValue);

                        _maxitems.intValue = EditorGUILayout.IntField("Max Items", _maxitems.intValue > 0 ? _maxitems.intValue : 0);

                        GUIContent preload_content = new GUIContent("Preload", "Do you want to pre-load items of the Pool ?");

                        _preload.boolValue = EditorGUILayout.Toggle(preload_content, _preload.boolValue);

                        if (_preload.boolValue)
                        {
                            GUIContent preloadcount_content = new GUIContent("Preload count", "How many items do you want to pre-load ? ");

                            _preloadcount.intValue = EditorGUILayout.IntField(preloadcount_content, Mathf.Clamp(_preloadcount.intValue,
                                0, _maxitems.intValue));

                            GUIContent preloaditeration_content = new GUIContent("Iteration Time", "how many secons wait between pre-loading each element of the pool ?");

                            _PreloadIterationTime.floatValue = EditorGUILayout.FloatField(preloaditeration_content,
                                Mathf.Clamp(_PreloadIterationTime.floatValue, 0, float.MaxValue));
                        }
                        _prefab.objectReferenceValue = EditorGUILayout.ObjectField("Prefab", _prefab.objectReferenceValue,
                           typeof(GameObject), false);
                    }

                }
                GoRecyclerEditorHelper.Endbox();
            }
            
            if (!Application.isPlaying)
            {
                GoRecyclerEditorHelper.DrawLine();
                if (GUILayout.Button("Add new", GUILayout.Height(30f)))
                {
                    pools.InsertArrayElementAtIndex(pools.arraySize > 0 ? pools.arraySize : 0);
                }
            }
            
            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}