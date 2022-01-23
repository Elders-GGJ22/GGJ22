using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LoadPrefs : MonoBehaviour
{
    [Header("General Setting")]
    [SerializeField] private bool _canUse = false;
    [SerializeField] private MenuController menuController;

    [Header("Volume Setting")]
    [SerializeField] private TMP_Text _volumeTextValue = null;
    [SerializeField] private Slider _volumeSlider = null;

    [Header("Brightness Setting")]
    [SerializeField] private TMP_Text _brightnessTextValue = null;
    [SerializeField] private Slider _brightnessSlider = null;

    [Header("Quality Setting")]
    [SerializeField] private TMP_Dropdown _qualityDropdown;

    [Header("Fullscreen Setting")]
    [SerializeField] private Toggle _fullScreenToggle;

    [Header("Sensitivity Setting")]
    [SerializeField] private TMP_Text _controllerSenTexValue = null;
    [SerializeField] private Slider _controllerSenslider = null;

    [Header("Invert Y Setting")]
    [SerializeField] private Toggle _invertYToggle = null;

    private void Awake()
    {
        if (_canUse)
        {
            if (PlayerPrefs.HasKey("Volume"))
            {
                float localVolume = PlayerPrefs.GetFloat("Volume");

                _volumeTextValue.text = localVolume.ToString("0.0");
                _volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }
            else
            {
                menuController.ResetButton("Audio");
            }

            if (PlayerPrefs.HasKey("Quality"))
            {
                int localQuality = PlayerPrefs.GetInt("Quality");
                _qualityDropdown.value = localQuality;
                QualitySettings.SetQualityLevel(localQuality);
            }

            if (PlayerPrefs.HasKey("FullScreen"))
            {
                int localFullscreen = PlayerPrefs.GetInt("FullScreen");

                if (localFullscreen == 1)
                {
                    Screen.fullScreen = true;
                    _fullScreenToggle.isOn = true;
                }
                else
                {
                    Screen.fullScreen = false;
                    _fullScreenToggle.isOn = false;
                }
            }

            if (PlayerPrefs.HasKey("Brightness"))
            {
                float localBrightness = PlayerPrefs.GetInt("Brightness");

                _brightnessTextValue.text = localBrightness.ToString("0.0");
                _brightnessSlider.value = localBrightness;
            }

            if (PlayerPrefs.HasKey("Sensitivity"))
            {
                float localSensitivity = PlayerPrefs.GetInt("Sensitivity");

                _controllerSenTexValue.text = localSensitivity.ToString("0.0");
                _controllerSenslider.value = localSensitivity;
                menuController.MainControllerSen = Mathf.RoundToInt(localSensitivity);
            }

            if (PlayerPrefs.HasKey("InvertY"))
            {
                if (PlayerPrefs.GetInt("InvertY") == 1)
                {
                    _invertYToggle.isOn = true;
                }
                else
                {
                    _invertYToggle.isOn = false;
                }
            }
        }
    }
}
