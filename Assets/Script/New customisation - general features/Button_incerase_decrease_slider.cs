using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;

public class Button_incerase_decrease_slider : XRBaseInteractable
{
    // Start is called before the first frame update


    [SerializeField, Range(-1,1)]
    public int increaseOrDecrease;
    public float step;

    public Slider mySlider;

    public Button myButton;

    void Start()
    {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClick()
    {
        if (increaseOrDecrease == -1 || increaseOrDecrease == 1)
        {
            if (mySlider.value - step >= mySlider.minValue && mySlider.value + step <= mySlider.maxValue)
            {
                mySlider.value += increaseOrDecrease * step;
                Debug.Log("button press for adjusting slider, new value is" + mySlider.value);
            }
        }
    }

    override protected void OnActivated(ActivateEventArgs args)
    {
        if (increaseOrDecrease == -1 || increaseOrDecrease == 1)
        {
            if (mySlider.value - step >= mySlider.minValue && mySlider.value + step <= mySlider.maxValue)
            {
                mySlider.value += increaseOrDecrease * step;
                Debug.Log("button press for adjusting slider, new value is" + mySlider.value);
            }
        }

    }
}
