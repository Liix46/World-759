using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private static TMP_Text _textMoney;
    private static TMP_Text _textDistance;
    
    private static int _count = 0;
    public static int Count
    {
        get { return _count; }
        set
        {
            _count = value;
            UpdateTextMoney();
        }
    }
    private static double _distance = 0d;
    public static double Distance
    {
        get { return _distance; }
        set
        {
            _distance = System.Math.Ceiling(value);
            UpdateTextDistance();
        }
    }

    private Canvas _menu;

    [System.Obsolete]
    void Start()
    {
        _menu = GameObject.Find("Settings").GetComponentInChildren<Canvas>();
        _textMoney = GameObject.Find("Money Text").GetComponent<TMP_Text>();
        _textDistance = GameObject.Find("Distance Text").GetComponent<TMP_Text>();

        Count = 0;
        //Distance = 0d;

        _menu.gameObject.active = false;
    }

    void Update()
    {
        
    }

    static void UpdateTextMoney()
    {
        _textMoney.text = $"MONEY: {Count}";
    }

    static void UpdateTextDistance()
    {
        if (_textDistance != null)
        {
            _textDistance.text = $"DISTANCE: {Distance}";
        }
        
    }
}
