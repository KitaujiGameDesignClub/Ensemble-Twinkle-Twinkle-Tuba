using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Windows.Forms;
using UnityEngine.UI;

public class Tools : MonoBehaviour
{
  public Image BG;

  

  string defaultPath = "";
  private string selectDir;
  
  void Start () {
      defaultPath = YamlReadWrite.UnityButNotAssets;
  }
  
public void SelectFolder() 
  {

      DirectoryInfo mydir = new DirectoryInfo(defaultPath);
      if(!mydir.Exists)
      {
          MessageBox.Show("请先创建资源文件夹");
          return;
      } 

      try
      {
          FolderBrowserDialog fbd = new FolderBrowserDialog(); 
          fbd.Description = "保存路径"; 
          fbd.ShowNewFolderButton = true;  
          fbd.RootFolder = Environment.SpecialFolder.MyComputer;//设置默认打开路径
          fbd.SelectedPath = defaultPath;  //默认打开路径下的详细路径

          if (fbd.ShowDialog() == DialogResult.OK) 
          { 
              defaultPath = fbd.SelectedPath; 
              selectDir = fbd.SelectedPath; 
          } 
      }
      catch(Exception e)
      {
          Debug.LogError("打开错误："+e.Message);
          return;
      }
  } 
  
 public void SelectFile() 
  { 
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.InitialDirectory = defaultPath;
      ofd.Filter = "*.jpg";
      if (ofd.ShowDialog() == DialogResult.OK) 
      { 
          Debug.Log(ofd.FileName); 
      } 
  }

}
