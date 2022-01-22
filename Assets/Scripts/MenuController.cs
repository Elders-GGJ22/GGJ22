using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("Volume Settings")]
    [SerializeField] private TMP_Text _volumeTexValue = null;
    [SerializeField] private Slider _volumeSlider = null;
    [SerializeField] private float _defaultVolume = 0.5f;

    [Header("Gameplay Settings")]
    [SerializeField] private TMP_Text _controllerSenTexValue = null;
    [SerializeField] private Slider _controllerSenslider = null;
    [SerializeField] private int _defaultSen = 5;
    public int MainControllerSen = 5;

    [Header("Toggle Settings")]
    [SerializeField] private Toggle _invertYToggle = null;

    [Header("Graphics Settings")]
    [SerializeField] private TMP_Text _brightnessTextValue = null;
    [SerializeField] private Slider _brightnessSlider = null;
    [SerializeField] private float _defaultBrightness = 1;

    [Space(10)]
    [SerializeField] private TMP_Dropdown _qualityDropdown;
    [SerializeField] private Toggle _fullScreenToggle;

    private int _qualityLevel;
    private bool _isFullScreen;
    private float _brightnessLevel;

    [Header("Confirmation")]
    [SerializeField] private GameObject confirmationPrompt = null;

    [Header("Levels To Load")]
    public string NewGameLevel;
    private string _levelToLoad;
    [SerializeField] private GameObject _noSavedGameDialog = null;

    [Header("Resolution Dropdowns")]
    public TMP_Dropdown ResolutionDropdown;
    private Resolution[] _resolutions;

    private void Start()
    {
        _resolutions = Screen.resolutions;
        ResolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height;
            options.Add(option);

            if (_resolutions[i].width == Screen.width && _resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = currentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(NewGameLevel);
    }

    public void LoadGameDialogYes()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            _levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(_levelToLoad);
        }
        else
        {
            _noSavedGameDialog.SetActive(true);
        }
    }

    public void Exitbutton()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        _volumeTexValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("Volume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public void SetCOntrollerSen(float sensitivity)
    {
        MainControllerSen = Mathf.RoundToInt(sensitivity);
        _controllerSenTexValue.text = sensitivity.ToString("0");
    }

    public void GameplayApply()
    {
        if (_invertYToggle.isOn)
        {
            PlayerPrefs.SetInt("InvertY", 1);
        }
        else
        {
            PlayerPrefs.SetInt("InvertY", 0);
        }
        PlayerPrefs.SetFloat("Sensitivity", MainControllerSen);
        StartCoroutine(ConfirmationBox());
    }

    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        _brightnessTextValue.text = brightness.ToString("0.0");
    }

    public void SetFullScreen(bool isFullscreen)
    {
        _isFullScreen = isFullscreen;
    }

    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("Brightness", _brightnessLevel);
        //Change brigthness with post process...

        PlayerPrefs.SetInt("Quality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);

        PlayerPrefs.SetInt("FullScreen", (_isFullScreen ? 1 : 0));
        Screen.fullScreen = _isFullScreen;
        StartCoroutine(ConfirmationBox());
    }
    public void ResetButton(string MenuType)
    {
        if (MenuType == "Graphics")
        {
            _brightnessSlider.value = _defaultBrightness;
            _brightnessTextValue.text = _defaultBrightness.ToString("0.0");

            _qualityDropdown.value = 1;
            QualitySettings.SetQualityLevel(1);

            _fullScreenToggle.isOn = false;
            Screen.fullScreen = false;

            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            ResolutionDropdown.value = _resolutions.Length;
            GraphicsApply();
        }
        if (MenuType == "Audio")
        {
            AudioListener.volume = _defaultVolume;
            _volumeSlider.value = _defaultVolume;
            _volumeTexValue.text = _defaultVolume.ToString("0.0");
            VolumeApply();
        }
        if (MenuType == "Gameplay")
        {
            _controllerSenTexValue.text = _defaultSen.ToString("0");
            _controllerSenslider.value = _defaultSen;
            MainControllerSen = _defaultSen;
            _invertYToggle.isOn = false;
            GameplayApply();
        }
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }
}
