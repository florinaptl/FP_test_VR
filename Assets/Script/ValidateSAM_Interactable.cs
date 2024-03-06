using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ValidateSAM_Interactable : XRBaseInteractable
{
    public ValidateSAM myValidateSAM;

    // Start is called before the first frame update
    void Start()
    {
        myValidateSAM = GetComponent<ValidateSAM>();

    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("test");
        // Debug.Log(this.interactorsHovering.Count.ToString());
    }



    override protected void OnHoverEntered(HoverEnterEventArgs args)
    {
        Debug.Log("hover enter test");


    }

    override protected void OnActivated(ActivateEventArgs args)
    {
        Debug.Log("validation clicked");
        //write the values to file
        myValidateSAM.myEmotion[0] = myValidateSAM.pleasureValue = 0;
        myValidateSAM.myEmotion[1] = myValidateSAM.arousalValue = 0;
        myValidateSAM.myEmotion[2] = myValidateSAM.dominanceValue = 0;
    }


}

