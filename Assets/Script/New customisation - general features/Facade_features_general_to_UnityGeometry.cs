using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

public class Facade_features_general_to_UnityGeometry : MonoBehaviour
{
    public SeveralSpots my_severalSpots;

    public Slider _procentageOpaqueOfFacade;
    public Slider _compactnessOfElement;
    public Slider _realSpeed;
    public Slider _compactnessOfFacade;
    public Slider _boundingRadiusOfElementProc;

    public float _areaFacadeTotal;
    public float _facadeDiagonal;
    public float _minimalUnitRadius;

    public int nrCircles_;
    public int nrOfSpots_;
    public float speed_;
    public float elementBoundingRadius_;
    public float compactness_facade_percentage_;


    public int finalSpotsPosition;
    public float testCompactnessOfFacadeFinal;
    public int nrOfMinimalUnitsPerSpot;

    public int test_nrMinUnits;
    public bool recalculate;

    // Start is called before the first frame update
    void Start()
    {
        _areaFacadeTotal = my_severalSpots.facadeArea;
        _facadeDiagonal = my_severalSpots.facadeDiag;
        _minimalUnitRadius = 0.026f;

        recalculate = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (this._areaFacadeTotal != my_severalSpots.facadeArea) this._areaFacadeTotal = my_severalSpots.facadeArea;
        if (this._facadeDiagonal != my_severalSpots.facadeDiag) this._facadeDiagonal = my_severalSpots.facadeDiag;


        //if (speed_ != CalculateSpeed_toUnity(_realSpeed.value))
        //{
        //    speed_ = CalculateSpeed_toUnity(_realSpeed.value);
        //    my_severalSpots.speed = speed_;

        //}

        //if (elementBoundingRadius_ != CalculateBoungRadius_toUnity(_boundingRadiusOfElementProc.value/20f, _facadeDiagonal))
        //{
        //    elementBoundingRadius_ = CalculateBoungRadius_toUnity(_boundingRadiusOfElementProc.value/20f, _facadeDiagonal);
        //    my_severalSpots.spotRadius = elementBoundingRadius_;
        //}

        compactness_facade_percentage_ = _compactnessOfFacade.value;

        (nrCircles_, nrOfSpots_, elementBoundingRadius_, speed_, nrOfMinimalUnitsPerSpot) = GeneralToUnityGeometry(
                                                                                        _procentageOpaqueOfFacade.value / 10f,
                                                                                        _compactnessOfElement.value/10f,
                                                                                        _compactnessOfFacade.value,
                                                                                        _boundingRadiusOfElementProc.value / 20f,
                                                                                        _realSpeed.value,
                                                                                        this._areaFacadeTotal,
                                                                                        this._facadeDiagonal,
                                                                                        this._minimalUnitRadius);


        if (recalculate == true)
        {
            //float opaqueArea = _procentageOpaqueOfFacade.value * this._areaFacadeTotal;
            //int k = 0;
            //int ok = 0;
            ////we know in this case there is only one spot
            //while (k < 20 && ok == 0)
            //{
            //    k++;
            //    if (CalculateNrOfMinUnitsPerSpot(k, this.elementBoundingRadius_) <= opaqueArea * 1.2 &&
            //        CalculateNrOfMinUnitsPerSpot(k, this.elementBoundingRadius_) >= opaqueArea * 0.8)
            //    {
            //        nrCircles_ = k;
            //        _compactnessOfElement.value = (int)(opaqueArea / (elementBoundingRadius_ * elementBoundingRadius_ * 3.14) * 10);
            //        ok = 1;
            //    }
            //}

        }

        if (my_severalSpots.spotRadius != elementBoundingRadius_)
        {
            my_severalSpots.spotRadius = elementBoundingRadius_;
        }
        if (my_severalSpots.nrCircles != nrCircles_)
        {
            my_severalSpots.nrCircles = nrCircles_;
        }
        if (my_severalSpots.nrOfSpots != nrOfSpots_)
        {
            my_severalSpots.nrOfSpots = nrOfSpots_;
        }
        if (my_severalSpots.speed != speed_)
        {
            my_severalSpots.speed = speed_;
        }

        if (my_severalSpots._compactness_facade_percentage != compactness_facade_percentage_)
        {
            my_severalSpots._compactness_facade_percentage = compactness_facade_percentage_;
        }

        test_nrMinUnits = (CalculateNrOfMinUnitsPerSpot(this.nrCircles_, this.elementBoundingRadius_));


    }




    public float CalculateSpeed_toUnity(float _realSpeed)
    {
        speed_ = _realSpeed / 34.67f;
        return speed_;
    }

    public float CalculateBoungRadius_toUnity(float _boundingRadiusOfElementProc, float _facadeDiagonal)
    {
        elementBoundingRadius_ = _boundingRadiusOfElementProc * _facadeDiagonal;
        return elementBoundingRadius_;
    }

