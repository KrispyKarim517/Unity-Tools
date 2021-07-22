#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace KrispyBuildAnalyzer
{
    public class tool_KrispysBuildAnalyzer : EditorWindow
    {
        #pragma warning disable CS0649
        [SerializeField]
        public GUISkin darkTheme, lightTheme; //Window color theme
        #pragma warning restore CS0649



        //Creates custom unity window
        [MenuItem("Tools/Krispy's Build Analyzer")]
        private static void ToolWindow()
        {
            GetWindow<tool_KrispysBuildAnalyzer>("Krispy's Build Analyzer");
        }



        private void OnGUI()
        {
            //Sets the minimum size of the window
            this.minSize = new Vector2(Screen.currentResolution.width / 4, Screen.currentResolution.height / 4);
            EditorGUIUtility.labelWidth = 75f;

            //Sets color theme based on unity theme
            if (EditorGUIUtility.isProSkin == true)
            {
                GUI.skin = darkTheme; //If unity is in dark mode
            }
            else
            {
                GUI.skin = lightTheme; //If unity is in light mode
            }

            //Title
            GUILayout.Label("Build Analyzer", GUILayout.Width(position.width), GUILayout.Height(75));

            //Tabs
            GUILayout.BeginArea(new Rect(0, 80, position.width / 3, position.height / 2));
            //General Tab
            if (GUILayout.Button("General", GUILayout.Width(position.width / 4), GUILayout.Height(position.height / 10)))
            {
                Debug.Log("Working");
            }
            //Size Data Tab
            if (GUILayout.Button("Size Data", GUILayout.Width(position.width / 4), GUILayout.Height(position.height / 10)))
            {
                Debug.Log("Working");
            }
            //Assets Tab
            if (GUILayout.Button("Assets", GUILayout.Width(position.width / 4), GUILayout.Height(position.height / 10)))
            {
                Debug.Log("Working");
            }
            //Project Settings Tab
            if (GUILayout.Button("Project Settings", GUILayout.Width(position.width / 4), GUILayout.Height(position.height / 10)))
            {
                Debug.Log("Working");
            }
            GUILayout.EndArea();

            //File Options
            GUILayout.BeginArea(new Rect(0, position.height - 50, position.width / 12, 100));
            //Open Player Log
            if (GUILayout.Button("Open\nPlayer\nLog", GUILayout.Width(position.width / 12), GUILayout.Height(50)))
            {
                Debug.Log("open the player log file");
            }
            GUILayout.EndArea();
            GUILayout.BeginArea(new Rect(position.width / 12, position.height - 50, position.width / 12, 100));
            //Open Editor Log
            if (GUILayout.Button("Open\nEditor\nLog", GUILayout.Width(position.width / 12), GUILayout.Height(50)))
            {
                Debug.Log("open the editor log file");
            }
            GUILayout.EndArea();
            GUILayout.BeginArea(new Rect(position.width / 6, position.height - 50, position.width / 12, 100));
            //Export Data
            if (GUILayout.Button("Export\nData", GUILayout.Width(position.width / 12), GUILayout.Height(50)))
            {
                Debug.Log("Create a text file of data");
            }
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(position.width / 4, 80, (4 * position.width) / 3, position.height));
            GUILayout.Box("My Box", GUILayout.Width((3 * position.width) / 4), GUILayout.Height(position.height - 80));
            GUILayout.EndArea();
        }
    }
}
#endif