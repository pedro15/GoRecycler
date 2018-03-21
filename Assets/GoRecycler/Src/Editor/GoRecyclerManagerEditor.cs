using UnityEngine;
using UnityEditor;
using GoRecycler;

namespace GoRecyclerEditor
{
    [CustomEditor(typeof(GoRecyclerManager))]
    internal class GoRecyclerManagerEditor : Editor
    {
        /// <summary>
        /// Recyclebin List
        /// </summary>
        SerializedProperty pools;

        private void OnEnable()
        {
            pools = serializedObject.FindProperty("Pools");
        }


        void RemoveButton(int index)
        {
            if (GUILayout.Button(new GUIContent("X")))
            {
                pools.DeleteArrayElementAtIndex(index);
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Manage your object pools here", new GUIStyle(EditorStyles.label)
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

                GUILayout.BeginVertical(GUI.skin.box);

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(10);

                current.isExpanded = EditorGUILayout.Foldout(current.isExpanded, isvalid ? _label.stringValue : "-No label-");

                GUILayout.FlexibleSpace();

                RemoveButton(i);

                EditorGUILayout.EndHorizontal();

                if (i < pools.arraySize && i >= 0)
                {
                    SerializedProperty _maxitems = current.FindPropertyRelative("MaxItems"); 

                    SerializedProperty _prefab = current.FindPropertyRelative("Prefab"); 
                    
                    SerializedProperty _preloadcount = current.FindPropertyRelative("PreAllocateCount");

                    SerializedProperty _PoolParent = current.FindPropertyRelative("PoolParent");


                    if (current.isExpanded)
                    {

                        EditorGUILayout.Space();

                        _label.stringValue = EditorGUILayout.TextField("Label", _label.stringValue);

                        _maxitems.intValue = EditorGUILayout.IntField("Max Items", Mathf.Clamp(_maxitems.intValue , 1 , int.MaxValue));
                        
                        EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);
                        
                        _preloadcount.intValue = EditorGUILayout.IntField("Pre-Allocate Count",
                            Mathf.Clamp(_preloadcount.intValue, 0, _maxitems.intValue));

                        _prefab.objectReferenceValue = EditorGUILayout.ObjectField("Prefab", _prefab.objectReferenceValue,
                           typeof(GameObject), false);

                        _PoolParent.objectReferenceValue = EditorGUILayout.ObjectField("Prefab Parent", _PoolParent.objectReferenceValue,
                            typeof(Transform), true);

                        EditorGUI.EndDisabledGroup();
                    }

                }

                GUILayout.Space(2);
                GUILayout.EndVertical();
            }


            if (!Application.isPlaying)
            {
                GUILayout.Space(2);

                GUILayout.BeginHorizontal();

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Add new", GUI.skin.FindStyle("LargeButton") , GUILayout.Height(25f),GUILayout.Width(150)))
                {
                    int index = pools.arraySize > 0 ? pools.arraySize : 0;
                    pools.InsertArrayElementAtIndex(index);
                    SerializedProperty prop = pools.GetArrayElementAtIndex(index);
                    SerializedProperty label = prop.FindPropertyRelative("label");
                    label.stringValue = string.Empty;
                }

                GUILayout.FlexibleSpace();

                GUILayout.EndHorizontal();
                GUILayout.Space(5f);
            }
            
            
            serializedObject.ApplyModifiedProperties();
            
        }
    }
}
