using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SeveralSpots : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Transform pointPrefab;

    public ValidateSAM myValidateSAM;

    [SerializeField]
    Spot spotPrefab;

    [SerializeField, Range(1, 30)]
    int nrOfSpots;

    [SerializeField, Range(1, 30)]
    public int nrCircles;

    //spotsPosition - 0=everywere on facade(y in range (0, 3f)), 1=buttom (y in range (0, 1f)), 2=center (y in range (1, 2f)), 3=up (y in range (2, 3f))
    [SerializeField, Range(0, 3)]
    public int spotsPosition;

    public int actualSpotPosition;

    public float speed;

    List<Spot> spotsList;


    //time
    public float myTotalTime; //for measuring the total time during the experiment
    public float myTime; //for setting the timeline
    public float changedTime;//for going back at a certain moment
    public int frameNumber;
    public int myFrameNumber;

    [TextAreaAttribute]
    public string myTextArea;
    public int nrOfCurrentSequence;

    //spline animate
    SplineAnimate mySplineAnimation;
    [SerializeField]
    SplineContainer randomSpline;
    [SerializeField]
    SplineContainer smallRandomSpline;


    float sequenceDuration;

    void Start()
    {
        myTime = 0f;
        myTotalTime = 0f;
        sequenceDuration = 15f;
        nrOfCurrentSequence = 0;
        myFrameNumber = 0;
        myTextArea = "start";

        changedTime = Time.deltaTime;

        speed = 0.2f;
        nrCircles = 2;
        nrOfSpots = 1;
        actualSpotPosition = spotsPosition = 0;


        spotsList = new List<Spot>();

        MakeSpots(1, 0f, 0f, 0f, 0f);
        if (nrOfSpots > 1)
        {
            MakeSpots(nrOfSpots - 1, -0.2f, 5f, -0.2f, 3f);
        }


        //spline animation
        mySplineAnimation = gameObject.AddComponent<SplineAnimate>();
        mySplineAnimation.Container = randomSpline;
        mySplineAnimation.Alignment = SplineAnimate.AlignmentMode.None;
        mySplineAnimation.AnimationMethod = SplineAnimate.Method.Speed;
        mySplineAnimation.MaxSpeed = 0.5f;
        mySplineAnimation.PlayOnAwake = false;
        mySplineAnimation.Pause();



    }

    // Update is called once per frame
    void Update()

    {

        //myTotalTime += Time.deltaTime; //do not modify later in script
        myTotalTime = Time.frameCount * 0.02f;
        frameNumber = Time.frameCount;

        myFrameNumber++;
        myTime = myFrameNumber * 0.02f;

        // SetSpotCollection(ref List<Spot> spotsList, float _speed, int _nrCircles, int _nrOfSpots, int _spotsPosition)

        if (myTime == 0.02f)
        {
            SetSpotCollection(ref spotsList, 0.2f, 2, 1, 0);
        }
        if (myTime > 0.02 && myTime < sequenceDuration)
        {
            if (myTextArea != "Random facade") nrOfCurrentSequence++;
            myTextArea = "Random facade";

            if (myTime % 2 == 0)
            {
                int[] choise = new int[2] { -1, 1 };

                //nrCircles = Random.Range(1, 4);
                // nrOfSpots += (nrOfSpots < 6 ? 1 : choise[Random.Range(0, 1)]);

                //nrOfSpots = Random.Range(1, 10);
                nrOfSpots++;


            }



            if ((myTime == 3f) ||
                (myTime == 6f) ||
                (myTime == 9f))
            {
                nrCircles++;
            }
            if ((myTime == 12f) ||
                (myTime == 14f))
            {
                nrCircles--;
            }

            //speed += 0.005f;
            speed = (Mathf.Sin(myTime / 15f * 2f * Mathf.PI) + 2f) / 2f;

            SetSpotCollection(ref spotsList, speed, nrCircles, nrOfSpots, spotsPosition);

        }
        if (myTime == sequenceDuration)
        {
            //myFrameNumber = 0;
        }


        if (myTime > sequenceDuration && myTime < 2 * sequenceDuration)
        {
            if (myTextArea != "Random facade 2") nrOfCurrentSequence++;
            myTextArea = "Random facade 2";

            nrCircles = 8;
            speed = (Mathf.Sin(myTime / 15f * 2f * Mathf.PI) + 2f) / 2f;
            
            if ((myTime == 18f) ||
                (myTime == 21f) ||
                (myTime == 25f))
            {
                nrCircles++;
            }

            if (myTime == 25f) nrOfSpots = 20;

            SetSpotCollection(ref spotsList, speed, nrCircles, nrOfSpots, spotsPosition);

        }


        if (myTime == 2 * sequenceDuration )
        {
            if (myTextArea != "medium speed") nrOfCurrentSequence++;
            myTextArea = "medium speed";

            ChangeNrCirclesOnSpot(ref spotsList, 3);
            ChangeNrOfSpots(ref spotsList, 10);
            ChangeAnimationSpeed(ref spotsList, 0.5f);
        }


        if (myTime > 3 * sequenceDuration && myTime < 3 * sequenceDuration + 1f)
        {
            myTextArea = "low speed";

            ChangeAnimationSpeed(ref spotsList, 0.1f);
        }


        if (myTime > 4 * sequenceDuration && myTime < 4 * sequenceDuration + 1f)
        {
            myTextArea = "high speed";

            ChangeAnimationSpeed(ref spotsList, 2f);
        }


        if (myTime > 5 * sequenceDuration && myTime < 5 * sequenceDuration + 1f)
        {
            myTextArea = "low nr of spots, medium speed";

            ChangeAnimationSpeed(ref spotsList, 0.5f);
            ChangeNrOfSpots(ref spotsList, 3);
        }


        if (myTime > 6 * sequenceDuration && myTime < 6 * sequenceDuration + 1f)
        {
            myTextArea = "medium nr of spots, medium speed";

            ChangeNrOfSpots(ref spotsList, 10);
        }


        if (myTime > 7 * sequenceDuration && myTime < 7 * sequenceDuration + 1f)
        {
            myTextArea = "high nr of spots, medium speed";

            ChangeNrOfSpots(ref spotsList, 20);
        }

        if (myTime > 95f && myTime < 96.02f)








        ChangeNrCirclesOnSpot(ref spotsList, nrCircles);

        ChangeNrOfSpots(ref spotsList, nrOfSpots);

        ChangePositionRange(spotsList, ref actualSpotPosition, spotsPosition);

        //ChangeAnimationSpeed(ref spotsList, speed);




    }









    //METHODS

    public void MakeSpots(int nrOfSpots, float minRangeX, float maxRangeX, float minRangeY, float maxRangeY)
    {
        for (int i = 0; i < nrOfSpots; i++)
        {
            Spot spot = Instantiate(spotPrefab, this.transform, false);
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
                MakeSpots(_nrOfSpots - spotsList.Count, -1f, 6f, -1f, 4f);
            }

            //update spotsList attributes
            this.nrOfSpots = spotsList.Count;
        }


    }


    //change the position of the spots without changing the list
    public void ChangePositionRange(List<Spot> spotsList, ref int actualSpotPosition, int spotsPosition)
    {

        if (spotsList.Count != 0 && actualSpotPosition != spotsPosition)

        {
            if (spotsPosition == 0)
            {
                foreach (var spot in spotsList)
                {
                    spot.transform.localPosition = new Vector3(spot.transform.localPosition.x, Random.Range(-0.2f, 3f), 0f);
                }
            }
            else if (spotsPosition == 1)
            {
                foreach (var spot in spotsList)
                {
                    spot.transform.localPosition = new Vector3(spot.transform.localPosition.x, Random.Range(-0.2f, 0.5f), 0f);
                }
            }

            else if (spotsPosition == 2)
            {
                foreach (var spot in spotsList)
                {
                    spot.transform.localPosition = new Vector3(spot.transform.localPosition.x, Random.Range(0.8f, 1.5f), 0f);
                }
            }
            else if (spotsPosition == 3)
            {
                foreach (var spot in spotsList)
                {
                    spot.transform.localPosition = new Vector3(spot.transform.localPosition.x, Random.Range(1.8f, 3f), 0f);
                }
            }

            //update animation spline
            foreach (var spot in spotsList)
            {
                UpdateAnimationPosition_FollowSpot(spot);
            }

            //update spotsList attributes
            actualSpotPosition = spotsPosition;
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
                    spot.mySpline.transform.localRotation = Quaternion.Euler(Random.Range(0, 90), 90, 90);

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
            _spot.mySpline = Instantiate(_mySpline, this.transform, false);
            _spot.mySpline.transform.localPosition = _spot.transform.localPosition;
            _spot.mySpline.transform.localRotation = Quaternion.Euler(Random.Range(0, 90), 90, 90);

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
                            spot.mySplineAnimate.MaxSpeed = _speed;
                            spot.mySplineAnimate.Play();
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
    public void SetSpotCollection(ref List<Spot> spotsList, float _speed, int _nrCircles, int _nrOfSpots, int _spotsPosition)
    {
        ChangeNrOfSpots(ref spotsList, _nrOfSpots);

        ChangeNrCirclesOnSpot(ref spotsList, _nrCircles);

        ChangePositionRange(spotsList, ref this.actualSpotPosition, _spotsPosition);

        //make animation
        ChangeAnimationSpeed(ref spotsList, _speed);

        //mySplineAnimation.MaxSpeed = _speed;

        //if (mySplineAnimation.MaxSpeed == 0f)
        //{
        //    mySplineAnimation.Pause();
        //}
        //else if (mySplineAnimation.IsPlaying == false) mySplineAnimation.Play();


    }
}
