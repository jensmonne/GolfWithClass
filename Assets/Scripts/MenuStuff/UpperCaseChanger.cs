using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_InputField))]
public class UpperCaseChanger : MonoBehaviour
{
    private TMP_InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(string text)
    {
        inputField.text = inputField.text.ToUpper();
    }
}
