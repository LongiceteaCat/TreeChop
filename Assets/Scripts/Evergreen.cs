using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Evergreen : MonoBehaviour
{
    public Transform wood;
    public bool isChoped;
    public HumanController humanController;
    public ParticleSystem chopFX;
    public CinemachineVirtualCamera cv;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float timeScale = 10;
    [SerializeField]
    private float recoverTime = 10;
    private bool onetime;
    [SerializeField]
    private float force;
    [SerializeField]
    private int maxWoodCount = 10;
    [SerializeField]
    private float treeHealth = 60f;
    private bool isChoping, isGrowed;
    private Collider col;




    void Start()
    {
        isChoped = false;
        isGrowed = true;
        col = GetComponent<Collider>();
    }

    void Update()
    {
        if (isChoping && !isChoped && isGrowed)
        {
            animator.SetBool("isChoping", true);
            if (!chopFX.isPlaying)
            {
                chopFX.Play();
            }
            else
            {
                chopFX.Stop();
            }
            treeHealth -= timeScale * Time.deltaTime;
        }
        else
        {
            animator.SetBool("isChoping", false);
        }
        if (treeHealth < 0 && !onetime) GrowSequence();


    }
    void GrowSequence()
    {
        onetime = true;
        isChoped = true;
        animator.SetInteger("TreeState", 0);

        if (isChoped)
        {
            if (isGrowed)
            {
                StartCoroutine(CameraShake());
            }
            isChoping = false;
            isGrowed = false;
            col.enabled = false;
            humanController.isChoping = false;
            for (int i = 0; i < maxWoodCount + 1; i++)
            {
                SpawnWood(i);
                if (i == maxWoodCount)
                {
                    Invoke("Grow", recoverTime);
                }
            }
        }
    }
    IEnumerator CameraShake()
    {
        yield return new WaitForSeconds(0.9f);
        cv.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 1;
        //Handheld.Vibrate();
        yield return new WaitForSeconds(0.4f);
        cv.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
    }
    void SpawnWood(int i)
    {
        //var instance = Instantiate(wood, transform.position + new Vector3(0 * i * 0.1f, 3, 5 * i * 0.1f), Quaternion.identity);
        var instance = Instantiate(wood, transform.position, Quaternion.identity, transform);
        instance.transform.localPosition = new Vector3(1, 2, 4);
        instance.transform.localRotation = Quaternion.Euler(new Vector3(Random.Range(0f, 360f), 0, Random.Range(0f, 360f)));

    }
    void Grow()
    {
        col.enabled = true;
        animator.SetInteger("TreeState", 3);
        treeHealth = 60;
        isGrowed = true;
        onetime = false;
        isChoped = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isGrowed) isChoping = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChoping = false;
        }
    }
}
