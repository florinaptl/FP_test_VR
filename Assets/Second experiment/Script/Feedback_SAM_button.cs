using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.Interaction.Toolkit;

public class Feedback_SAM_button : MonoBehaviour
{
    [SerializeField]
    public ValidateSAM validateSAM;

    [SerializeField, Range(1, 5)]
    public int myValue;

    //0=pleasure, 1=arousal, 2=dominance
    [SerializeField, Range(0, 2)]
    public int myEmotion;

    [SerializeField]
    public Material selectedMaterial;

    [SerializeField]
    public Material unselectedMaterial;


    Renderer render;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<MeshRenderer>();
        render.material = unselectedMaterial;

    }

    // Update is called once per frame
    void Update()
    {
        if (validateSAM != null)
        {
            if (validateSAM.myEmotion[myEmotion] == myValue)
            {
                render.material = selectedMaterial;
            }
            else render.material = unselectedMaterial;
        }


    }

    void OnMouseDown()
    {
        Debug.Log("for emotion " + myEmotion + " value " + myValue + " was clicked;");
        if (validateSAM != null)
        {
            validateSAM.myEmotion[myEmotion] = myValue;
        }
    }





}
