using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private static TMP_Text _textMoney;
    private static TMP_Text _textDistance;
    private static TMP_Text _textWatch;
    
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

    private static float _angleWatch = 0f;
    public static float AngleWatch
    {
        get { return _angleWatch; }
        set
        {
            _angleWatch = (float)System.Math.Ceiling(value);
            UpdateTextWatch();
        }
    }

    private Canvas _menu;

    [System.Obsolete]
    void Start()
    {
        _menu = GameObject.Find("Settings").GetComponentInChildren<Canvas>();
        _textMoney = GameObject.Find("Money Text").GetComponent<TMP_Text>();
        _textDistance = GameObject.Find("Distance Text").GetComponent<TMP_Text>();
        _textWatch = GameObject.Find("Watch Text").GetComponent<TMP_Text>();

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

    private static void UpdateTextDistance()
    {
        if (_textDistance != null)
        {
            _textDistance.text = $"DISTANCE: {Distance}";
        }
    }

    private static void UpdateTextWatch()
    {
        if (_textWatch != null)
        {
            if (_angleWatch < 0)
            {
               
                _textWatch.text = $"WATCH:    {_angleWatch} >>";
            }
            else if (_angleWatch > 0)
            {
                _textWatch.text = $"WATCH: << {_angleWatch}";
            }
            else
            {
                _textWatch.text = $"WATCH:    {_angleWatch}";
            }
        }
    }
}
