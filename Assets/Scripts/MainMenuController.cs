using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    GameObject loadingPanel;
    [SerializeField]
    GameObject policyPanel;
    [SerializeField]
    GameObject menuPanel;
    [SerializeField]
    GameObject levelsPanel;
    [SerializeField]
    GameObject policyTextPanel;
    [SerializeField]
    GameObject policyMainPanel;
    [SerializeField]
    GameObject settingsPanel;
    [SerializeField]
    GameObject mainMenuPanel;
    [SerializeField]
    private AudioSource _musicAudioSource;
    [SerializeField]
    private AudioSource _SoundAudioSource;

    [SerializeField]
    GameObject exitPanel;

    [SerializeField] Slider volumeSlider;
    [SerializeField] Slider SoundSlider;
    private bool _privacyAccepted;
    private bool _music;
    [SerializeField]
    private bool isInGame;



    public void acceptPolicy()
    {
        _privacyAccepted = true;
        PlayerPrefs.SetInt("policy_accepted", 1);
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"go to level menu:{PlayerPrefs.GetInt("GoToLevelManager", 0)}");
        _privacyAccepted = (PlayerPrefs.GetInt("policy_accepted", 0) == 1);
        Debug.Log($" Privacy : {_privacyAccepted}");
        if (_privacyAccepted)
        {
            loadingPanel.SetActive(false);
            menuPanel.SetActive(true);
        }
        else
        {
            loadingPanel.SetActive(false);
            policyPanel.SetActive(true);
        }
        SoundSlider.value = _SoundAudioSource.volume = PlayerPrefs.GetFloat("soundVlm", 1);
        volumeSlider.value = _musicAudioSource.volume = PlayerPrefs.GetFloat("musicVlm", 1);
        Debug.Log($"Preference volume music: {PlayerPrefs.GetFloat("musicVlm", 1)}");
        Debug.Log($"Preference volume sound: {PlayerPrefs.GetFloat("soundVlm", 1)}");

        if (PlayerPrefs.GetInt("GoToLevelManager", 0) == 1)
        {
            menuPanel.SetActive(false);
            levelsPanel.SetActive(true);
            PlayerPrefs.SetInt("GoToLevelManager", 0);
        }
    }

    public void ChangeMusic()
    {
        _musicAudioSource.volume = volumeSlider.value;
       
        Debug.Log($"Set value of volume : {volumeSlider.value}");
        PlayerPrefs.SetFloat("musicVlm", volumeSlider.value);
        Debug.Log($"new value of musicVlm : {PlayerPrefs.GetFloat("musicVlm", 1)}");
         }
    public void changeSound()
    {
        _SoundAudioSource.volume = SoundSlider.value;
        PlayerPrefs.SetFloat("soundVlm", SoundSlider.value);
    }

    public void WelcomeOkButtonClicked()
    {
        policyPanel.SetActive(false);
        menuPanel.SetActive(true);
        acceptPolicy();
        _SoundAudioSource.Play();


    }

    public void PolicyButtonClicked()
    {
        policyPanel.SetActive(true);
        menuPanel.SetActive(false);
        policyMainPanel.SetActive(false);
        policyTextPanel.SetActive(true);
        _SoundAudioSource.Play();

    }
    public void PolicyTextBackButtonClicked()
    {
        if (!_privacyAccepted)
        {
            policyPanel.SetActive(true);
            policyMainPanel.SetActive(true);
            policyTextPanel.SetActive(false);
            _SoundAudioSource.Play();
        }
        else
        {
            policyPanel.SetActive(false);
            menuPanel.SetActive(true);
            _SoundAudioSource.Play();
        }
    }

    public void ExitButtonClicked()
    {
        exitPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        _SoundAudioSource.Play();
    }
    public void ExitYesButtonClicked()
    {
        Application.Quit();
    }

    public void ExitNoButtonClicked()
    {
        _SoundAudioSource.Play();
        exitPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void SettingsButtonClicked()
    {
        _SoundAudioSource.Play();
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void MainMenuBackButtonClicked()
    {
        _SoundAudioSource.Play();
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        exitPanel.SetActive(false);
        levelsPanel.SetActive(false);

    }

    public void PlayButtonClicked()
    {
        _SoundAudioSource.Play();
        levelsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }


}
