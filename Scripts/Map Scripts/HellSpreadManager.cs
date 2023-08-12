using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellSpreadManager : MonoBehaviour
{
    static Material hellSpread;

    public static float spreadPercent = 0;
    const string spreadPercentRef = "Vector1_6881899c3e8b4f1884224bd766ae7708";


    void Start()
    {
        hellSpread = GetComponent<Renderer>().material;
    }

    void Update()
    {
       /* if (Input.GetKey(KeyCode.H))
        {
            spreadPercent += 10 * Time.deltaTime;
            hellSpread.SetFloat(spreadPercentRef, spreadPercent);
        }
        if (Input.GetKey(KeyCode.G))
        {
            spreadPercent -= 10 * Time.deltaTime;
            hellSpread.SetFloat(spreadPercentRef, spreadPercent);
        }
        */
    }

    public static void AddHell(float amount)
    {
        spreadPercent += amount;
        hellSpread.SetFloat(spreadPercentRef, spreadPercent);
    }


}
