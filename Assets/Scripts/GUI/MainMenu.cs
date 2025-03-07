using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string nameEssentialScene;
    [SerializeField] string naemNewGameStartScene;

    [SerializeField] PlayerData playerData;

    public Gender selectedGender;
    public Text genderText;
    public TMPro.TMP_InputField nameInputField;

    AsyncOperation operation;

    private void OnEnable()
    {
        SetGenderFemale();
        UpdateName();
    }

    public void ExitGame()
    {
        Debug.Log("Quitting the game!");
        Application.Quit();
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(naemNewGameStartScene, LoadSceneMode.Single);
        SceneManager.LoadScene(nameEssentialScene, LoadSceneMode.Additive);
    }

    public void SetGenderMale()
    {
        selectedGender = Gender.Male;
        playerData.playerCharacterGender = selectedGender;
        genderText.text = "Masculino";
    }

    public void SetGenderFemale() 
    {
        selectedGender = Gender.Female;
        playerData.playerCharacterGender = selectedGender;
        genderText.text = "Femenino";
    }

    public void UpdateName() 
    {
        playerData.characterName = nameInputField.text;
    }

    public void SetSavingSlot(int num) 
    {
        playerData.saveSlotId = num;

    }
}
