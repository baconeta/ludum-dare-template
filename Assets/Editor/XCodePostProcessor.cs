#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

public class XCodePostProcessor : MonoBehaviour
{
    [PostProcessBuild(1)]
    public static void OnPostProcessBuild(BuildTarget target, string buildPath)
    {
        if (target == BuildTarget.iOS)
        {
            ModifyXcodeproj(buildPath);
            ModifyInfoplist(buildPath);
        }
    }

    private static void ModifyXcodeproj(string buildPath)
    {
        Debug.Log("Configuring Xcode project with environment variables...");

        // Read config from file.
        string projectPath = PBXProject.GetPBXProjectPath(buildPath);
        PBXProject project = new PBXProject();
        project.ReadFromFile(projectPath);


        // Get environment variables.

        // Temporarily hardcode values, we were having issues with environment variables.
        // TODO Fix fetching from environmental variables.
        bool useAutomaticSigning = false;
        bool devMode = false;
        string provisioningProfile = "65fab9de-6381-4cd2-8c07-a79d40698449";
        string devTeam = "3W74XNV66H";

        //bool useAutomaticSigning = Environment.GetEnvironmentVariable("IOS_AUTOMATIC_SIGNING") == "true";
        //bool devMode = Environment.GetEnvironmentVariable("IOS_DEV_MODE") == "true";
        //string provisioningProfile = Environment.GetEnvironmentVariable("IOS_PROVISIONING_PROFILE_SPECIFIER") ?? "";
        //string devTeam = Environment.GetEnvironmentVariable("IOS_DEVELOPMENT_TEAM") ?? "";
        string codeSignIdentity = devMode ? "Apple Development" : "Apple Distribution";

        Debug.Log($"Using automatic signing '{useAutomaticSigning}'.");
        Debug.Log($"Using provisioning profile '{provisioningProfile}'.");
        Debug.Log($"Using development team '{devTeam}'.");
        Debug.Log($"Using code signing identity '{codeSignIdentity}'.");

        string iPhoneUnityTarget = project.GetUnityMainTargetGuid();
        string unityFramework = project.GetUnityFrameworkTargetGuid();
        string releaseConfigGuid = project.BuildConfigByName(iPhoneUnityTarget, (devMode ? "Debug" : "Release"));

        // Set CODE_SIGN_IDENTITY at target-level AND at release-level.
        project.SetBuildProperty(iPhoneUnityTarget, "CODE_SIGN_IDENTITY", codeSignIdentity);
        //project.SetBuildProperty(unityFramework, "CODE_SIGN_IDENTITY", codeSignIdentity);
        project.SetBuildPropertyForConfig(releaseConfigGuid, "CODE_SIGN_IDENTITY", codeSignIdentity);

        // Set DEVELOPMENT_TEAM at target-level AND at release-level. Also set for UnityFramework target.
        project.SetBuildProperty(iPhoneUnityTarget, "DEVELOPMENT_TEAM", devTeam);
        project.SetBuildProperty(unityFramework, "DEVELOPMENT_TEAM", devTeam);
        project.SetBuildPropertyForConfig(releaseConfigGuid, "DEVELOPMENT_TEAM", devTeam);

        // Set CODE_SIGN_STYLE and PROVISIONING_PROFILE_SPECIFIER at the release-level ONLY.
        if (useAutomaticSigning)
        {
            // Configure automatic signing.
            project.SetBuildPropertyForConfig(releaseConfigGuid, "CODE_SIGN_STYLE", "Automatic");
            // Leave PROVISIONING_PROFILE_SPECIFIER empty, Xcode will handle it automatically.
        }
        else
        {
            // Configure manual signing.
            project.SetBuildPropertyForConfig(releaseConfigGuid, "CODE_SIGN_STYLE", "Manual");
            project.SetBuildPropertyForConfig(releaseConfigGuid, "PROVISIONING_PROFILE_SPECIFIER", provisioningProfile);
        }

        // Save changes.
        project.WriteToFile(projectPath);
        Debug.Log("Finished configuring Xcode project.");
    }

    private static void ModifyInfoplist(string buildPath)
    {
        // Read Info.plist file.
        string plistPath = Path.Combine(buildPath, "Info.plist");
        var plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        // Set CFBundleShortVersionString to Unity version
        plist.root.SetString("CFBundleShortVersionString", Application.version);
        Debug.Log("Using marketing version string '" + Application.version + "'.");

        // Persist changes to file.
        plist.WriteToFile(plistPath);
        Debug.Log("Finished configuring Info.plist file.");
    }
}
#endif
