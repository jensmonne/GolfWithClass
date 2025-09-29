using TMPro;
using UnityEngine;

public class CheckMaxNumber : MonoBehaviour
{
    [SerializeField] private int maxNumber;
    private TMP_InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onValueChanged.AddListener(OnValueChanged);
    }
    
    private void OnValueChanged(string text)
    {
        if (text.Contains("-")) inputField.text = text.Replace("-", "");
        int.TryParse(text, out int result);
        if (result > maxNumber)
        {
            inputField.text = maxNumber.ToString();
        }
    }
}
