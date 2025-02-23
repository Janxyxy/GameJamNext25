using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QueenRooom : Room
{
    [Header("UI")]
    [SerializeField] private Button SubmitButton;

    void Start()
    {
        SubmitButton.onClick.AddListener(HaveQuestResources);
        StartCoroutine(GenerateQuests());
    }

    private void HaveQuestResources()
    {
        
    }

    private IEnumerator GenerateQuests()
    {
        while (true)
        {
            yield return new WaitForSeconds(20);
            GenerateNewQuest();

        }
    }

    private void continueBreeding()
    {
        throw new NotImplementedException();
    }
 

    private void GenerateNewQuest()
    {
        
    }

    private void checkMaterials()
    {
        throw new NotImplementedException();
    }
}
