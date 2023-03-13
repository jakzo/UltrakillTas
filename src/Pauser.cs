using System;
using UnityEngine;

namespace UnityTas {
public class Pauser : MonoBehaviour {
  private float _resumeTimeScale;
  private bool _isPaused;

  public void Update() {
    if (Input.GetKeyDown(KeyCode.P))
      TogglePause();
  }

  private void TogglePause() {
    if (_isPaused)
      Unpause();
    else
      Pause();
  }

  private void Pause() {
    if (_isPaused)
      return;
    _isPaused = true;
    _resumeTimeScale = Time.timeScale;
    Time.timeScale = 0f;
    AudioListener.pause = true;
    Log.Debug("Paused...");
  }

  private void Unpause() {
    if (!_isPaused)
      return;
    _isPaused = false;
    Time.timeScale = _resumeTimeScale;
    AudioListener.pause = false;
    Log.Debug("Unpaused");
  }
}
}
