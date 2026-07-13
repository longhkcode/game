using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Levels : MonoBehaviour
{
    public void Game_Map1()
    {
        SceneManager.LoadScene("Map1");
    }
    public void Game_Map2()
    {
        SceneManager.LoadScene("Map2");
    }
    public void Game_Map3()
    {
        SceneManager.LoadScene("Map3");
    }
}
