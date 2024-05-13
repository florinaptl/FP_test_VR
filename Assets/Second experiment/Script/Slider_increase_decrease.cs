using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slider_increase_decrease : MonoBehaviour
{
    public Slider mySlider;
    // Start is called before the first frame update

    public int step;
    void Start()
    {
        mySlider= GetComponent<Slider>();
        step = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseSlider()
    {
        
            if (mySlider.value + step <= mySlider.maxValue)
            {
                mySlider.value += step;
                Debug.Log("button press for adjusting slider, new value is" + mySlider.value);
            }
       
    }


    public void DecreaseSlider()
    {

        if (mySlider.value - step >= mySlider.minValue)
        {
            mySlider.value -= step;
            Debug.Log("button press for adjusting slider, new value is" + mySlider.value);
        }

    }
}
