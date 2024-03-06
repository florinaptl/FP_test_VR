using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        renderValidate = GetComponent<MeshRenderer>();
        renderValidate.material = invalidMaterial;

        myEmotion = new int[3];
        myEmotion[0] = pleasureValue = 0;
        myEmotion[1] = arousalValue = 0;
        myEmotion[2] = dominanceValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        pleasureValue = myEmotion[0];
        arousalValue = myEmotion[1];
        dominanceValue = myEmotion[2];

        if((pleasureValue==0)|| (arousalValue==0)|| (dominanceValue==0))
        {
            renderValidate.material = invalidMaterial;
        }else renderValidate.material = validMaterial;
    }

    void OnMouseDown()
    {
        Debug.Log("validation clicked");
        //write the values to file
        myEmotion[0] = pleasureValue = 0;
        myEmotion[1] = arousalValue = 0;
        myEmotion[2] = dominanceValue = 0;
    }
}
