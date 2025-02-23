using UnityEngine;
using System.Text.RegularExpressions;
using TMPro;

public class LetterOnlyInput : MonoBehaviour
{
    public TMP_InputField inputField;

    void Start()
    {
        inputField.onValueChanged.AddListener(FilterInput);
    }

    void FilterInput(string input)
    {
        string filteredText = Regex.Replace(input, "[^a-zA-Z0-9]", "");

        if (input != filteredText)
        {
            inputField.text = filteredText;
        }
    }
}
