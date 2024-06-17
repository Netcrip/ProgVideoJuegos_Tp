using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static HUDManager Instance => instance;

    private static HUDManager instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {          
            Destroy(gameObject);
        }

        SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
    }

    public void updateHealt(float healt)
    {
        
    }
    public void updateShield(float healt)
    {

    }
    public void updateStamina(float healt)
    {

    }
    public void Destroy()
    {
        Destroy(this);
    }
}
