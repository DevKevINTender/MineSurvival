using UnityEngine;
using UnityEditor;
using System.IO;

public class ScriptCreator : EditorWindow
{
    private string scriptName = "Name";

    [MenuItem("Tools/Create Script Template")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ScriptCreator));
    }

    private void OnGUI()
    {
        GUILayout.Label("Create Script Template", EditorStyles.boldLabel);

        scriptName = EditorGUILayout.TextField("Script Name:", scriptName);

        if (GUILayout.Button("Create Scripts"))
        {

            CreateScript(scriptName);
            AssetDatabase.Refresh();
            Debug.Log("Scripts created successfully.");
        }
    }

    private void CreateScript(string scriptName)
    {
        string template =
        @"using Zenject;
        using UnityEngine;

        public class #SCRIPTNAME#View : MonoBehaviour
        {
            // Add your fields and methods here
        }

        public class #SCRIPTNAME#Service : IService
        {
            [Inject] private IViewFabric _viewFabric;
            [Inject] private IMarkerService _markerService;
            private #SCRIPTNAME#View _#SCRIPTNAME#View;

            public void ActivateService()
            {       
                _#SCRIPTNAME#View = _viewFabric.Init<#SCRIPTNAME#View>();
            }

            public void DeactivateService()
            {       
                // Add your deactivation logic here
            }
        }";

        string scriptContent = template.Replace("#SCRIPTNAME#", scriptName);

        // Specify the directory where you want to create the scripts
        string folderPath = $"Assets/Scripts/Logic/{scriptName}/";

        // Create the directory if it doesn't exist
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Specify the file path
        string filePath = folderPath + scriptName + "View.cs";

        // Write the script content to the file
        File.WriteAllText(filePath, scriptContent);
    }
}
