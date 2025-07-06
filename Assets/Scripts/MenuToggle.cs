using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuToggle : MonoBehaviour
{
    public GameObject OptionsPanel;
    public GameObject MainMenu;

    private bool isPaused = false;

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;

            if (OptionsPanel != null)
                OptionsPanel.SetActive(!OptionsPanel.activeSelf);

            if (MainMenu != null)
                MainMenu.SetActive(!MainMenu.activeSelf);

            Time.timeScale = isPaused ? 0f : 1f;

            Cursor.visible = isPaused;
            Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    public void ResumeGame()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            return;

        if (OptionsPanel != null)
            OptionsPanel.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