    public int CalculateNrOfMinUnitsPerSpot(int nrCircles_, float spotRadius_)
    {
        float nrOfMinUnits_ = 0f;
        for (int k = nrCircles_; k >= 0; k--)
        {
            if (k == nrCircles_) nrOfMinUnits_ += Mathf.Sqrt(k * 100f * (spotRadius_ * k / nrCircles_)) + 2f;
            else if (k == 0) nrOfMinUnits_ += 1;
            else nrOfMinUnits_ += Mathf.Sqrt(k * 100f * spotRadius_ * k / nrCircles_ * (1f - 0.4f * k / (1f + 0.4f * k))) + 2f + k;
        }
        return (int)nrOfMinUnits_;
    }


    public float CalculateCompactnessOfFacade(int spotsPosition, float areaFacadeTotal, int nrOfSpots, float spotRadius, float facadeDiagonal)
    {
        float totalAreaOfSpot = Mathf.Pow(spotRadius, 2) * Mathf.PI;
        float totalAreaOfElement = Mathf.Pow(spotRadius, 2) * 4;
        int maxNrOfElements;

        if (spotsPosition == 0)
        {
            maxNrOfElements = (int)(areaFacadeTotal / totalAreaOfSpot);
        }
        else
        {
            maxNrOfElements = (int)(areaFacadeTotal / totalAreaOfSpot / 3);
        }

        int emptyNumberOfElements = maxNrOfElements - nrOfSpots;
        float filledProportionOnFacade = emptyNumberOfElements / nrOfSpots;
        float maxiumDistanceBetweenElements;

        if (emptyNumberOfElements < maxNrOfElements * 0.75)
        {
            if (emptyNumberOfElements <= 2)
            {
                maxiumDistanceBetweenElements = 0;
            }
            else
            {
                maxiumDistanceBetweenElements = filledProportionOnFacade * 2 * spotRadius;
            }
        }
        else
        {
            maxiumDistanceBetweenElements = 3;
        }

        float compactnessOfFacade = (float)Mathf.Round(maxiumDistanceBetweenElements * 10000f) / 10000f;
        return compactnessOfFacade;
    }





    public (int, int, float, float, int) GeneralToUnityGeometry(float procentageOpaqueOfFacade,
                                                                                        float compactnessOfElement,
                                                                                        float compactnessOfFacade,
                                                                                        float boundingRadiusOfElementProc,
                                                                                        float realSpeed,
                                                                                        float areaFacadeTotal,
                                                                                        float facadeDiagonal,
                                                                                        float minimalUnitRadius)
    {
        float elementBoundingRadius = boundingRadiusOfElementProc * facadeDiagonal;
        float totalAreaOfElement = Mathf.Pow(elementBoundingRadius, 2) * Mathf.PI;
        float opaqueAreaOfElement = totalAreaOfElement * compactnessOfElement;
        int nrOfMinimalUnitsPerSpot = (int)(opaqueAreaOfElement / (Mathf.Pow(minimalUnitRadius, 2) * Mathf.PI));

        recalculate = false;
        int ok = 0;
        int i = 0;
        int nrCircles = 0;
        int testNrCircles = 0;
        while (i < 20 && ok == 0)
        {
            i++;
            testNrCircles = i;
            int testNrMinUnits = CalculateNrOfMinUnitsPerSpot(testNrCircles, elementBoundingRadius);
            if (testNrMinUnits == nrOfMinimalUnitsPerSpot)
            {
                ok = 1;
                nrCircles = testNrCircles;
            }
        }

        i = 0;
        if (ok == 0)
        {
            while (i < 40 && ok == 0)
            {
                i++;
                testNrCircles = i;
                int testNrMinUnits = CalculateNrOfMinUnitsPerSpot(testNrCircles, elementBoundingRadius);
                if (testNrMinUnits > nrOfMinimalUnitsPerSpot * (1 - 0.5) && testNrMinUnits < nrOfMinimalUnitsPerSpot * (1 + 0.5))
                {
                    ok = 1;
                    nrCircles = testNrCircles;
                }
            }
        }

        if (ok == 0)
        {
            nrCircles = 1;
            int testNrMinUnits = CalculateNrOfMinUnitsPerSpot(nrCircles, elementBoundingRadius);
        }

        float opaqueAreaOfFacade = procentageOpaqueOfFacade * areaFacadeTotal;
        int nrOfSpots = (int)(opaqueAreaOfFacade / (nrOfMinimalUnitsPerSpot * Mathf.PI * Mathf.Pow(0.026f, 2f)));
        if (nrOfSpots <= 0)
        {
            nrOfSpots = 1;
            recalculate = true;
        }

        float speed = Mathf.Round(realSpeed / 34.67f * 100) / 100;
        float testCompactnessOfFacade = -1;

        return (nrCircles, nrOfSpots, Mathf.Round(elementBoundingRadius * 100) / 100, speed, nrOfMinimalUnitsPerSpot);
    }

}
