using UnityEngine;

public class MenuToggle : MonoBehaviour
{
    public GameObject OptionsPanel;
    public GameObject MainMenu; // May or may not be assigned depending on scene

    private bool isPaused = false;

    void Update()
    {
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
        if (OptionsPanel != null)
            OptionsPanel.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
