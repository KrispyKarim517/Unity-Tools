#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace KrispyBuildAnalyzer
{
    public class tool_KrispysBuildAnalyzer : EditorWindow
    {
        #pragma warning disable CS0649
        [SerializeField]
        public GUISkin darkTheme, lightTheme; //Window color theme
        #pragma warning restore CS0649

        Vector2 scrollPosition;
        helper_KrispysBuildAnalyzer helper;
        BuildReport report;
        BuildSummary summary;
        private string currentTab = "General";


        //Creates custom unity window
        [MenuItem("Tools/Krispy's Build Analyzer")]
        private static void ToolWindow()
        {
            GetWindow<tool_KrispysBuildAnalyzer>("Krispy's Build Analyzer");
        }

        //Builds the project
        private void MyBuild()
        {
            BuildPlayerOptions build = new BuildPlayerOptions();
            build.scenes = new[] { "Assets/Scenes/SampleScene.unity" };
            build.locationPathName = "C:/Users/Karim Najib/Desktop/tempUnity";
            build.target = BuildTarget.StandaloneWindows64;
            build.options = BuildOptions.DetailedBuildReport;

            report = BuildPipeline.BuildPlayer(build);
            summary = report.summary;
        }

        private void Awake()
        {
            MyBuild();
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
            GUILayout.Label("Project Build Analyzer", GUILayout.Width(position.width), GUILayout.Height(75));


            //Tabs
            GUILayout.BeginArea(new Rect(0, 80, position.width / 3, position.height));
            //General Tab
            if (GUILayout.Button("General", GUILayout.Width(position.width / 4), GUILayout.Height(position.height / 10)))
            {
                currentTab = "General";
            }
            //Build Steps Tab
            if (GUILayout.Button("Build Steps", GUILayout.Width(position.width / 4), GUILayout.Height(position.height / 10)))
            {
                currentTab = "Build Steps";
            }
            //Size Data Tab
            if (GUILayout.Button("Size Data", GUILayout.Width(position.width / 4), GUILayout.Height(position.height / 10)))
            {
                currentTab = "Size Data";
            }
            //Assets Tab
            if (GUILayout.Button("Assets", GUILayout.Width(position.width / 4), GUILayout.Height(position.height / 10)))
            {
                currentTab = "Assets";
            }
            //Project Settings Tab
            if (GUILayout.Button("Project Settings", GUILayout.Width(position.width / 4), GUILayout.Height(position.height / 10)))
            {
                currentTab = "Project Settigns";
            }
            GUILayout.EndArea();


            //File Options
            GUILayout.BeginArea(new Rect(0, position.height - 50, position.width / 12, 100));
            //Open Player Log
            if (GUILayout.Button("Open\nPlayer\nLog", GUILayout.Width(position.width / 12), GUILayout.Height(50)))
            {
                
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


            //Data
            GUILayout.BeginArea(new Rect(position.width / 4, 80, (4 * position.width) / 3, position.height));
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width((3 * position.width) / 4), GUILayout.Height(position.height - 80));
            switch (currentTab)
            {
                case "General":
                    GUILayout.Label("Total Build Size: " + ((summary.totalSize) * 0.000001).ToString() + " mb");
                    GUILayout.Label("Total Build Time: " + summary.totalTime.ToString());
                    GUILayout.Label("Total Warnings: " + summary.totalWarnings.ToString());
                    GUILayout.Label("Total Errors: " + summary.totalErrors.ToString());
                    GUILayout.Label("Build Platform: " + summary.platform.ToString());
                    GUILayout.Label("Build GUID: " + summary.guid.ToString());
                    GUILayout.Label("Build Options: " + summary.options.ToString());
                    GUILayout.Label("Build Result: " + summary.result.ToString());
                    break;
                case "Build Steps":
                    int stepNum = 0;
                    foreach (BuildStep step in report.steps)
                    {
                        stepNum++;
                        GUILayout.Label("Step " + stepNum + ": " + step.ToString());
                    }
                    break;
                case "Size Data":
                    foreach(PackedAssets br in report.packedAssets)
                    {
                        foreach(PackedAssetInfo item in br.contents)
                        {
                            Debug.Log(item.sourceAssetPath.ToString());
                        }
                    }
                    break;
                case "Assets":
                    break;
                case "Project Settings":
                    
                    break;
            }
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
    }
}
#endif