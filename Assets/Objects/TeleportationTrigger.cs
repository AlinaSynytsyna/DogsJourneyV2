using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportationTrigger : BaseTrigger
{
    public enum TeleportationType { InsideLevel, NextLevel, PreviousLevel, MainMenu }

    public TeleportationType State;
    public float TeleportPositionX;
    public float TeleportPositionY;

    private PlayerCamera _playerCamera;

    public new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        _playerCamera = FindObjectOfType<PlayerCamera>();
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        var teleportationDelay = 1f;

        if (Input.GetKeyDown(CustomInput.Interact))
        {
            switch (State)
            {
                case TeleportationType.InsideLevel:
                    StartCoroutine(TeleportWithinLevel(teleportationDelay));
                    break;
                case TeleportationType.NextLevel:
                    StartCoroutine(LoadNextLevel(teleportationDelay));
                    break;
                case TeleportationType.PreviousLevel:
                    StartCoroutine(LoadPreviousLevel(teleportationDelay));
                    break;
                case TeleportationType.MainMenu:
                    StartCoroutine(LoadNextLevel(teleportationDelay));
                    break;
            }
        }
    }

    private IEnumerator TeleportWithinLevel(float Delay)
    {
        _playerCamera.FadeCameraIn();
        yield return new WaitForSeconds(Delay);
        ActivePlayer.transform.position = new Vector3(TeleportPositionX, TeleportPositionY, ActivePlayer.transform.position.z);
        yield return new WaitForSeconds(Delay);
        _playerCamera.FadeCameraOut();
    }

    private IEnumerator LoadNextLevel(float Delay)
    {
        _playerCamera.FadeCameraIn();
        yield return new WaitForSeconds(Delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator LoadPreviousLevel(float Delay)
    {
        _playerCamera.FadeCameraIn();
        yield return new WaitForSeconds(Delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    private IEnumerator LoadMainMenu(float Delay)
    {
        _playerCamera.FadeCameraIn();
        yield return new WaitForSeconds(Delay);
        SceneManager.LoadScene("MainMenu");
    }
}
