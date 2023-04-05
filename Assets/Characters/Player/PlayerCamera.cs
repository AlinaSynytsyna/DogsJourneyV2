using System.Linq;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float MinPosition;
    public float MaxPosition;
    public float MoveSpeed = 1.0f;
    private Camera _camera;

    private Player _activePlayer;
    private Vector3 _playerCameraOffset;
    private ScreenFader _screenFader;

    public void Start()
    {
        _camera = GetComponent<Camera>();
        _activePlayer = GetActivePlayer();
        _playerCameraOffset = transform.position - _activePlayer.transform.position;
        _screenFader = GetComponent<ScreenFader>();
        FadeCameraOut();
    }

    public Camera GetCamera()
    {
        return _camera;
    }

    public void FadeCameraOut()
    {
        _screenFader.fadeState = ScreenFader.FadeState.Out;
        _screenFader.fromOutDelay = Constants.FadeDelay;
        _screenFader.fadeSpeed = Constants.FadeSpeed;
    }

    public void FadeCameraIn()
    {
        _screenFader.fadeState = ScreenFader.FadeState.In;
        _screenFader.fadeSpeed = Constants.FadeSpeed;
    }

    public void LateUpdate()
    {
        if (_activePlayer == null) { return; }

        _playerCameraOffset = Vector3.Lerp(transform.position, _activePlayer.transform.position, MoveSpeed * Time.deltaTime);
        _playerCameraOffset.x = Mathf.Clamp(_playerCameraOffset.x, MinPosition, MaxPosition);
        _playerCameraOffset.y = _activePlayer.transform.position.y + 2.5f;
        _playerCameraOffset.z = transform.position.z;
        transform.position = _playerCameraOffset;
    }

    public Player GetActivePlayer()
    {
        return FindObjectsOfType<Player>().Where(x => x.IsActive).First();
    }
}

