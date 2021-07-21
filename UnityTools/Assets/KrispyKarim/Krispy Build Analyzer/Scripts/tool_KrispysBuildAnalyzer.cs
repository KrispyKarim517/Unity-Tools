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
            

            //Sets color theme based on unity theme
            if (EditorGUIUtility.isProSkin == true)
            {
                GUI.skin = darkTheme; //If unity is in dark mode
            }
            else
            {
                GUI.skin = lightTheme; //If unity is in light mode
            }
        }
    }
}
#endif