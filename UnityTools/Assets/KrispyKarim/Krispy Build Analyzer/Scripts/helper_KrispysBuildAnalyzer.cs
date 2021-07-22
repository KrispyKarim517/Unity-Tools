#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace KrispyBuildAnalyzer
{
    public class helper_KrispysBuildAnalyzer : MonoBehaviour
    {
        [MenuItem("Build/Build Report")]
        public static void MyBuild()
        {
            BuildPlayerOptions build = new BuildPlayerOptions();
            build.scenes = new[] { "Assets/Scenes/SampleScene.unity" };
            build.locationPathName = "C:/Users/Karim Najib/Desktop/tempUnity";
            build.target = BuildTarget.StandaloneWindows64;
            build.options = BuildOptions.None;

            BuildReport report = BuildPipeline.BuildPlayer(build);
            BuildSummary summary = report.summary;
        }
    }
}
#endif