using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Cooking : MonoBehaviour
{ 
    #region public -> GameObjects Slider 
    public Slider slider;
    public Slider successArea;
    public RectTransform sliderSize;
    #endregion

    #region Text UI

    public TextMeshProUGUI resultTextDisplay;
    public GameObject textObject;
    public Animator _animator;
    #endregion
    #region private variables -> Values 
    [Header("Adjustable")]
    [SerializeField] private float tolerance = 0.4f;
    [SerializeField] private float sliderSpeed = 5f;
    [SerializeField] public bool inRange;
   
    private float _sliderValue;
    private float targetAmount;
    #endregion
    
    void Update()
    {
        // Constantly moves the slider back and forth. 
        MoveSlider();

    }

    public void MoveSlider()
    {
        _sliderValue = Mathf.PingPong(Time.time * sliderSpeed, 1f);
        slider.value = _sliderValue;
        // Debug.Log(sliderValue);
            
        float lowerBound = Mathf.Max(0f, targetAmount - tolerance);
        float upperBound = Mathf.Min(1f, targetAmount + tolerance);

        inRange = _sliderValue >= lowerBound && _sliderValue <= upperBound;
    }
    
    //Sets different success points after a success pr fail
    public void SetSuccessArea()
    {
        targetAmount = Random.Range(0f, 1f);
        successArea.value = targetAmount;
        
       // Debug.Log(targetAmount);

        float scalingFactor = 100f;

        float currentSliderSize = sliderSize.sizeDelta.x;
        float newSliderWidth = currentSliderSize * tolerance * scalingFactor;

        if (newSliderWidth > tolerance * scalingFactor)
        {
            newSliderWidth = tolerance * scalingFactor;
        }

        if (newSliderWidth < 1f)
        {
            newSliderWidth = 1f;
        }

        sliderSize.sizeDelta = new Vector2(newSliderWidth, sliderSize.sizeDelta.y);
    }

    //shows Result text
    public IEnumerator ResultTextDisplay(string outcome)
    {
        textObject.SetActive(true);
        resultTextDisplay.text = outcome;
        yield return new WaitForSeconds(0.6f);
        textObject.SetActive(false);

    }
    
    // starts the cooking and adds and removes the items accordingly
    public void CookAction(RemoveItem removeItem)
    {
        if ( inRange)
        {
            StartCoroutine(ResultTextDisplay("Purrfection!"));
            Debug.Log("SUCCESS");
            FindObjectOfType<GameState>().Add(removeItem.state);
            FindObjectOfType<GameState>().Remove(removeItem.money);

        }
        else
        {
            StartCoroutine(ResultTextDisplay("Cat-astrophe..."));

            Debug.Log("FAIL");
            FindObjectOfType<GameState>().Remove(removeItem.money);

        }
    }
}


