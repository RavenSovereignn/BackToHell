using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priest : MonoBehaviour, IBoss
{
    public Transform player;
    public GameLogic gameLogicScript;

    [Header("General Stats")]
    public int maxHealth = 15;
    private int currentHealth;
    public HealthBar bossHealthBar;
    public float attackRange;

    [Header("Staff gun")]
    public GameObject projectile;
    public int sprayBulletCount;

    public Transform firePoint;
    private Vector3 firePointStartPos;
    private Quaternion firePointStartRot;

    [Header("Holy Beams")]
    public GameObject sunBeam;
    public float cooldownTime;
    public float beamMinDistBtw;    //how close beam can be to eachother
    public int sunBeamCount = 5;

    public Transform areaBotLeft;
    public Transform areaTopRight;
    private Vector4 areaWorldSpace;
    public List<Transform> priestTeleportLocations;
    private List<Vector3> teleportPositons; //holds world location of position 

    private AudioComponent audioComponent;

    void Start()
    {
        audioComponent = GetComponent<AudioComponent>();

        firePointStartPos = firePoint.localPosition;
        firePointStartRot = firePoint.localRotation;

        //store the whole area in vector 4
        areaWorldSpace = new Vector4(areaBotLeft.position.x, areaTopRight.position.x, areaBotLeft.position.z, areaTopRight.position.z);

        teleportPositons = new List<Vector3>();
        foreach(var t in priestTeleportLocations)
        {
            teleportPositons.Add(t.position);
        }

        currentHealth = maxHealth;
        bossHealthBar.SetMaxHealth(maxHealth);

        StartCoroutine(AttackSequence());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(Teleport());
        }

    }

    private IEnumerator AttackSequence()
    {
        while (true)
        {
            float rndAttack = Random.Range(0f, 1f);

            //if boss health above 70% always shoot projectiles
            if(currentHealth >= (maxHealth * 0.7) || rndAttack >= 0.5f)
            {
                StartCoroutine(BulletSpray(1f, 180, -90, 1, sprayBulletCount));

                yield return new WaitForSeconds(1f);

                StartCoroutine(BulletSpray(1f, 90, 45, -1, sprayBulletCount));

            }
            //30% chance to holy beam when below 70%
            else if(rndAttack >= 0.2f)
            {
                StartCoroutine(HolyBeams());
            }
            else//20% to teleport
            {
                StartCoroutine(Teleport());
            }
            yield return new WaitForSeconds(3.5f);

        }
    }


    private IEnumerator BulletSpray(float duration, float sprayAngle, float angleStart, int direction, int bulletCount)
    {
        //time delay between shots
        WaitForSeconds shotDelay = new WaitForSeconds(duration / (float)bulletCount);

        //resets the staff position and then roates it to the start point
        firePoint.localPosition = firePointStartPos;
        firePoint.localRotation = firePointStartRot;
        firePoint.RotateAround(transform.position, Vector3.up, angleStart);

        //rotate to look at player
        transform.LookAt(player.transform);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

        Vector3 projectileStartAngle = firePoint.rotation.eulerAngles;
        
        //angle change after each bullet
        float angleChange = sprayAngle / (float)bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            //find the new angle of the staff
            Vector3 bulletAngle = projectileStartAngle;
            bulletAngle.y += angleChange * i * direction;

            //rotate the staff to shoot one bullet
            firePoint.RotateAround(transform.position, Vector3.up, angleChange * direction);

            GameObject g = Instantiate(projectile, firePoint.position, firePoint.rotation);

            audioComponent.PlaySound(0, null, firePoint.position, 0.5f);

            yield return shotDelay;
        }
        yield return null;
    }

    private IEnumerator HolyBeams()
    {
        //holds all positions of the holy beams
        List<Vector3> beamStrikePoses = new List<Vector3>();

        int iterations = 0; //safety check
        while(beamStrikePoses.Count < sunBeamCount && iterations < 100)
        {
            //random positon within given area
            Vector3 pos = new Vector3(Random.Range(areaWorldSpace.x, areaWorldSpace.y), -0.25f, Random.Range(areaWorldSpace.z, areaWorldSpace.w));

            bool nearbyCheck = true;
            //check the new strike pos is not near others
            foreach(Vector3 otherPos in beamStrikePoses)
            {
                if(Vector3.Distance(pos,otherPos) < beamMinDistBtw)
                {
                    //dont add this positon to the list
                    nearbyCheck = false;
                }
            }

            //adds the new position to the list if its not close to others
            if (nearbyCheck) { beamStrikePoses.Add(pos); }
            iterations++;
        }

        //creates the beams
        foreach (Vector3 beamPos in beamStrikePoses)
        {
            Instantiate(sunBeam, beamPos, Quaternion.identity);
        }

        yield return null;
    }

    private IEnumerator Teleport()
    {
        Vector3 newLocation = new Vector3();
        bool found = false;

        //loops through till found new location
        int iterations = 0; //safety check
        while (!found || iterations >100)
        {
            newLocation = teleportPositons[Random.Range(0, priestTeleportLocations.Count)];
            if(Vector3.Distance(newLocation,transform.position) > 1)
            {
                found = true;
            }
            iterations++;
        }

        transform.position = newLocation;
        yield return null;
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        bossHealthBar.SetHealth((int)currentHealth);
        if (currentHealth <= 0)
        {
            gameLogicScript.slainBoss(BossType.Priest);
            Destroy(gameObject);
        }
    }
}
