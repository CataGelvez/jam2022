using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;
using System;

public class ScriptReader : MonoBehaviour
{
    [SerializeField]
    private TextAsset _InkJsonFile;
    private Story _StoryScript;

    public TMP_Text dialogueBox;
    public TMP_Text nameTag;

    public Image characterIcon;

    public Image backgroundImage;
    public Image boxImage;

    [SerializeField]
    private GridLayoutGroup choiseHolder;

    [SerializeField]
    private Button choiseBasePrefab;


    void Start()
    {
        LoadStory();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextLine();
        }
    }

    void LoadStory()
    {
        _StoryScript = new Story(_InkJsonFile.text);

        _StoryScript.BindExternalFunction("Name", (string charName) => ChangeName(charName));

        _StoryScript.BindExternalFunction("Icon", (string charName) => ChracterIcon(charName));

        _StoryScript.BindExternalFunction("BG", (string charName) => ChracterBG(charName));

        _StoryScript.BindExternalFunction("Box", (string charName) => ChracterBox(charName));

    }
    public void DisplayNextLine()
    {
        if (_StoryScript.canContinue)
        {

            string text = _StoryScript.Continue();
            text = text?.Trim();
            dialogueBox.text = text;
        }
        else if (_StoryScript.currentChoices.Count > 0)
        {
            DisplayChoices();

        }
        else
        {
            dialogueBox.text = "Fin";
        }
    }

    private void DisplayChoices()
    {
        if (choiseHolder.GetComponentsInChildren<Button>().Length > 0) return;
        for (int i = 0; i < _StoryScript.currentChoices.Count; i++)
        {
            var choise = _StoryScript.currentChoices[i];
            var button = CreateChoiseButton(choise.text);
        }
    }

    Button CreateChoiseButton(String text)
    {
        var choiceButton = Instantiate(choiseBasePrefab);
        choiceButton.transform.SetParent(choiceButton.transform, false);
        var buttonText = choiceButton.GetComponentInChildren<Text>();
        buttonText.text = text;

        return choiceButton;

    }
    public void ChangeName(string name)
    {
        string SpeakerName = name;
        nameTag.text = SpeakerName;

    }

    public void ChracterIcon(string name)
    {
        var charIcon = Resources.Load<Sprite>("CharactersIcons/"+name);
        characterIcon.sprite = charIcon;
       
    }

    public void ChracterBox(string name)
    {
        var backgroundSprite = Resources.Load<Sprite>("DialogueBox/box-" + name);
        boxImage.sprite = backgroundSprite;

    }
   public void ChracterBG(string name)
    {
        var backgroundSprite = Resources.Load<Sprite>("Fondos/fondo-" + name);
        backgroundImage.sprite = backgroundSprite;

    }



}
