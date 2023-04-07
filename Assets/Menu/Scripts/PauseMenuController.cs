using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public SaveLoadSystem SaveLoadSystem;
    private CustomSettings _customSettings;
    private PlayerCamera _playerCamera;
    private Canvas _pauseMenu;
    private Player _player;
    private bool _isMenuSeen = false;

    public void Start()
    {
        _customSettings = CustomSettingsSaveLoadManager.GetCustomSettings();
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
                PauseGame();
            else
                ResumeGame();
            _isMenuSeen = !_isMenuSeen;
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        _pauseMenu.gameObject.SetActive(true);
        _player.enabled = false;
    }

    public void Quit()
    {
        SaveLoadSystem.SaveCurrentScene();
        foreach (Player Obj in FindObjectsOfType<Player>())
        {
            if (Obj.gameObject.activeInHierarchy)
                SaveLoadSystem.SavePlayerInfo(Obj);
        }
        Time.timeScale = 1;
        _playerCamera.FadeCameraIn();
        Invoke(nameof(QuitButtonPressed), 2F);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        _pauseMenu.gameObject.SetActive(false);
        _player.enabled = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        _player.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitButtonPressed()
    {
        _player.enabled = true;
        SceneManager.LoadScene("MainMenu");
    }
}
