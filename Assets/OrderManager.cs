using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;
    public TextMeshProUGUI textbox;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame

    public void AddString(int order, string text)
    {
        textbox.text += $"> Passo {order} -> {text}\n\n";
    }
}
