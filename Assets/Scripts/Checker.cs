using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checker : MonoBehaviour
{
    public Vector3 slideTarget;
    private Vector3 correct;
    private Image _color;
    public int num;
    public bool isCorrect;

    // Start is called before the first frame update
    void Awake()
    {
        slideTarget = transform.position;
        correct = slideTarget;
        _color = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(a: transform.position, b: slideTarget, t: 0.05f);
        if (slideTarget == correct)
        {
            _color.color = Color.green;
            isCorrect = true;
        }
        else
        {
            _color.color = Color.white;
            isCorrect = false;
        }
    }
}
