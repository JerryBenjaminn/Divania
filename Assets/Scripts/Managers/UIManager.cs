using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject endPanel;

    public static bool isMenuOpen = false;

    private static UIManager _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene"); // Replace "GameScene" with the name of your game scene
    }

    public void OpenOptionsPanel()
    {
        optionsPanel.SetActive(true);
    }

    public void CloseOptionsPanel()
    {
        optionsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowEndPanel()
    {
        endPanel.SetActive(true);
        isMenuOpen = true;
    }

    public void HideEndPanel()
    {
        endPanel.SetActive(false);
        isMenuOpen = false;
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene"); // Replace "GameScene" with the name of your game scene
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu"); // Replace "MainMenu" with the name of your main menu scene
    }

    public void ToggleEndPanel()
    {
        //Check if endpanel is active
        if (endPanel.activeInHierarchy)
        {
            endPanel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            endPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void ResumeGame()
    {
        ToggleEndPanel();
        optionsPanel.SetActive(false);
    }
}
