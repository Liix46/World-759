using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private static TMP_Text _textMoney;
    private static TMP_Text _textDistance;
    private static TMP_Text _textWatch;
    private static TMP_Text _textButton;

    private Canvas _menu;

    private AudioSource[] _audioSources;
    private AudioSource _audioSourceBack;

    private GameObject _statistic;

    private Slider _musicSlider;

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

    

    [System.Obsolete]
    void Start()
    {
        //Canvas
        _menu = GameObject.Find("Settings").GetComponentInChildren<Canvas>();
        //TMP
        _textMoney = GameObject.Find("Money Text").GetComponent<TMP_Text>();
        _textDistance = GameObject.Find("Distance Text").GetComponent<TMP_Text>();
        _textWatch = GameObject.Find("Watch Text").GetComponent<TMP_Text>();
        _textButton = GameObject.Find("Button Text").GetComponent<TMP_Text>();
        //AUDIO
        _audioSources = GetComponents<AudioSource>();
        // Menu settings
        _statistic = GameObject.Find("Statistic");
        // Music Slider
        _musicSlider = GameObject.Find("Music Slider").GetComponent<Slider>();

        //Initialization initial values 
        Count = 0;
        Time.timeScale = 0;
        _audioSourceBack = _audioSources[0];
        _statistic.active = false;
        // Play music on start game
        _audioSourceBack.Play();
    }

    [System.Obsolete]
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale =
                Time.timeScale == 0
                ? 1
                : 0;
            Debug.Log(Time.timeScale);

            if (Time.timeScale == 0)
            {
                _menu.gameObject.active = true;
                _statistic.active = false;
                _textButton.SetText("CONTINUE");
            }
            else
            {
                _menu.gameObject.active = false;
                _statistic.active = true;
            }
            
        }
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

    [System.Obsolete]
    public void ButtonClick()
    {
        _menu.gameObject.active = false;

        _statistic.active = true;

        Time.timeScale = 1;
        
    }

    public void SLiderChangeValue()
    {
        if (_musicSlider != null)
        {
            //print(_musicSlider.value);
            _audioSourceBack.volume = _musicSlider.value;
        }
        
    }
}
