using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class URPGraphicsSettingsManager : MonoBehaviour
{
    public Slider antiAliasingSlider;
    public TMP_Dropdown shadowQualityDropdown;
    public TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    private UniversalRenderPipelineAsset urpAsset;
    private Volume volume;
    private ColorAdjustments colorAdjustments;

    private Resolution[] standardResolutions = new Resolution[]
    {
        new Resolution { width = 1280, height = 720 },
        new Resolution { width = 1366, height = 768 },
        new Resolution { width = 1600, height = 900 },
        new Resolution { width = 1920, height = 1080 },
        new Resolution { width = 2560, height = 1440 },
        new Resolution { width = 3840, height = 2160 }
    };

    void Start()
    {
        urpAsset = (UniversalRenderPipelineAsset)GraphicsSettings.renderPipelineAsset;

        resolutionDropdown.ClearOptions();
        var resolutionOptions = new System.Collections.Generic.List<string>();
        foreach (var resolution in standardResolutions)
        {
            resolutionOptions.Add(resolution.width + "x" + resolution.height);
        }

        resolutionDropdown.AddOptions(resolutionOptions);

        resolutionDropdown.value = GetCurrentResolutionIndex();
        resolutionDropdown.RefreshShownValue();

        fullscreenToggle.isOn = Screen.fullScreen;

        if (Camera.main != null)
        {
            volume = Camera.main.GetComponent<Volume>();
            if (volume == null)
            {
                volume = Camera.main.gameObject.AddComponent<Volume>();
            }

            if (volume.profile == null)
            {
                volume.profile = ScriptableObject.CreateInstance<VolumeProfile>();
            }

            volume.profile.TryGet(out colorAdjustments);

            if (colorAdjustments == null)
            {
                colorAdjustments = volume.profile.Add<ColorAdjustments>();
            }
        }

        if (urpAsset != null)
        {
            antiAliasingSlider.value = urpAsset.msaaSampleCount;
            shadowQualityDropdown.value = urpAsset.shadowCascadeCount;
        }
    }


    public void ApplySettings()
    {
        if (urpAsset != null)
        {
            urpAsset.msaaSampleCount = (int)antiAliasingSlider.value;
            SetShadowQuality(shadowQualityDropdown.value);
        }

        SetScreenResolution(resolutionDropdown.value);
        SetFullscreenMode(fullscreenToggle.isOn); 
    }

    public void SetShadowQuality(int qualityIndex)
    {
        switch (qualityIndex)
        {
            case 0:
                urpAsset.shadowCascadeCount = 1;
                urpAsset.shadowDistance = 0;
                break;
            case 1:
                urpAsset.shadowCascadeCount = 1;
                urpAsset.shadowDistance = 50;
                break;
            case 2:
                urpAsset.shadowCascadeCount = 2;
                urpAsset.shadowDistance = 100;
                break;
            case 3:
                urpAsset.shadowCascadeCount = 4;
                urpAsset.shadowDistance = 150;
                break;
            default:
                urpAsset.shadowCascadeCount = 4;
                urpAsset.shadowDistance = 150;
                break;
        }
    }

    // Функция для изменения разрешения экрана
    public void SetScreenResolution(int index)
    {
        Resolution selectedResolution = standardResolutions[index];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
    }

    // Функция для изменения режима экрана (полноэкранный или оконный)
    public void SetFullscreenMode(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    // Получение текущего индекса разрешения экрана
    private int GetCurrentResolutionIndex()
    {
        for (int i = 0; i < standardResolutions.Length; i++)
        {
            if (standardResolutions[i].width == Screen.currentResolution.width &&
                standardResolutions[i].height == Screen.currentResolution.height)
            {
                return i;
            }
        }
        return 0;
    }

public void CloseSettings()
    {
        gameObject.SetActive(false);
    }
}