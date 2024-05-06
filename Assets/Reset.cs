using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Reset : MonoBehaviour
{
    public TMP_InputField URL, Query;
    public TextMeshProUGUI debugger, Order;
    public GameObject prefab;

    MouseEnterEvent MEE;

    private void Start()
    {
        MEE = GetComponent<MouseEnterEvent>();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Order.text = "";

            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            var sg = Instantiate(prefab,transform).GetComponent<StartGraph>();
            sg.URLField = URL;
            sg.queryField = Query;
            sg.Debugger = debugger;

            MonoBehaviour[] newarr = { MEE.OnHoverActivate[0], sg.GetComponentInChildren<MouseListener>(), sg.GetComponentInChildren<StartGraph>() };
            MEE.OnHoverActivate = newarr;
        }
    }



}
