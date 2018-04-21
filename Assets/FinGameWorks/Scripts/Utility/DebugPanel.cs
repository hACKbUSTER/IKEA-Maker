using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class SROptions {

    [Category("Utilities")] 
    public void LoadRemote() {
        Debug.Log("Load Remote Scene");
        SceneManager.LoadScene(1);
    }
        
    [Category("Utilities")]
    public void LoadMain() {
        Debug.Log("Load Remote Scene");
        SceneManager.LoadScene(0);
    }
}