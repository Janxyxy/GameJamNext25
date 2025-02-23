using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QueenRooom : Room
{

    [Header("UI")]
    
    [SerializeField] private Button SubmitButton;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SubmitButton.onClick.AddListener(HaveQuestResources);
        StartCoroutine(GenerateQuests());
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
