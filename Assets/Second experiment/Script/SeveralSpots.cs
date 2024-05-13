using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

public class SeveralSpots : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Transform pointPrefab;

    public ValidateSAM myValidateSAM;

    [SerializeField]
    Spot spotPrefab;

    //spots attributes

    // [SerializeField, Range(1, 50)]
    public int nrOfSpots;

    //[SerializeField, Range(1, 30)]
    public int nrCircles;

    //spotsPosition - 0=everywere on facade(y in range (0, 3f)), 1=buttom (y in range (0, 1f)), 2=center (y in range (1, 2f)), 3=up (y in range (2, 3f))
    [SerializeField, Range(0, 3)]
    public int spotsPosition;
    public int actualSpotPosition;
    public float speed;

    //[SerializeField, Range(0.01f, 3f)]
    public float spotRadius;
    List<Spot> spotsList;


    //time
    public float myTotalTime; //for measuring the total time during the experiment
    public float myTime; //for setting the timeline
    public int frameNumber;
    public int myFrameNumber;
    //narrative
    float sequenceDuration;
    public Sequence[] myNarrative;
    public Sequence[] myCustomNarrative;
    public int totalNrOfSequences;
    float timer;
    int ok;


    [TextAreaAttribute]
    public string myTextArea;
    public int nrOfCurrentSequence;

    //spline animate
    SplineAnimate mySplineAnimation;
    [SerializeField]
    SplineContainer randomSpline;
    [SerializeField]
    SplineContainer smallRandomSpline;

    //sliders for customisation by participant
    public Canvas myCustomizationCanvas;
    public Slider slider_nrOfSpots;
    public Slider slider_speed;
    public Slider slider_nrCircles;
    public Slider slider_spotRadius;
    public Slider slider_spotsPosition;

    public Canvas myExamplesCanvas;
    public Canvas myGeneralCustomizationCanvas;

    public float facadeBase, facadeHeight, facadeDiag, facadeArea, centerLine, centerlineRange, compactness_facade_percentage;
    [SerializeField, Range(0f, 1f)]
    public float _compactness_facade_percentage;
    [SerializeField, Range(0f, 1f)]
    public float _centerlineRange;

    GameObject centerlineGameObj;



    void Start()
    {

        Application.targetFrameRate = 50;

        //time setting
        myTime = 0f;
        myTotalTime = 0f;
        sequenceDuration = 15f;
        nrOfCurrentSequence = 0;
        myFrameNumber = 0;
        myTextArea = "start";

        //additional variables
        timer = 0f;

        //facade features at start
        speed = 0.2f;
        nrCircles = 2;
        nrOfSpots = 1;
        actualSpotPosition = spotsPosition = 0;
        spotRadius = 0.6f;

        //describe facade
        facadeBase = 6f;
        facadeHeight = 3.5f;
        facadeDiag = Mathf.Sqrt(facadeBase * facadeBase + facadeHeight * facadeHeight);
        facadeArea = facadeBase * facadeHeight;
        centerLine = facadeHeight / 2f;
        compactness_facade_percentage = 0f;
        _compactness_facade_percentage = 0f;
        centerlineRange = 0.5f;
        _centerlineRange = 0.5f;

        centerlineGameObj = new GameObject();
        centerlineGameObj.transform.SetParent(this.transform, false);
        centerlineGameObj.name = "centerlineGameObj";
        centerlineGameObj.transform.localPosition = new Vector3(0f, 0f, 0f);

        GetComponent<Transform>().position = new Vector3(0f, 0f, 0f);

        spotsList = new List<Spot>();

        MakeSpots(1, facadeBase / 2f, facadeBase / 2f, facadeHeight / 2f, facadeHeight / 2f);
        if (nrOfSpots > 1)
        {
            MakeSpots(nrOfSpots - 1, 0f, facadeBase, 0f, facadeHeight);
        }


        //spline animation for spotList as one GameObject
        mySplineAnimation = gameObject.AddComponent<SplineAnimate>();
        mySplineAnimation.Container = randomSpline;
        mySplineAnimation.Alignment = SplineAnimate.AlignmentMode.None;
        mySplineAnimation.AnimationMethod = SplineAnimate.Method.Speed;
        mySplineAnimation.MaxSpeed = 0.5f;
        mySplineAnimation.PlayOnAwake = false;
        mySplineAnimation.Pause();

        //CUSTOM CANVAS
        myCustomizationCanvas.enabled = false;
        myExamplesCanvas.enabled = false;
        myGeneralCustomizationCanvas.enabled = false;


        //MAKE NARRATIVE
        int k;
        myNarrative = new Sequence[20];
        myCustomNarrative = new Sequence[20];
        for (int i = 0; i < 20; i++)
        {
            myNarrative[i] = new Sequence();
        }

        //sequence 1
        k = 0;
        myNarrative[k].name = "initial mood";
        myNarrative[k].number = k;

        myNarrative[k].speed = 0.01f;
        myNarrative[k].nrCircles = 1;
        myNarrative[k].nrOfSpots = 1;
        myNarrative[k].spotRadius = 0.6f;
        myNarrative[k].spotsPosition = 0;

        ////sequence
        //k++;
        //myNarrative[k].name = "random 1";
        //myNarrative[k].number = k;

        //myNarrative[k].speed = 2f;
        //myNarrative[k].nrCircles = 3;
        //myNarrative[k].nrOfSpots = 1;
        //myNarrative[k].spotRadius = 0.6f;
        //myNarrative[k].spotsPosition = 0;
        //myNarrative[k].validated = true;

        ////sequence
        //k++;
        //myNarrative[k].name = "random 2";
        //myNarrative[k].number = k;

        //myNarrative[k].speed = 2f;
        //myNarrative[k].nrCircles = 3;
        //myNarrative[k].nrOfSpots = 10;
        //myNarrative[k].spotRadius = 0.4f;
        //myNarrative[k].spotsPosition = 0;

        ////sequence 3
        //k++;
        //myNarrative[k].name = "low speed few spots";
        //myNarrative[k].number = k;

        //myNarrative[k].speed = 5f;
        //myNarrative[k].nrCircles = 3;
        //myNarrative[k].nrOfSpots = 8;
        //myNarrative[k].spotRadius = 0.6f;
        //myNarrative[k].spotsPosition = 0;

        //sequence 1
        k++;
        myNarrative[k].name = "medium speed high nr of spots ";
        myNarrative[k].number = k;

        myNarrative[k].speed = 0.2f;
        myNarrative[k].nrCircles = 3;
        myNarrative[k].nrOfSpots = 50;
        myNarrative[k].spotRadius = 0.6f;
        myNarrative[k].spotsPosition = 0;

        ////sequence
        //k++;
        //myNarrative[k].name = "explosion with big radius";
        //myNarrative[k].number = k;

        //myNarrative[k].speed = 0.2f;
        //myNarrative[k].nrCircles = 3;
        //myNarrative[k].nrOfSpots = 5;
        //myNarrative[k].spotRadius = 0.6f;
        //myNarrative[k].spotsPosition = 0;

        ////sequence
        //k++;
        //myNarrative[k].name = "medium speed medium number of spots";
        //myNarrative[k].number = k;

        //myNarrative[k].speed = 0.5f;
        //myNarrative[k].nrCircles = 3;
        //myNarrative[k].nrOfSpots = 15;
        //myNarrative[k].spotRadius = 0.6f;
        //myNarrative[k].spotsPosition = 0;

        ////sequence
        //k++;
        //myNarrative[k].name = "explosion with small radius";
        //myNarrative[k].number = k;

        //myNarrative[k].speed = 0.5f;
        //myNarrative[k].nrCircles = 5;
        //myNarrative[k].nrOfSpots = 10;
        //myNarrative[k].spotRadius = 0.6f;
        //myNarrative[k].spotsPosition = 0;

        ////sequence
        //k++;
        //myNarrative[k].name = "low speed high number of spots";
        //myNarrative[k].number = k;

        //myNarrative[k].speed = 0.1f;
        //myNarrative[k].nrCircles = 3;
        //myNarrative[k].nrOfSpots = 50;
        //myNarrative[k].spotRadius = 0.6f;
        //myNarrative[k].spotsPosition = 0;

        //sequence 2
        k++;
        myNarrative[k].name = "density of many compact spots";
        myNarrative[k].number = k;

        myNarrative[k].speed = 0.2f;
        myNarrative[k].nrCircles = 7;
        myNarrative[k].nrOfSpots = 30;
        myNarrative[k].spotRadius = 0.3f;
        myNarrative[k].spotsPosition = 0;

        //sequence 3
        k++;
        myNarrative[k].name = "lower area position";
        myNarrative[k].number = k;

        myNarrative[k].speed = 0.2f;
        myNarrative[k].nrCircles = 3;
        myNarrative[k].nrOfSpots = 30;
        myNarrative[k].spotRadius = 0.6f;
        myNarrative[k].spotsPosition = 1;

        //sequence 4
        k++;
        myNarrative[k].name = "density of few loose spots";
        myNarrative[k].number = k;

        myNarrative[k].speed = 0.2f;
        myNarrative[k].nrCircles = 5;
        myNarrative[k].nrOfSpots = 15;
        myNarrative[k].spotRadius = 1f;
        myNarrative[k].spotsPosition = 0;

        //sequence 5
        k++;
        myNarrative[k].name = "top area position";
        myNarrative[k].number = k;

        myNarrative[k].speed = 0.2f;
        myNarrative[k].nrCircles = 3;
        myNarrative[k].nrOfSpots = 30;
        myNarrative[k].spotRadius = 0.6f;
        myNarrative[k].spotsPosition = 3;

        ////sequence 13
        //k++;
        //myNarrative[k].name = "low speed few spots";
        //myNarrative[k].number = k;

        //myNarrative[k].speed = 0.1f;
        //myNarrative[k].nrCircles = 5;
        //myNarrative[k].nrOfSpots = 5;
        //myNarrative[k].spotRadius = 0.8f;
        //myNarrative[k].spotsPosition = 0;

        ////sequence 14
        //k++;
        //myNarrative[k].name = "big spot radius many circles";
        //myNarrative[k].number = k;

        //myNarrative[k].speed = 0.1f;
        //myNarrative[k].nrCircles = 20;
        //myNarrative[k].nrOfSpots = 5;
        //myNarrative[k].spotRadius = 1.2f;
        //myNarrative[k].spotsPosition = 0;

        //sequence 6
        k++;
        myNarrative[k].name = "big spot radius few circles";
        myNarrative[k].number = k;

        myNarrative[k].speed = 0.1f;
        myNarrative[k].nrCircles = 5;
        myNarrative[k].nrOfSpots = 5;
        myNarrative[k].spotRadius = 1.2f;
        myNarrative[k].spotsPosition = 0;

        //sequence 16
        k++;
        myNarrative[k].name = "acceleration low";
        myNarrative[k].number = k;

        myNarrative[k].speed = 0.0288f;
        myNarrative[k].nrCircles = 5;
        myNarrative[k].nrOfSpots = 5;
        myNarrative[k].spotRadius = 0.6f;
        myNarrative[k].spotsPosition = 0;

        //sequence 17
        k++;
        myNarrative[k].name = "acceleration fast";
        myNarrative[k].number = k;

        myNarrative[k].speed = 0.0288f;
        myNarrative[k].nrCircles = 5;
        myNarrative[k].nrOfSpots = 5;
        myNarrative[k].spotRadius = 0.6f;
        myNarrative[k].spotsPosition = 0;

        ////sequence 18
        //k++;
        //myNarrative[k].name = "custom";
        //myNarrative[k].number = k;

        //myNarrative[k].speed = 0.1f;
        //myNarrative[k].nrCircles = 5;
        //myNarrative[k].nrOfSpots = 5;
        //myNarrative[k].spotRadius = 1.2f;
        //myNarrative[k].spotsPosition = 0;

        //sequence 19
        k++;
        myNarrative[k].name = "general custom1";
        myNarrative[k].number = k;

        myNarrative[k].speed = 0.1f;
        myNarrative[k].nrCircles = 5;
        myNarrative[k].nrOfSpots = 5;
        myNarrative[k].spotRadius = 1.2f;
        myNarrative[k].spotsPosition = 0;

        //sequence 20
        k++;
        myNarrative[k].name = "general custom2";
        myNarrative[k].number = k;

        myNarrative[k].speed = 0.1f;
        myNarrative[k].nrCircles = 5;
        myNarrative[k].nrOfSpots = 5;
        myNarrative[k].spotRadius = 1.2f;
        myNarrative[k].spotsPosition = 0;

        //sequence 21
        k++;
        myNarrative[k].name = "general custom3";
        myNarrative[k].number = k;

        myNarrative[k].speed = 0.1f;
        myNarrative[k].nrCircles = 5;
        myNarrative[k].nrOfSpots = 5;
        myNarrative[k].spotRadius = 1.2f;
        myNarrative[k].spotsPosition = 0;

        //sequence 22
        k++;
        myNarrative[k].name = "final mood";
        myNarrative[k].number = k;

        myNarrative[k].speed = 0.01f;
        myNarrative[k].nrCircles = 1;
        myNarrative[k].nrOfSpots = 1;
        myNarrative[k].spotRadius = 0.6f;
        myNarrative[k].spotsPosition = 0;

        //SetSpotCollection(ref spotsList, myNarrative[k].speed, myNarrative[k].nrCircles, myNarrative[k].nrOfSpots, myNarrative[k].spotsPosition);


        //MAKE CUSTOM NARRATIVE - with initial mood, 3 randomly choosen sequences from collection and 3 customisable
        totalNrOfSequences = k + 1; //including the initial mood at i=0
        myCustomNarrative[0] = myNarrative[0]; //make first sequence the inital mood

        //last element has index totalNrOfSequences-1 and the sup limit is excluded so add 1, also -4 (3 custom and final mood)
        myCustomNarrative[1] = myNarrative[Random.Range(1, totalNrOfSequences - 1 - 4 + 1)];
        //generate sequences for sequence 2 and 3, different
        for (int i = 2; i <= 3; i++)
        {
            bool randomTest = false;
            int iterations = 0;
            int rnd = 0;
            while (randomTest == false && iterations < 30)
            {
                iterations++;
                rnd = Random.Range(1, totalNrOfSequences - 1 - 4 + 1);
                randomTest = true;
                for (int j = 1; j < i; j++)
                {
                    if (myNarrative[rnd].name == myNarrative[j].name)
                        randomTest = false;
                }
            }
            myCustomNarrative[i] = myNarrative[rnd];
            if (iterations == 30)
            {
                Debug.Log("iteration " + iterations + "reached maximum");
            }
            else Debug.Log("iteration " + iterations);
        }

        for (int i = 0; i < totalNrOfSequences; i++)
        {
            if (myNarrative[i].name == "general custom1")
                myCustomNarrative[4] = myNarrative[i];
            else if (myNarrative[i].name == "general custom2")
                myCustomNarrative[5] = myNarrative[i];
            else if (myNarrative[i].name == "general custom3")
                myCustomNarrative[6] = myNarrative[i];
        }

        //make last sequecne the final mood
        myCustomNarrative[7] = myNarrative[totalNrOfSequences - 1];

        //set the narrative as the custom narrative
        myNarrative = myCustomNarrative;

    }



    // Update is called once per frame
    void Update()

    {


        if (GetComponent<Transform>().position != new Vector3(0f, 0f, 0f))
        {
            GetComponent<Transform>().position = new Vector3(0f, 0f, 0f);
        }
        //myTotalTime += Time.deltaTime; //do not modify later in script
        //there are 50 frames per second as set at the beginning of start
        myTotalTime = Time.frameCount * 0.02f;
        frameNumber = Time.frameCount;

        myFrameNumber++;
        myTime = (float)myFrameNumber * 0.02f;


        // SetSpotCollection(ref List<Spot> spotsList, float _speed, int _nrCircles, int _nrOfSpots, int _spotsPosition)


        nrOfCurrentSequence = (int)(myTime / sequenceDuration);

        if (myNarrative[nrOfCurrentSequence].name == "initial mood" || myNarrative[nrOfCurrentSequence].name == "final mood")
        {
            myExamplesCanvas.enabled = true;
            centerlineGameObj.gameObject.SetActive(false);
        }
        else
        {
            myExamplesCanvas.enabled = false;
            centerlineGameObj.gameObject.SetActive(true);
        }


        //CUSTOM CANVAS
        if (myNarrative[nrOfCurrentSequence].name == "custom")
        {
            myCustomizationCanvas.enabled = true;

            if (slider_nrOfSpots != null)
            {
                int k = nrOfCurrentSequence;

                myNarrative[k].speed = slider_speed.value;
                myNarrative[k].nrCircles = (int)slider_nrCircles.value;
                myNarrative[k].nrOfSpots = (int)slider_nrOfSpots.value;
                myNarrative[k].spotRadius = slider_spotRadius.value;
                myNarrative[k].spotsPosition = (int)slider_spotsPosition.value;

                SetSpotCollection(ref spotsList, myNarrative[k].speed, myNarrative[k].nrCircles, myNarrative[k].nrOfSpots, myNarrative[k].spotsPosition, myNarrative[k].spotRadius);
            }
        }
        else myCustomizationCanvas.enabled = false;

        //CUSTOM CANVAS
        if (myNarrative[nrOfCurrentSequence].name == "general custom1")
        {
            myGeneralCustomizationCanvas.enabled = true;
            int k = nrOfCurrentSequence;

            myNarrative[k].speed = this.speed;
            myNarrative[k].nrCircles = this.nrCircles;
            myNarrative[k].nrOfSpots = this.nrOfSpots;
            myNarrative[k].spotRadius = this.spotRadius;
            myNarrative[k].spotsPosition = this.spotsPosition;

            SetSpotCollection(ref spotsList, myNarrative[k].speed, myNarrative[k].nrCircles, myNarrative[k].nrOfSpots, myNarrative[k].spotsPosition, myNarrative[k].spotRadius);

        }
        else if (myNarrative[nrOfCurrentSequence].name == "general custom2")
        {
            myGeneralCustomizationCanvas.enabled = true;
            int k = nrOfCurrentSequence;

            myNarrative[k].speed = this.speed;
            myNarrative[k].nrCircles = this.nrCircles;
            myNarrative[k].nrOfSpots = this.nrOfSpots;
            myNarrative[k].spotRadius = this.spotRadius;
            myNarrative[k].spotsPosition = this.spotsPosition;

            SetSpotCollection(ref spotsList, myNarrative[k].speed, myNarrative[k].nrCircles, myNarrative[k].nrOfSpots, myNarrative[k].spotsPosition, myNarrative[k].spotRadius);

        }
        else if (myNarrative[nrOfCurrentSequence].name == "general custom3")
        {
            myGeneralCustomizationCanvas.enabled = true;
            int k = nrOfCurrentSequence;

            myNarrative[k].speed = this.speed;
            myNarrative[k].nrCircles = this.nrCircles;
            myNarrative[k].nrOfSpots = this.nrOfSpots;
            myNarrative[k].spotRadius = this.spotRadius;
            myNarrative[k].spotsPosition = this.spotsPosition;

            SetSpotCollection(ref spotsList, myNarrative[k].speed, myNarrative[k].nrCircles, myNarrative[k].nrOfSpots, myNarrative[k].spotsPosition, myNarrative[k].spotRadius);

        }
        else
        {
            myGeneralCustomizationCanvas.enabled = false;
        }

        //additional changes to narrative that are needed during runtime
        if (myNarrative[nrOfCurrentSequence].name == "random 1")
        {
            if (myTime % 2 == 0)
            {
                myNarrative[nrOfCurrentSequence].nrOfSpots++;
            }
            if ((myTime == 3f) ||
                (myTime == 6f) ||
                (myTime == 9f))
            {
                myNarrative[nrOfCurrentSequence].nrCircles++;
            }
            if ((myTime == 12f) ||
                (myTime == 14f))
            {
                myNarrative[nrOfCurrentSequence].nrCircles++;
                myNarrative[nrOfCurrentSequence].spotRadius += 0.4f;
            }

            myNarrative[nrOfCurrentSequence].speed = (Mathf.Sin(myTime / 15f * 2f * Mathf.PI) + 2f) / 2f;

            int k = nrOfCurrentSequence;
            SetSpotCollection(ref spotsList, myNarrative[k].speed, myNarrative[k].nrCircles, myNarrative[k].nrOfSpots, myNarrative[k].spotsPosition, myNarrative[k].spotRadius);
        }

        if (myNarrative[nrOfCurrentSequence].name == "random 2")
        {

            myNarrative[nrOfCurrentSequence].speed = (Mathf.Sin(myTime / 15f * 2f * Mathf.PI) + 2f) / 2f;

            if ((myTime == 18f) ||
                (myTime == 21f) ||
                (myTime == 25f))
            {
                myNarrative[nrOfCurrentSequence].nrCircles += 3;
            }

            //if (myTime == 25f) myNarrative[nrOfCurrentSequence].nrOfSpots = 20;

            int k = nrOfCurrentSequence;
            SetSpotCollection(ref spotsList, myNarrative[k].speed, myNarrative[k].nrCircles, myNarrative[k].nrOfSpots, myNarrative[k].spotsPosition, myNarrative[k].spotRadius);
        }

        if (myNarrative[nrOfCurrentSequence].name == "explosion with big radius")
        {
            if (myTime <= nrOfCurrentSequence * sequenceDuration + 0.02f)
            {
                ok = 1;
                timer = 0.1f;
            }

            if (timer < 3)
            {
                //adjust speed of radius expansion using the timer
                timer += 0.05f;

            }
            else
            {
                timer = 0.1f;
                ok = ok * (-1);
            }

            if (ok == 1)
            {
                myNarrative[nrOfCurrentSequence].spotRadius = timer;
            }
            else if (ok == -1)
            {
                myNarrative[nrOfCurrentSequence].spotRadius = 3.2f - timer;
            }

            int k = nrOfCurrentSequence;
            SetSpotCollection(ref spotsList, myNarrative[k].speed, myNarrative[k].nrCircles, myNarrative[k].nrOfSpots, myNarrative[k].spotsPosition, myNarrative[k].spotRadius);
        }

        if (myNarrative[nrOfCurrentSequence].name == "explosion with small radius")
        {
            if (myTime <= nrOfCurrentSequence * sequenceDuration + 0.02f)
            {
                ok = 1;
                timer = 0.1f;
            }

            if (timer < 0.8f)
            {
                //adjust speed of radius expansion using the timer
                timer += 0.005f;

            }
            else
            {
                timer = 0.05f;
                ok = ok * (-1);
            }

            if (ok == 1)
            {
                myNarrative[nrOfCurrentSequence].spotRadius = timer;
            }
            else if (ok == -1)
            {
                myNarrative[nrOfCurrentSequence].spotRadius = 0.9f - timer;
            }

            int k = nrOfCurrentSequence;
            SetSpotCollection(ref spotsList, myNarrative[k].speed, myNarrative[k].nrCircles, myNarrative[k].nrOfSpots, myNarrative[k].spotsPosition, myNarrative[k].spotRadius);
        }

        //acceleration of velocity
        if (myNarrative[nrOfCurrentSequence].name == "acceleration low")
        {
            //use timer as speed
            if (timer <= 0.0288f)
            {
                ok = 1;
                //timer = 0.0288f;
            }

            if (timer < 15 / 34.67 && ok == 1)
            {
                //add acceleration per secound - meaning per frame - there are 50 frames per secound
                //accelerate with 1 cm/s, meaning adding 0.0288 at unity speed every secound, meaning adding 0.0288*0.02 every frame
                //adjust acceleration using the timer
                timer += 0.0288f * 0.02f;
            }
            else if (ok != -1 && timer > 15f / 34.67f)
            {
                ok = -1;
            }
            else if (timer > 0.0288f && ok == -1)
            {
                timer -= 0.0288f * 0.02f;
            }

            myNarrative[nrOfCurrentSequence].speed = timer;
            Debug.Log("speed is " + timer);

            int k = nrOfCurrentSequence;
            SetSpotCollection(ref spotsList, myNarrative[k].speed, myNarrative[k].nrCircles, myNarrative[k].nrOfSpots, myNarrative[k].spotsPosition, myNarrative[k].spotRadius);

        }

        //acceleration of velocity
        if (myNarrative[nrOfCurrentSequence].name == "acceleration fast")
        {
            //use timer as speed
            if (timer <= 0.0288f)
            {
                ok = 1;
                //timer = 0.0288f;
            }

            if (timer < 15 / 34.67 && ok == 1)
            {
                //add acceleration per secound - meaning per frame - there are 50 frames per secound
                //accelerate with 1 cm/s, meaning adding 0.0288 at unity speed every secound, meaning adding 0.0288*0.02 every frame
                //adjust acceleration using the timer
                timer += 0.0288f * 0.02f * 4f;
            }
            else if (ok != -1 && timer > 15f / 34.67f)
            {
                ok = -1;
            }
            else if (timer > 0.0288f && ok == -1)
            {
                timer -= 0.0288f * 0.02f * 4f;
            }

            myNarrative[nrOfCurrentSequence].speed = timer;
            Debug.Log("speed is " + timer);

            int k = nrOfCurrentSequence;
            SetSpotCollection(ref spotsList, myNarrative[k].speed, myNarrative[k].nrCircles, myNarrative[k].nrOfSpots, myNarrative[k].spotsPosition, myNarrative[k].spotRadius);

        }

        //narrative


        if (myNarrative[nrOfCurrentSequence].playedOnce == false)
        {
            int k = nrOfCurrentSequence;
            SetSpotCollection(ref spotsList, myNarrative[k].speed, myNarrative[k].nrCircles, myNarrative[k].nrOfSpots, myNarrative[k].spotsPosition, myNarrative[k].spotRadius);
            myNarrative[nrOfCurrentSequence].playedOnce = true;
            myTextArea = myNarrative[nrOfCurrentSequence].name;
        }

        //time to check validation
        if (myTime >= (nrOfCurrentSequence + 1) * sequenceDuration - 0.03f)
        {
            Debug.Log("time to check validation");
            if (myNarrative[nrOfCurrentSequence].validated == false)
            {
                //go back
                myFrameNumber = (int)sequenceDuration * 50 * nrOfCurrentSequence - 1;
            }
        }

        if (myValidateSAM.wasValidated == true)
        {
            myNarrative[nrOfCurrentSequence].validated = true;
            myValidateSAM.wasValidated = false;
            myFrameNumber = (int)sequenceDuration * 50 * (nrOfCurrentSequence + 1) - 1;
        }







        //for manual changes

        //ChangeNrCirclesOnSpot(ref spotsList, nrCircles);

        //ChangeNrOfSpots(ref spotsList, nrOfSpots);

        //ChangePositionRange(ref spotsList, ref actualSpotPosition, spotsPosition);

        //ChangeSpotRadius(ref spotsList, spotRadius);

        ChangePositionRange_byCompacntess(ref spotsList, _compactness_facade_percentage);

        ChangePositionOfCenterline(ref spotsList, _centerlineRange);

        //ChangeAnimationSpeed(ref spotsList, speed);


        //update sliders
        if (myCustomizationCanvas.enabled == true)
        {
            slider_nrOfSpots.value = this.nrOfSpots;
            slider_speed.value = this.speed;
            slider_nrCircles.value = this.nrCircles;
            slider_spotRadius.value = this.spotRadius;
            slider_spotsPosition.value = this.spotsPosition;
        }


        //if (myGeneralCustomizationCanvas.enabled == true)
        //{
        //    slider_nrOfSpots.value = this.nrOfSpots;
        //    slider_speed.value = this.speed;
        //    slider_nrCircles.value = this.nrCircles;
        //    slider_spotRadius.value = this.spotRadius;
        //    slider_spotsPosition.value = this.spotsPosition;
        //}


    }









    //METHODS

    public void MakeSpots(int nrOfSpots, float minRangeX, float maxRangeX, float minRangeY, float maxRangeY)
    {
        for (int i = 0; i < nrOfSpots; i++)
        {
            Spot spot = Instantiate(spotPrefab, centerlineGameObj.transform, false);
            spot.pointPrefab = pointPrefab;
            spot.transform.localPosition = new Vector3(Random.Range(minRangeX, maxRangeX), Random.Range(minRangeY, maxRangeY), 0f);
            spot.nrCircles = this.nrCircles;

            spotsList.Add(spot);
            spotsList[spotsList.Count - 1].nrCircles = this.nrCircles;

            //make animation for each new spot (the last spot added to list) with speed 0.2f and Pause()
            MakeAnimationSpline_onSpot(spotsList[spotsList.Count - 1], smallRandomSpline, this.speed, true);
        }
    }


    //change no of spots without changing the whole list
    public void ChangeNrOfSpots(ref List<Spot> spotsList, int _nrOfSpots)
    {
        if (_nrOfSpots != spotsList.Count)
        {
            if (_nrOfSpots < spotsList.Count)
            {
                for (int i = spotsList.Count - 1; i > _nrOfSpots - 1; i--)
                {
                    //destroy possible animation spine of the spot
                    DestroyAnimationSpline_onSpot(spotsList[i]);
                    //destroy spot and remove from list
                    Destroy(spotsList[i].gameObject);
                    spotsList.RemoveAt(i);
                }
            }
            else if (_nrOfSpots > spotsList.Count)
            {
                MakeSpots(_nrOfSpots - spotsList.Count, 0f, this.facadeBase, centerLine - facadeHeight / 2 * (1 - compactness_facade_percentage), centerLine + facadeHeight / 2 * (1 - compactness_facade_percentage));
            }

            //update spotsList attributes
            this.nrOfSpots = spotsList.Count;
        }


    }

    //change the position of the spots without changing the list and with compactness - reducing the height of the facade's rectangle
    public void ChangePositionRange_byCompacntess(ref List<Spot> spotsList, float _percentage)
    {
        if (compactness_facade_percentage != _percentage)
        {
            if (spotsList.Count != 0)
            {
                foreach (var spot in spotsList)
                {
                    spot.transform.localPosition = new Vector3(spot.mySpline.transform.localPosition.x, Random.Range(centerLine - facadeHeight / 2 * (1 - _percentage), centerLine + facadeHeight / 2 * (1 - _percentage)), 0f);
                    Debug.Log("change position range for spot");
                }
            }
            Debug.Log("change position range");

            compactness_facade_percentage = _percentage;

            //update animation spline
            foreach (var spot in spotsList)
            {
                UpdateAnimationPosition_FollowSpot(spot);
            }
        }

    }

    public void ChangePositionOfCenterline(ref List<Spot> spotsList, float _centerlineRange)
    {
        if (_centerlineRange != this.centerlineRange)
        {
            centerLine = facadeHeight / 6f * (1 - _centerlineRange) + 5f * facadeHeight / 6f * (1 - _centerlineRange);
            this.centerlineRange = _centerlineRange;

            centerlineGameObj.transform.localPosition = new Vector3(centerlineGameObj.transform.localPosition.x, centerLine - facadeHeight / 2, 0);

        }

    }


    //change the position of the spots without changing the list
    public void ChangePositionRange(ref List<Spot> spotsList, ref int actualSpotPosition, int _spotsPosition)
    {

        if (spotsList.Count != 0 && actualSpotPosition != _spotsPosition)

        {
            if (_spotsPosition == 0)
            {
                foreach (var spot in spotsList)
                {
                    spot.transform.localPosition = new Vector3(spot.transform.localPosition.x, Random.Range(0f, facadeHeight), 0f);
                }
            }
            else if (_spotsPosition == 1)
            {
                foreach (var spot in spotsList)
                {
                    spot.transform.localPosition = new Vector3(spot.transform.localPosition.x, Random.Range(0f, facadeHeight / 3f - 0.3f), 0f);
                }
            }

            else if (_spotsPosition == 2)
            {
                foreach (var spot in spotsList)
                {
                    spot.transform.localPosition = new Vector3(spot.transform.localPosition.x, Random.Range(facadeHeight / 3f, 2f * facadeHeight / 3f - 0.3f), 0f);
                }
            }
            else if (_spotsPosition == 3)
            {
                foreach (var spot in spotsList)
                {
                    spot.transform.localPosition = new Vector3(spot.transform.localPosition.x, Random.Range(2f * facadeHeight / 3f, facadeHeight), 0f);
                }
            }

            //update animation spline
            foreach (var spot in spotsList)
            {
                UpdateAnimationPosition_FollowSpot(spot);
            }

            //update spotsList attributes
            actualSpotPosition = _spotsPosition;
            this.spotsPosition = _spotsPosition;
        }

    }



    //change the nrCircles of each spot in the list
    public void ChangeNrCirclesOnSpot(ref List<Spot> spotsList, int _nrCircles)
    {
        if (spotsList.Count != 0)
        {
            if (spotsList[0].nrCircles != _nrCircles || spotsList[spotsList.Count - 1].nrCircles != _nrCircles)
            {
                foreach (var spot in spotsList)
                {
                    spot.nrCircles = _nrCircles;
                }

                //update spotsList attributes
                this.nrCircles = _nrCircles;
            }
        }
    }

    //change spot radius for all spots in the list
    public void ChangeSpotRadius(ref List<Spot> spotsList, float _spotRadius)
    {
        if (spotsList.Count != 0)
        {
            foreach (var spot in spotsList)
            {
                if (spot.spotRadius != _spotRadius)
                    spot.spotRadius = _spotRadius;
            }
            this.spotRadius = _spotRadius;
        }

    }



    //ANIMATION

    //make animation for an entire list of spots, each spot having it's own spline and SplineAnimate component
    public void MakeAnimationSpline(ref List<Spot> spotsList, SplineContainer _mySpline, float _speed, bool _play)
    {
        if (spotsList.Count != 0)
        {

            foreach (var spot in spotsList)
            {
                //all spot's splines are destroyed
                if (spot.mySpline != null)
                {
                    Destroy(spot.mySpline.gameObject);
                    spot.mySpline = null;
                    Destroy(spot.gameObject.GetComponent<SplineAnimate>());
                }

                //if _mySpline is not null, new splines are created; otherwise, nothing is created and this way the previous splines are destroyed
                if (_mySpline != null)
                {
                    SplineAnimate spotSplineAnimate = spot.gameObject.AddComponent<SplineAnimate>();

                    //add _mySpline prefab to the spot's mySpline and then modify it
                    spot.mySpline = Instantiate(_mySpline, this.transform, false);
                    spot.mySpline.transform.localPosition = spot.transform.localPosition;
                    spot.mySpline.transform.localRotation = Quaternion.Euler(Random.Range(80, 140), 90, 90);

                    //SplineAnimate settings
                    spotSplineAnimate.Container = spot.mySpline;
                    spotSplineAnimate.Alignment = SplineAnimate.AlignmentMode.None;
                    spotSplineAnimate.AnimationMethod = SplineAnimate.Method.Speed;
                    spotSplineAnimate.MaxSpeed = _speed;
                    spotSplineAnimate.PlayOnAwake = true;
                    if (_play == false) spotSplineAnimate.Pause();
                    else spotSplineAnimate.Play();
                }
            }
        }
    }

    //destroy animation for one spot
    public void DestroyAnimationSpline_onSpot(Spot _spot) //destroy both spline and SplineAnimate component
    {
        if (_spot.mySpline != null)
        {
            Destroy(_spot.mySpline.gameObject);
            _spot.mySpline = null;
            Destroy(_spot.mySplineAnimate.gameObject);
            _spot.mySplineAnimate = null;
        }

    }


    //make animation for one spot
    public void MakeAnimationSpline_onSpot(Spot _spot, SplineContainer _mySpline, float _speed, bool _play)
    {
        DestroyAnimationSpline_onSpot(_spot);

        if (_mySpline != null)
        {

            _spot.mySplineAnimate = _spot.gameObject.AddComponent<SplineAnimate>();

            //add _mySpline prefab to the spot's mySpline and then modify it
            _spot.mySpline = Instantiate(_mySpline, _spot.transform.parent, false);
            _spot.mySpline.transform.localPosition = _spot.transform.localPosition;
            _spot.mySpline.transform.localRotation = Quaternion.Euler(Random.Range(80, 140), 90, 90);

            //SplineAnimate settings
            _spot.mySplineAnimate.Container = _spot.mySpline;
            _spot.mySplineAnimate.Alignment = SplineAnimate.AlignmentMode.None;
            _spot.mySplineAnimate.AnimationMethod = SplineAnimate.Method.Speed;

            if (_speed == 0)
            {
                _spot.mySplineAnimate.MaxSpeed = 0.2f;
                _spot.mySplineAnimate.Pause();
            }
            else
            {
                _spot.mySplineAnimate.MaxSpeed = _speed;
                if (_play == false) _spot.mySplineAnimate.Pause();
                else _spot.mySplineAnimate.Play();
            }
            _spot.mySplineAnimate.PlayOnAwake = true;

        }
    }

    //change speed of animation for an entire list of spots
    public void ChangeAnimationSpeed(ref List<Spot> spotsList, float _speed)
    {
        if (spotsList.Count != 0)
        {
            if (spotsList[0].mySpline != null) //verify if the elements in the list have a spline for animation
            {
                foreach (var spot in spotsList)
                {

                    if (spot.mySplineAnimate != null && spot.mySplineAnimate.MaxSpeed != _speed)
                    {
                        if (_speed == 0)
                        {
                            spot.mySplineAnimate.MaxSpeed = 0.2f;
                            spot.mySplineAnimate.Pause();
                        }
                        else
                        {
                            float myOldTime = spot.mySplineAnimate.ElapsedTime;
                            float myOldSpeed = spot.mySplineAnimate.MaxSpeed;
                            spot.mySplineAnimate.MaxSpeed = _speed;
                            spot.mySplineAnimate.Play();
                            //move the object to the same position were it left before changing speed - otherwise it will be teleported to a different position
                            spot.mySplineAnimate.ElapsedTime = myOldTime * (1 / _speed) * myOldSpeed;
                        }

                        this.speed = _speed;
                        Debug.Log("speed on spot was changed to " + spot.mySplineAnimate.MaxSpeed + " and .Play() is" + spot.mySplineAnimate.IsPlaying);
                    }
                }
            }
        }
    }

    public void UpdateAnimationPosition_FollowSpot(Spot _spot)
    {
        if (_spot.mySpline != null)
        {
            _spot.mySpline.transform.localPosition = _spot.transform.localPosition;
        }
    }

    public void ChangeAnimationSpline_onSpot(Spot _spot, SplineContainer _mySpline)
    {
        if (_spot.mySpline != null)
        {
            Destroy(_spot.mySpline.gameObject);

            _spot.mySpline = Instantiate(_mySpline, this.transform, false);
            _spot.mySpline.transform.localPosition = _spot.transform.localPosition;
            _spot.mySpline.transform.localRotation = Quaternion.Euler(Random.Range(0, 90), 90, 90);



            _spot.mySplineAnimate.Container = _spot.mySpline;
        }


    }




    //ALL TOGHEDER
    public void SetSpotCollection(ref List<Spot> spotsList, float _speed, int _nrCircles, int _nrOfSpots, int _spotsPosition, float _spotRadius)
    {
        ChangeSpotRadius(ref spotsList, _spotRadius);

        ChangeNrOfSpots(ref spotsList, _nrOfSpots);

        ChangeNrCirclesOnSpot(ref spotsList, _nrCircles);

        ChangePositionRange(ref spotsList, ref this.actualSpotPosition, _spotsPosition);

        //make animation
        ChangeAnimationSpeed(ref spotsList, _speed);



    }
}
