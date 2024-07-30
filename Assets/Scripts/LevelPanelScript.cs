using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelPanelScript : MonoBehaviour
{

    public List<Button> levelButtons;
    public static int currentLevel;
    public static int UnlockedLevel;


    [SerializeField]
    private Transform puzzleFieldContainer;

    [SerializeField]
    private GameObject buttonPrefab;



    [SerializeField]
    private Sprite[] numberSprites;

    //private Text lvlNum;

    public void OnLevelButtonClick(int levelNum)
    {
        Debug.Log($"Level {levelNum} clicked");
        currentLevel = levelNum;
        SceneManager.LoadScene("GameScene");
    }

    // Update is called once per frame
    void Start()
    {
        initializeLevelButtons();
    }
    public void initializeLevelButtons()
    {
        UnlockedLevel = PlayerPrefs.GetInt("UnlockedLevels", 0);
        for (int i = 0; i < levelButtons.Count; i++)
        {
            if (i <= UnlockedLevel)
            {
                levelButtons[i].interactable = true;
            }
            else
            {
                levelButtons[i].interactable = false;
            }
        }
    }
    private void Awake()
    {
        for (int i = 0; i < 12; i++)
        {
            GameObject button = Instantiate(buttonPrefab);
            button.name = "" + i;
            button.transform.SetParent(puzzleFieldContainer, false);

            Image lvlImage = button.transform.GetChild(0).GetComponent<Image>();

            if (lvlImage != null && i < numberSprites.Length)
            {
                lvlImage.sprite = numberSprites[i];
            }
            Button btnComponent = button.GetComponent<Button>();
            int levelNum = i;
            btnComponent.onClick.AddListener(() => OnLevelButtonClick(levelNum));
            levelButtons.Add(btnComponent);
        }
        initializeLevelButtons();
    }

    
}
