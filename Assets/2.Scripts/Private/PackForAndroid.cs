#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
 
public class PackForAndroid : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    public int callbackOrder => 0;
 
    public void OnPreprocessBuild(BuildReport report)
    {
        //1=Windows平台 0是Android平台
        if (File.ReadAllLines($"{YamlReadWrite.UnityButNotAssets}/Platform.ini")[0] == "1")
        {
            Debug.Log(File.ReadAllLines($"{YamlReadWrite.UnityButNotAssets}/Platform.ini")[0]);
            return;
        }

        //打包前，将外部目录下的文件，复制到Resources文件夹中
        Directory.CreateDirectory($"{Application.dataPath}/Resources/Dialogue/yaml");//用于储存小剧场的清单文件
       Directory.CreateDirectory($"{Application.dataPath}/Resources/Dialogue/Images");//用于储存小剧场的图片
       Directory.CreateDirectory($"{Application.dataPath}/Resources/saves");
       //小剧场的yaml清单移动到Dialogue/yaml文件夹中
       DirectoryInfo directoryInfo = new DirectoryInfo($"{YamlReadWrite.UnityButNotAssets}/Dialogue");
       FileInfo[] manifests = directoryInfo.GetFiles("*.yaml");
       for (int i = 0; i < manifests.Length; i++)
       {
           File.Copy(manifests[i].FullName,$"{Application.dataPath}/Resources/Dialogue/yaml/{manifests[i].Name}");
       }
       //小剧场的图片移动到Dialogue/Images文件夹中
        manifests = directoryInfo.GetFiles("*.jpg");
       for (int i = 0; i < manifests.Length; i++)
       {
           File.Copy(manifests[i].FullName,$"{Application.dataPath}/Resources/Dialogue/Images/{manifests[i].Name}");
       }
       //移动saves文件夹中所有的Yaml文件到saves文件夹中
       directoryInfo = new DirectoryInfo($"{YamlReadWrite.UnityButNotAssets}/saves");
       manifests = directoryInfo.GetFiles("*.yaml");
       for (int i = 0; i < manifests.Length; i++)
       {
           File.Copy(manifests[i].FullName,$"{Application.dataPath}/Resources/saves/{manifests[i].Name}");
       }
     
    }
 
    public void OnPostprocessBuild(BuildReport report)
    {
        //编译完了，删掉Resources文件夹
     
        Directory.Delete($"{Application.dataPath}/Resources/Dialogue");
        Directory.Delete($"{Application.dataPath}/Resources/saves");

    
    }
}
#endif
