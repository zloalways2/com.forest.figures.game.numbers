using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class Game : MonoBehaviour
{
    [SerializeField] private Transform voidObj = null;
    private int voidObjIndex = 15;
    private Camera _camera;
    private Timer _time;
    private bool boardIsActive;
    private bool choiseLvlIsActive;
    private int isWin = 0;
    private GameObject forSaveLvl;
    [SerializeField] private Checker[] checkers;
    [SerializeField] private GameObject imgWin;
    [SerializeField] private TextMeshProUGUI timeWin;
    [SerializeField] private GameObject checkerBoard;
    [SerializeField] private GameObject set;
    [SerializeField] private GameObject choiseLvl;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject backBtn;
    [SerializeField] private GameObject timer;

    [SerializeField] private TextMeshProUGUI tmpLvlComplete;
    [SerializeField] private TextMeshProUGUI tmpLvl;

    [SerializeField] private Slider slide_1, slide_2;
    [SerializeField] private AudioMixerGroup mixer;
    [SerializeField] private AudioSource aEffsects;
    float vol;


    //�������� ����������

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _time = timer.GetComponent<Timer>();
        menu.SetActive(true);
        LoadMusic();
        LoadEffect();
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    public void onClick(Transform btn) //����������� ����� �� ����
    {
        CheckWin();
        if(Vector2.Distance(a: voidObj.position, b: btn.position) < 1)
        {
            Vector3 lastVoidObj = voidObj.position;
            Checker thisTarget = btn.GetComponent<Checker>();
            voidObj.position = thisTarget.slideTarget;
            thisTarget.slideTarget = lastVoidObj;

            int checkerInd = CheckFilling(thisTarget);
            checkers[voidObjIndex] = checkers[checkerInd];
            checkers[checkerInd] = null;
            voidObjIndex = checkerInd;
        }
        aEffsects.Play();
    }

    public void CheckWin()
    {
       int correctCheckers = 0;
        foreach (var a in checkers)
        {
            if (a != null)
            {
                if (a.isCorrect)
                {
                    correctCheckers++;
                }
            }
        }

        if (correctCheckers == checkers.Length-1)
        {
            imgWin.SetActive(true);
            checkerBoard.SetActive(false);
            set.SetActive(false);
            isWin = 1;
            timeWin.text = _time.timeTxt.text;
            tmpLvlComplete.text = "Level " + tmpLvl + "\nComplite";
            PlayerPrefs.SetInt("isWin" + forSaveLvl.name, isWin);
            PlayerPrefs.SetString("timeWin" + forSaveLvl.name, timeWin.text);
        }
    }

    public void RandomFilling()
    {
        if (voidObjIndex != 15)
        {
            var temp = checkers[15].slideTarget;
            checkers[15].slideTarget = voidObj.position;
            voidObj.position = temp;
            checkers[voidObjIndex] = checkers[15];
            checkers[15] = null;
            voidObjIndex = 15;
        }

        int inversion;
        do
        {
            for (int i = 0; i <= 14; i++)
            {
                var lastPos = checkers[i].slideTarget;

                int rnd = Random.Range(0, 14);
                checkers[i].slideTarget = checkers[rnd].slideTarget;
                checkers[rnd].slideTarget = lastPos;

                var checker = checkers[i];
                checkers[i] = checkers[rnd];
                checkers[rnd] = checker;
            }
            inversion = GetInversions();
        } while (inversion % 2 != 0);
    }

    public int CheckFilling(Checker ch)
    {
        for (int i = 0; i < checkers.Length; i++)
        {
            if (checkers[i] != null)
            {
                if (checkers[i] == ch)
                    return i;
            }
        }
        return -1;
    }

    int GetInversions()
    {
        int inversionsSum = 0;
        for (int i = 0; i < checkers.Length; i++)
        {
            int thisCheckerInvertion = 0;
            for (int j = i; j < checkers.Length; j++)
            {
                if (checkers[j] != null)
                {
                    if (checkers[i].num > checkers[j].num)
                    {
                        thisCheckerInvertion++;
                    }
                }
            }
            inversionsSum += thisCheckerInvertion;
        }
        return inversionsSum;
    }

    public void clickBackBtn()
    {
        aEffsects.Play();
        if (choiseLvl.active == true)
        {
            choiseLvl.SetActive(false);
            choiseLvlIsActive = false;
            backBtn.SetActive(false);
            menu.SetActive(true);
            return;
        }
        
        if (options.active == true)
        {
            if (boardIsActive)
            {
                checkerBoard.SetActive(true);
                set.SetActive(true);
            }
            else if (choiseLvlIsActive)
            {
                choiseLvl.SetActive(true);
            }
            else
            {
                menu.SetActive(true);
                backBtn.SetActive(false);
            }
            options.SetActive(false);
            return;
        }

        if (imgWin.active == true)
        {
            imgWin.SetActive(false);
            choiseLvl.SetActive(true);
            choiseLvlIsActive = true;
            return;
        }

        if (checkerBoard.active == true)
        {
            choiseLvl.SetActive(true);
            choiseLvlIsActive = true;
            checkerBoard.SetActive(false);
            boardIsActive = false;
            set.SetActive(false);
            _time.startTimer = false;
            return;
        }

    }

    public void clickStart()
    {
        aEffsects.Play();
        menu.SetActive(false);
        backBtn.SetActive(true);
        choiseLvl.SetActive(true);
        choiseLvlIsActive = true;
    }

    public void clickOptions()
    {
        aEffsects.Play();
        backBtn.SetActive(true);
        if (boardIsActive)
        {
            checkerBoard.SetActive(false);
            set.SetActive(false);
        }
        else if (choiseLvlIsActive)
        {
            choiseLvl.SetActive(false);
        }
        else
            menu.SetActive(false);
        options.SetActive(true);
    }

    public void clickExit()
    {
        aEffsects.Play();
        Application.Quit();
    }

    public void clickBtnLvl(GameObject btn)
    {
        if (PlayerPrefs.HasKey("isWin" + btn.name))
            isWin = PlayerPrefs.GetInt("isWin" + btn.name);
        else
            isWin = 0;
        forSaveLvl = btn;
        Debug.Log(btn.name);
        string[] temp = btn.name.Split(' ');
        tmpLvl.text = temp[1];
        choiseLvl.SetActive(false);
        choiseLvlIsActive = false;
        aEffsects.Play();
        if (isWin == 1)
        {
            imgWin.SetActive(true);

            if (PlayerPrefs.HasKey("timeWin" + btn.name))
                timeWin.text = PlayerPrefs.GetString("timeWin" + btn.name);
            else
                timeWin.text = "00:00";
            tmpLvlComplete.text = "Level " + tmpLvl.text + "\nComplite";
            set.SetActive(false);
        }
        else if (isWin == 0)
        {
            checkerBoard.SetActive(true);
            set.SetActive(true);
            boardIsActive = true;
            _time.sec = 0;
            _time.min = 0;
            _time.startTimer = true;
            RandomFilling();
        }

    }

    public void clickBtnNextLvl()
    {
        choiseLvl.SetActive(true);
        choiseLvlIsActive = true;
        if (tmpLvl.text != "12")
        {
            imgWin.SetActive(false);
            //string[] temp = forSaveLvl.name.Split(' ');
            int lvlIndex;
            int.TryParse(tmpLvl.text, out lvlIndex);
            lvlIndex++;
            clickBtnLvl(GameObject.Find("LevelBtn " + lvlIndex));
        }
        else
        {
            imgWin.SetActive(false);
            choiseLvl.SetActive(true);
            choiseLvlIsActive = true;
        }

    }

    void LoadMusic()
    {
        if (PlayerPrefs.HasKey("music"))
        {
            slide_1.value = PlayerPrefs.GetFloat("music");
            Options.Value(ref vol, slide_1.value);
            mixer.audioMixer.SetFloat("Music", vol);
        }
        else
        {
            slide_1.value = 10;
            Options.Value(ref vol, slide_1.value);
            mixer.audioMixer.SetFloat("Music", vol);
        }
    }

    void LoadEffect()
    {
        if (PlayerPrefs.HasKey("platform"))
        {
            slide_2.value = PlayerPrefs.GetFloat("platform");
            Options.Value(ref vol, slide_2.value);
            mixer.audioMixer.SetFloat("Effects", vol);
        }
        else
        {
            slide_1.value = 10;
            Options.Value(ref vol, slide_1.value);
            mixer.audioMixer.SetFloat("Effects", vol);
        }
    }
}
