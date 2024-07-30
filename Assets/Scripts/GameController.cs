using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public float elapsedTime;
    private int secondsElapsed = 0;
    private bool isPause = false;

    public Text timerDisplay;
    public Text winPanelTimerDisplay;


    public GameObject[] levels;
    public GameObject WinPanel;
    public GameObject puzzleContainer;
    public GameObject settingsContainer;

    [SerializeField]
    private Sprite bgImage;
    public Sprite[] puzzleSprites;

    private int playerScore;
    public Text scoreDisplay;
    public Text winPanelScoreDisplay;

    public List<Button> puzzleButtons;
    public List<Sprite> gamePuzzleSprites  = new List<Sprite>();

    private bool isFirstGuess, isSecondGuess;

    private int totalGuesses;
    private int correctGuesses;
    private int requiredGuesses;

    private int firstGuessIndex, secondGuessIndex;
    private string firstGuessPuzzle, seconGuessPuzzle;
    private int totalLevels;
    private const int totalUniqueCards = 8;

    [SerializeField]
    private AudioSource _MusicAudioSource; 
    [SerializeField]
    private AudioSource _SoundAudioSource;
    [SerializeField]
    private AudioSource _WinAudioSource;

    [SerializeField] Slider volumeSlider;
    [SerializeField] Slider SoundSlider;
    private float _sound; 
    private float _music;


    private void Awake()
    {
        puzzleContainer.SetActive(true);
        WinPanel.SetActive(false);
        requiredGuesses = 0;

        for (int i = 0; i < puzzleButtons.Count; i++)
        {
            puzzleButtons[i].name = "" + i;
        }
        puzzleSprites = Resources.LoadAll<Sprite>("Gems");
    }
    void Start()
    {
        _sound = PlayerPrefs.GetFloat("soundVlm", 1);
        _music = PlayerPrefs.GetFloat("musicVlm", 1);
        SoundSlider.value = _MusicAudioSource.volume = _WinAudioSource.volume = _sound;
        volumeSlider.value= _SoundAudioSource.volume = _music;
        OnPuzzleButtonClick();
        LoadGamePuzzles();
        UpdateTimerDisplay();
        requiredGuesses = gamePuzzleSprites .Count / 2;
    }

    private void Update()
    {
        if ((correctGuesses < requiredGuesses)&&(!isPause)) 
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 1f)
            {
                secondsElapsed++;
                elapsedTime = 0f;
                UpdateTimerDisplay();
            }
        }
    }
    void LoadGamePuzzles()
    {
        List<Sprite> tempList = new List<Sprite>(puzzleSprites);
        for (int i = 0; i < totalUniqueCards; i++)
        {
            int _randomIndex = UnityEngine.Random.Range(0, tempList.Count);
            Sprite selectedCard = tempList[_randomIndex];
            gamePuzzleSprites .Add(selectedCard);
            gamePuzzleSprites .Add(selectedCard); // add 2nd copy of the gem
            tempList.RemoveAt(_randomIndex);
        }
        Shuffle(gamePuzzleSprites );
    }

    void OnPuzzleButtonClick()
    {
        foreach (Button buttonprefab in puzzleButtons)
        {
            buttonprefab.onClick.AddListener(() => PickAPuzzle());
        }
    }

    public void PickAPuzzle()
    {
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        Debug.Log("Clic" + name);

        if (!isFirstGuess)
        {
            isFirstGuess = true;
            firstGuessIndex = int.Parse(name);
            firstGuessPuzzle = gamePuzzleSprites [firstGuessIndex].name;
            puzzleButtons[firstGuessIndex].image.sprite = gamePuzzleSprites [firstGuessIndex];
            _SoundAudioSource.Play();

        }
        else if (!isSecondGuess)
        {
            _SoundAudioSource.Play();
            secondGuessIndex = int.Parse(name);
            if (secondGuessIndex == firstGuessIndex)
            {

                return; // If it is the same, exit the method early
            }
            isSecondGuess = true;
            seconGuessPuzzle = gamePuzzleSprites [secondGuessIndex].name;
            puzzleButtons[secondGuessIndex].image.sprite = gamePuzzleSprites [secondGuessIndex];
            totalGuesses++;
            
            StartCoroutine(CheckPuzzleMatch());
        }
    }
    IEnumerator CheckPuzzleMatch()
    {
        yield return new WaitForSeconds(1f);
        if (firstGuessPuzzle == seconGuessPuzzle)
        {
            playerScore += 25;
            scoreDisplay.text = (playerScore).ToString();
            //yield return new WaitForSeconds(0.5f);

            //puzzleButtons[firstGuessIndex].interactable = false;
            //puzzleButtons[secondGuessIndex].interactable = false;
            puzzleButtons[firstGuessIndex].GetComponent<Button>().enabled = false;
            puzzleButtons[secondGuessIndex].GetComponent<Button>().enabled = false;

            _WinAudioSource.Play();
            CheckPuzzleMatch();
            CheckGameCompletion();
        }
        else
        { 
            playerScore -= LevelPanelScript.currentLevel+1;
            if (playerScore < 0) playerScore = 0;
            scoreDisplay.text = (playerScore).ToString();
            //yield return new WaitForSeconds(0.5f);
            puzzleButtons[firstGuessIndex].image.sprite = bgImage;
            puzzleButtons[secondGuessIndex].image.sprite = bgImage;
        }
        //yield return new WaitForSeconds(0.5f);
        isFirstGuess = isSecondGuess = false;
    }

    void CheckGameCompletion()
    {
        correctGuesses++;
        if (correctGuesses == requiredGuesses)
        {
            if (LevelPanelScript.currentLevel == LevelPanelScript.UnlockedLevel)
            {
                LevelPanelScript.UnlockedLevel++;
                PlayerPrefs.SetInt("UnlockedLevels", LevelPanelScript.UnlockedLevel);
                Debug.Log($"unlocked lvls = {LevelPanelScript.UnlockedLevel}");
            }
            //levelComplete.text = (LevelPanelScript.currentLevel + 1).ToString();
            winPanelScoreDisplay.text = (playerScore).ToString();
            WinPanel.SetActive(true);
            puzzleContainer.SetActive(false);

        }
    }

    void Shuffle(List<Sprite> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Sprite temp = list[i];
            int randomIndex = Random.Range(0, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    void UpdateTimerDisplay()
    {
        int minutes = secondsElapsed / 60;
        int displaySeconds = secondsElapsed % 60;
        timerDisplay.text = string.Format("{0:00}:{1:00}", minutes, displaySeconds);
        winPanelTimerDisplay.text = string.Format("{0:00}:{1:00}", minutes, displaySeconds);
    }


    public void NextLevelButtonClicked()
    {
        WinPanel.SetActive(false);
        LevelPanelScript.currentLevel++;
        SceneManager.LoadScene("GameScene");
    }

    public void WinPanelMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void  BackButtonClicked() 
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SettingsButtonClicked()
    {
        _SoundAudioSource.Play();
        puzzleContainer.SetActive(false);
        settingsContainer.SetActive(true);
        isPause=true;
    }

    public void ChangeMusic()
    {
        _MusicAudioSource.volume = volumeSlider.value;
        Debug.Log($"Set value of volume : {volumeSlider.value}");
        PlayerPrefs.SetFloat("musicVlm", volumeSlider.value);
        Debug.Log($"new value of musicVlm : {PlayerPrefs.GetFloat("musicVlm", 1)}");
    }
    public void changeSound()
    {
        _SoundAudioSource.volume = SoundSlider.value;
        _WinAudioSource.volume = SoundSlider.value;
        PlayerPrefs.SetFloat("soundVlm", SoundSlider.value);
    }
    public void SettingsBackButtonClicked()
    {
        _SoundAudioSource.Play();
        puzzleContainer.SetActive(true );
        settingsContainer.SetActive(false);
        isPause=false;
    }


}


