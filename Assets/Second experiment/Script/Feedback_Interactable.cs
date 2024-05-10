using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Feedback_Interactable : XRBaseInteractable
{
    public Feedback_SAM_button myFeedback;

    // Start is called before the first frame update
    void Start()
    {
        myFeedback = GetComponent<Feedback_SAM_button>();
    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log("test");
       // Debug.Log(this.interactorsHovering.Count.ToString());
    }

    

    override protected void  OnHoverEntered(HoverEnterEventArgs args)
    {
        Debug.Log("hover enter test");

       
    }

    override protected void OnActivated(ActivateEventArgs args)
    {
        Debug.Log("for emotion " + myFeedback.myEmotion + " value " + myFeedback.myValue + " was clicked;");
        if (myFeedback.validateSAM != null)
        {
            myFeedback.validateSAM.myEmotion[myFeedback.myEmotion] = myFeedback.myValue;
        }
    }


}
