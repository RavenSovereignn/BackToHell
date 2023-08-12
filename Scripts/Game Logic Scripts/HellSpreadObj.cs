using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellSpreadObj : MonoBehaviour
{
    public GameLogic.DestructableObjects destructableObjectType;

    public bool ignoreThisObjForCalc = false;
    private void OnDestroy()
    {
        GameLogic.spreadHell(destructableObjectType);
    }
}
