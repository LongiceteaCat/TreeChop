using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour

{
    private Scene scene;
    
    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
    }

   
    void Update()
    {
        
    }
    public void RestartScene(){
        SceneManager.LoadScene(scene.name);
    }
}
