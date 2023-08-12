using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class GameLogic : MonoBehaviour
{
    //types of destructable objects, set on each prefab
    public enum DestructableObjects { House, SmallProps, NPC, Animal, Nature };

    //public list of a destructable object struct which hold the type and hell spread amount set in inspector
    public List<DstrObject> destructibleObjectValues;

    //links the object type to the struct holding the hell values
    private static Dictionary<DestructableObjects, DstrObject> objectsInfo;

    //appears when player kills all bosses
    private PauseMenu pausemenu; 
    private List<bool> bossKill = new List<bool>() { false,false,false};

    [Header("Hell Spreading")]
    public HellSpreadManager hellSpreadManager; //manages the shader
    public float maxSpreadPerSecond;
    private float spreadPerinterval;    //how fast the hell can spread per interval
    private static float targetSpreadPercent;
    private float intervalLength = 0.01f;
    private float prevUpdateTime = 0;

    public Slider destructionPercentageBar;

    [Header("Boss")]
    public Text bossTitleText;
    private bool bossSpawned = false;
    public GameObject bossHealthBar;

    [Header("Bosses")]
    public BossInfo HolyPig;
    public BossInfo Priest;
    public BossInfo Paladin;

    [Header("Audio")]
    public AudioSource AmbienceSource;
    public AudioClip DemonBackgroundnoise;
    bool changed = false;

    private void Start()
    {
        HellSpreadCalcs();
        BossInit();
        pausemenu = GetComponent<PauseMenu>();
    }

    private void BossInit()
    {
        Priest.bossObj.SetActive(false);
        bossHealthBar.SetActive(false);
        bossTitleText.enabled = false;
    }

    private void HellSpreadCalcs()
    {
        targetSpreadPercent = 0;
        objectsInfo = new Dictionary<DestructableObjects, DstrObject>();

        foreach (var dsrtObj in destructibleObjectValues)
        {
            objectsInfo.Add(dsrtObj.objType, dsrtObj);
        }

        //finds all hell spread objects in scene
        HellSpreadObj[] allHellSpreadObjs = Object.FindObjectsOfType<HellSpreadObj>();

        //finds the total value of hell spread by every object
        float totalHellSpread = 0;
        foreach (var obj in allHellSpreadObjs)
        {
            if(obj.ignoreThisObjForCalc == false)
            {
                totalHellSpread += objectsInfo[obj.destructableObjectType].hellSpreadAmount;
            }
        }

        //only need to destroy half of objects to cover map
        float alterAmount = 300f / totalHellSpread;

        //adjust the hell spread amount of each object
        foreach (var key in objectsInfo.Keys.ToList())
        {
            DstrObject newValue = objectsInfo[key];
            newValue.hellSpreadAmount *= alterAmount;

            objectsInfo[key] = newValue;
        }

        spreadPerinterval = maxSpreadPerSecond * intervalLength;
    }

    public static void spreadHell(DestructableObjects objType)
    {
        //HellSpreadManager.AdjustHell(objectsInfo[objType].hellSpreadAmount);
        targetSpreadPercent += objectsInfo[objType].hellSpreadAmount;
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Equals)){
            targetSpreadPercent += 0.1f;
        }

        if (Time.time - prevUpdateTime >= intervalLength)
        {
            if(targetSpreadPercent - HellSpreadManager.spreadPercent >= spreadPerinterval)
            {
                HellSpreadManager.AddHell(spreadPerinterval);
            }
            else
            {
                HellSpreadManager.AddHell(targetSpreadPercent - HellSpreadManager.spreadPercent);
            }

            prevUpdateTime = Time.time;
        }

        destructionPercentageBar.value = targetSpreadPercent;

        if(targetSpreadPercent > 50 && !changed)
        {
            AmbienceSource.clip = DemonBackgroundnoise;
            AmbienceSource.Play();
            changed = true;
        }
        AmbienceSource.volume = Controls.instance.volume;

    }

    public void slainBoss(BossType _boss)
    {
        switch (_boss)
        {
            case BossType.Pig:
                HolyPig.objectiveText.text = "<s>" + HolyPig.objectiveText.text + "</s>";
                bossKill[0] = true;
                break;
            case BossType.Priest:
                Priest.objectiveText.text = "<s>" + Priest.objectiveText.text + "</s>";
                bossKill[1] = true;
                break;
            case BossType.Paladin:
                Paladin.objectiveText.text = "<s>" + Paladin.objectiveText.text + "</s>";
                bossKill[2] = true;
                break;
        }

        //all bosses are defeated
        if(bossKill[0] && bossKill[1] && bossKill[2])
        {
            pausemenu.YouWinScreen();
        }
    }

    private void SpawnBoss(BossInfo boss)
    {
        if (boss.Spawned == false)
        {
            bossHealthBar.SetActive(true);
            bossTitleText.enabled = true;

            boss.bossObj.SetActive(true);
            boss.Spawned = true;
            bossTitleText.text = boss.bossTitleText;

            boss.objectiveText.fontStyle = FontStyles.Bold;
        }
    }

    public void spawnPigboss()
    {
        SpawnBoss(HolyPig);
    }

    public void spawnPriestboss()
    {
        SpawnBoss(Priest);
    }
    public void spawnPaladinboss()
    {
        SpawnBoss(Paladin);
    }
}

//stores the object type and spread amount
[System.Serializable]
public struct DstrObject{
    public GameLogic.DestructableObjects objType;
    public float hellSpreadAmount;
}

[System.Serializable]
public struct BossInfo
{
    public GameObject bossObj;
    public string bossTitleText;
    public bool Spawned;
    public TMP_Text objectiveText;
}