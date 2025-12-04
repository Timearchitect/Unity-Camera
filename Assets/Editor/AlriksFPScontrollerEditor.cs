using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AlriksFPSController))]

public class AlriksFPSControllerEditor : Editor
{

    SerializedProperty canMove;
    SerializedProperty isRunning;
    SerializedProperty isScoped;
  
    void OnEnable()
    {
        isRunning = serializedObject.FindProperty("isRunning");

        
    }
    public override void OnInspectorGUI()
    {
        #region draw custom row with bools
        serializedObject.Update();

        GUILayout.BeginHorizontal();

 
        EditorGUILayout.PropertyField(isRunning, new GUIContent("isRunning"));
        EditorGUILayout.PropertyField(isScoped, new GUIContent("isScoped"));
        //EditorGUI.PropertyField(isScoped, new GUIContent("Value A"));

        GUILayout.EndHorizontal();


        serializedObject.ApplyModifiedProperties();

        #endregion

        #region draw Inspektor utan bool

        //DrawDefaultInspector();
        DrawPropertiesExcluding(serializedObject, "isRunning", "isScoped");
        #endregion

        #region button
        AlriksFPSController fpsController = (AlriksFPSController)target;
        // Start horizontal layout
        GUILayout.BeginHorizontal();

        // Flexible space pushes element toward center
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Reset position", GUILayout.Width(200), GUILayout.Height(40)))
        {
            fpsController.Reset();
        }
        // Add flexible space on the other side
        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();
        #endregion 
    }
}
