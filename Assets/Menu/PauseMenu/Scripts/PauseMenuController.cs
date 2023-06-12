using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class PauseMenuController : MonoBehaviour
{
    private CustomSettings _customSettings;
    private PlayerCamera _playerCamera;
    private Canvas _pauseMenu;
    private Player _player;
    private bool _isMenuSeen = false;
    private readonly DialogueRunner _dialogueRunner;

    public void Start()
    {
        _customSettings = CustomSettingsManager.GetCustomSettings();
        _playerCamera = GetComponentInParent<PlayerCamera>();
        _pauseMenu = FindObjectsOfType<Canvas>().Where(x => x.name.Equals("PauseMenu")).First();
        _player = _playerCamera.GetActivePlayer();
        _pauseMenu.gameObject.SetActive(false);
        _player.enabled = true;
    }

    public void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_isMenuSeen)
                Pause();
            else
                Resume();
            _isMenuSeen = !_isMenuSeen;
        }
    }

    public void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            _isMenuSeen = true;
            Pause();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        _pauseMenu.gameObject.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        _pauseMenu.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        LevelManager.IsReloadingLevel = true;
        HealingObjectsManager.MarkAllHealingObjectsAsActive();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Time.timeScale = 1;

        LevelInfo.SaveProgress();

        _playerCamera.FadeCameraIn();
        Invoke(nameof(QuitButtonPressed), 1.5F);
    }

    private void QuitButtonPressed()
    {
        _player.enabled = true;
        SceneManager.LoadScene("MainMenu");
    }
}
