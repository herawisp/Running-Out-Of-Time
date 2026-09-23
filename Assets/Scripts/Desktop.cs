using UnityEngine;
using UnityEngine.SceneManagement;

public class Desktop : MonoBehaviour
{
    public void GoToPlanner()
    {
        SceneManager.LoadScene("Planner");
    }

    public void GoToAcademics()
    {
        SceneManager.LoadScene("Academic");
    }
}