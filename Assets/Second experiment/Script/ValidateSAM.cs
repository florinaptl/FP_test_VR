using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

using System;
using System.Globalization;

public class ValidateSAM : MonoBehaviour
{
    [SerializeField]
    public GameObject _P_1;

    [SerializeField, Range(0, 5)]
    public int pleasureValue;
    [SerializeField, Range(0, 5)]
    public int arousalValue;
    [SerializeField, Range(0, 5)]
    public int dominanceValue;

    [SerializeField]
    public Material validMaterial;

    [SerializeField]
    public Material invalidMaterial;

    public int[] myEmotion;

    Renderer renderValidate;

    public bool wasValidated;
    public SeveralSpots mySeveralSpots;
    public Facade_features_general_to_UnityGeometry myGeneralFeatures;

    public string participantName;

    // Start is called before the first frame update
    void Start()
    {
        renderValidate = GetComponent<MeshRenderer>();
        renderValidate.material = invalidMaterial;

        myEmotion = new int[3];
        myEmotion[0] = pleasureValue = 0;
        myEmotion[1] = arousalValue = 0;
        myEmotion[2] = dominanceValue = 0;

        wasValidated = false;
    }

    // Update is called once per frame
    void Update()
    {
        pleasureValue = myEmotion[0];
        arousalValue = myEmotion[1];
        dominanceValue = myEmotion[2];

        if ((pleasureValue == 0) || (arousalValue == 0) || (dominanceValue == 0))
        {
            renderValidate.material = invalidMaterial;
        }
        else renderValidate.material = validMaterial;
    }

    void OnMouseDown()
    {
        Debug.Log("validation clicked");
        //write the values to file
        myEmotion[0] = pleasureValue = 0;
        myEmotion[1] = arousalValue = 0;
        myEmotion[2] = dominanceValue = 0;
    }

    public void Validate()
    {
        if (this.myEmotion[0] != 0 &&
           this.myEmotion[1] != 0 &&
           this.myEmotion[2] != 0)
        {
            //wrtie to file
            WriteToCsvFile();
            Debug.Log("validation clicked");
            wasValidated = true;



            this.myEmotion[0] = this.pleasureValue = 0;
            this.myEmotion[1] = this.arousalValue = 0;
            this.myEmotion[2] = this.dominanceValue = 0;


        }
        else Debug.Log("validation not possible");
    }

    public void WriteToCsvFile()
    {
        string separator = ",";
        StringBuilder output = new StringBuilder();

        //modify path according to computer and file location
        string file = @"C:\Users\flori\Desktop\OutputOf_" + participantName + ".csv";
        if (new FileInfo(file).Exists == false)
        {
            string[] headings = { "pleasure",
                                  "arousal",
                                  "dominance",
                                  "speed",
                                  "nrCircles",
                                  "noOfSpots",
                                  "spotsPosition",
                                  "spotRadius",
                                  "nrOfCurrentSequence",
                                  "nameOfCurrentSequence",
                                  "myTime",
                                  "myFrameNumber",
                                  "actualTime",
                                  "actualFrameNumber",
                                  "_procentageOpaqueOfFacade_",
                                  "_compactnessOfElement_",
                                  "_compactnessOfFacade_",
                                  "_boundingRadiusOfElementProc_",
                                  "_centerLineRange_",
                                  "_realSpeed_"};
            output.AppendLine(string.Join(separator, headings));
        }

        string[] newLine = { myEmotion[0].ToString(),
                             myEmotion[1].ToString(),
                             myEmotion[2].ToString(),
                             mySeveralSpots.speed.ToString(CultureInfo.InvariantCulture),
                             mySeveralSpots.nrCircles.ToString(),
                             mySeveralSpots.nrOfSpots.ToString(),
                             mySeveralSpots.spotsPosition.ToString(),
                             mySeveralSpots.spotRadius.ToString(CultureInfo.InvariantCulture),
                             mySeveralSpots.nrOfCurrentSequence.ToString(),
                             mySeveralSpots.myTextArea,
                             mySeveralSpots.myTime.ToString(CultureInfo.InvariantCulture),
                             mySeveralSpots.myFrameNumber.ToString(),
                             mySeveralSpots.myTotalTime.ToString(CultureInfo.InvariantCulture),
                             mySeveralSpots.frameNumber.ToString(),
                             myGeneralFeatures._procentageOpaqueOfFacade_.ToString(),
                             myGeneralFeatures._compactnessOfElement_.ToString(),
                             myGeneralFeatures._compactnessOfFacade_.ToString(),
                             myGeneralFeatures._boundingRadiusOfElementProc_.ToString(),
                             myGeneralFeatures._centerLineRange_.ToString(),
                             myGeneralFeatures._realSpeed_.ToString()};//put all general features

        //output.AppendLine("test");
        output.AppendLine(string.Join(separator, newLine));
        File.AppendAllText(file, output.ToString());
    }
}
