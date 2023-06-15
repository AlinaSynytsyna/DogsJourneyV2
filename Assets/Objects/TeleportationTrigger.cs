﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TeleportationType { InsideLevel, NextLevel, MainMenu }

public class TeleportationTrigger : BaseTrigger
{
    public TeleportationType State;
    public float TeleportPositionX;
    public float TeleportPositionY;

    private PlayerCamera _playerCamera;

    public new void Awake()
    {
        base.Awake();

        IsActive = TeleportTriggersManager.GetIsTeleportTriggerActive(Id);
    }

    public new void OnTriggerEnter2D(Collider2D entity)
    {
        base.OnTriggerEnter2D(entity);

        _playerCamera = FindObjectOfType<PlayerCamera>();
    }

    public void OnTriggerStay2D(Collider2D entity)
    {
        if (entity.CompareTag(PlayerEntityTag) && IsActive)
        {
            ActivePlayer = FindActivePlayer();

            if (ActivePlayer != null && Input.GetKeyDown(CustomInput.Interact))
            {
                var teleportationDelay = 1f;

                switch (State)
                {
                    case TeleportationType.InsideLevel:
                        StartCoroutine(TeleportWithinLevel(teleportationDelay));
                        break;
                    case TeleportationType.NextLevel:
                        StartCoroutine(LoadNextLevel(teleportationDelay));
                        break;
                    case TeleportationType.MainMenu:
                        StartCoroutine(LoadMainMenu(teleportationDelay));
                        break;
                }
            }
        }
    }

    private IEnumerator TeleportWithinLevel(float delay)
    {
        _playerCamera.FadeCameraIn();
        yield return new WaitForSeconds(delay);
        ActivePlayer.transform.position = new Vector3(TeleportPositionX, TeleportPositionY, ActivePlayer.transform.position.z);
        yield return new WaitForSeconds(delay);
        _playerCamera.FadeCameraOut();
    }

    private IEnumerator LoadNextLevel(float delay)
    {
        LevelInfo.SaveProgress();

        _playerCamera.FadeCameraIn();
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator LoadMainMenu(float delay)
    {
        _playerCamera.FadeCameraIn();
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("MainMenu");
    }
}
