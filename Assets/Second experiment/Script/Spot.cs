using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class Spot : MonoBehaviour
{

    public Transform pointPrefab;

    [SerializeField, Range(1f, 10f)]
    public float opacityDegree;
    [SerializeField, Range(0f, 0.5f)]
    public float opacityGradient;

    [SerializeField, Range(1, 30)]
    public int nrCircles;

    [SerializeField, Range(0f, 4f)]
    public float spotRadius;

    public Transform[] points;

    public List<Transform> pointsList;
    // public SplineAnimate splineAnimation;

    //[SerializeField]
    //public SplineContainer randomSpline;


    //attributes from past frame
    float previousOpacityDegree;
    float previousOpacityGradient;
    public int previousNrCircles;
    float previousSpotRadius;


    public SplineContainer mySpline;
    public SplineAnimate mySplineAnimate;
    public float mySplineLength;

    // Start is called before the first frame update
    void Start()
    {
        //disable spline animation
        //splineAnimation = GetComponent<SplineAnimate>();
        // splineAnimation.enabled = false;
        //splineAnimation.splineContainer.Spline =randomSpline.AddSpline();



        previousOpacityDegree = opacityDegree = 4.3f;
        previousOpacityGradient = opacityGradient = 0.4f;
        previousNrCircles =  6;
        previousSpotRadius = spotRadius = 0.6f;


        Vector3 position = new Vector3(0f, 0f, 0f);
        Vector3 scale = new Vector3(1f, 0.01f, 1f);

        //this.transform.localPosition = position;
        this.transform.localRotation = Quaternion.identity;

        pointsList = new List<Transform>();
        //the origin of the circles on which the points are located are in 0,0,0
        pointsList = MakeSpot(spotRadius, nrCircles);

        if(mySpline!=null)    mySplineLength = mySpline.CalculateLength();

    }


    // Update is called once per frame
    void Update()
    {
        this.transform.localRotation = Quaternion.identity;

        float time = Time.time;
        //int k = 1;
        //int grow = 1;

        if (previousOpacityDegree != opacityDegree ||
            previousOpacityGradient != opacityGradient ||
            previousNrCircles != nrCircles ||
            previousSpotRadius != spotRadius)
        {
            if (pointsList.Count != 0)
            {
                foreach (var disk in pointsList)
                {
                    Destroy(disk.gameObject);
                }
                pointsList.Clear();
            }

            pointsList = MakeSpot(spotRadius, nrCircles);

            previousOpacityDegree = opacityDegree;
            previousOpacityGradient = opacityGradient;
            previousNrCircles = nrCircles;
            previousSpotRadius = spotRadius;

        }



    }

    //returns points on one circle; this points will be the origion of the minimal units
    private Vector3[] DrawCirclePoints(int nrSegments, float radius, Vector3 origin, float offset)
    {
        Vector3[] positions = new Vector3[nrSegments];
        float angle = 0f;
        //radial offset the first point on circle 
        if (offset != 0)
        {
            angle = Mathf.PI / nrSegments;
        }
        for (int i = 0; i < nrSegments; i++)
        {
            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);
            positions[i] = new Vector3(x, y, 0) + origin;
            angle += 2f * Mathf.PI / nrSegments;
            //Debug.Log(positions[i].x + ", " + positions[i].y + " " + positions[i].z);
        }

        return positions;
    }

    //makes all the circles contained in the spot and the minimal units on those circles
    private List<Transform> MakeSpot(float spotRadius, float nrCircles)
    {
        List<Transform> newList;
        newList = new List<Transform>();
        float circleRadius;
        float step = spotRadius / nrCircles;
        Vector3 testOrigin = new Vector3(0f, 0f, 0f);
        //scale = new Vector3(step, 0.1f, step);
        //int totalNrUnits = 0;
        int nrSegments = 0;


        for (int k = (int)nrCircles; k >= 0; k--)
        {
            //for having the minimalUnitRadius proportional to spotRadius
            //float mUnitRadius = spotRadius / (2 * opacityDegree + 1.2f * (Mathf.Log(k + 1f, 2f)) + 1);

            //for having the minimaUnitRadius constant
            float mUnitRadius = 0.6f / (2 * opacityDegree + 1.2f * (Mathf.Log(k + 1f, 2f)) + 1);
            circleRadius = spotRadius * (k / nrCircles);
            //make gradient in radial circles
            if (k != 0 && k != nrCircles)
            {
                circleRadius = circleRadius - circleRadius / (1 / opacityGradient * k + 1);
            }

            if (k == 0) nrSegments = 1;
            else if (k == nrCircles) nrSegments = (int)(k == 0 ? 1 : Mathf.Sqrt(k * circleRadius * 100) + 2);
            else nrSegments = (int)(k == 0 ? 1 : Mathf.Sqrt(k * circleRadius * 100) + 2 + k);

            Vector3[] circlePoints = DrawCirclePoints(nrSegments, circleRadius, testOrigin, (k % 2 != 0 ? 0f : 1f)); //offset when 1

            //Debug
            //Debug.Log("nr Points on circle " + k + " is " + (int)(Mathf.Sqrt(k / 2f * circleRadius * 100) + 3));
            //totalNrUnits += (int)(Mathf.Sqrt(k / 2f * circleRadius * 100) + 3);
            //Debug.Log("totalNrUnits= " + totalNrUnits);


            foreach (var cPoint in circlePoints)
            {
                Transform mUnit = Instantiate(pointPrefab);
                mUnit.localScale = new Vector3(mUnitRadius, 0.002f, mUnitRadius);
                mUnit.localPosition = cPoint;
                mUnit.SetParent(this.transform, false);
                newList.Add(mUnit);
            }
        }
        return newList;
    }
}
