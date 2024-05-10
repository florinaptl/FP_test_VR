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

        //write the values to file
        myValidateSAM.Validate();


    }


}

