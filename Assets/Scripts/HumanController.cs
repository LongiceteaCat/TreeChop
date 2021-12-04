using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class HumanController : MonoBehaviour
{
    public Animator animator;
    public Animator uiAnimator;
    public Transform bridgeWood;
    public bool isChoping;
    public JoystickController dirGet;
    public float gravity = -19.8f;
    public Text woodCountText;
    public Transform groundCheck;
    public float groudDistance = 0.4f;
    public LayerMask groundMask, waterMask;
    public ParticleSystem pickupFX;
    public GameObject winText;
    [SerializeField]
    private CharacterController controller;
    [SerializeField]
    private float speed;
    private int woodCount = 0;
    private bool isRunning, isGrounded, isDead, isBuilding;
    private Transform humanMesh;
    private Vector3 move, velocity;
    private Collider bridge;
    
   
    void Start()
    {
        controller = GetComponent<CharacterController>();
        humanMesh = transform.GetChild(0);
        isChoping = false;
        woodCountText.text = woodCount.ToString();

    }

    
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groudDistance, groundMask);
        isDead = Physics.CheckSphere(groundCheck.position, groudDistance, waterMask);
        if (Input.GetKeyDown(KeyCode.Space) || isDead)
        {
            Respawn();
        }
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        if (JoystickController.dir != Vector2.zero)
        {
            Run();
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
        animator.SetBool("isChopping", isChoping);

    }
    void Run()
    {
        move = new Vector3(JoystickController.dir[0], 0, JoystickController.dir[1]);
        controller.Move(move * Time.deltaTime * speed);
        isRunning = move.Equals(Vector3.zero) ? false : true;
        humanMesh.rotation = Quaternion.LookRotation(move);
        Vector3 v = humanMesh.rotation.eulerAngles;
        humanMesh.rotation = Quaternion.Euler(-90, v.y, v.z);
        animator.SetBool("isRunning", isRunning);
    }
    void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        controller.enabled = false;
        transform.position = new Vector3(0, 20, 7);
        controller.enabled = true; //:)
        isDead = false;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if (hit.collider.tag == "Wood")
        {
            Destroy(hit.gameObject);
            uiAnimator.SetTrigger("popTrigger");
            woodCount++;
            woodCountText.text = woodCount.ToString();
            pickupFX.gameObject.SetActive(true);
            pickupFX.Play();
            
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tree"))
        {
            if(other.GetComponent<Evergreen>().isChoped == false){
                isChoping = true;
            }else{
                 isChoping = false;
            }
           
            
        }
        if (other.CompareTag("Win"))
        {
            winText.SetActive(true);
        }
        if (other.CompareTag("Bridge"))
        {
            isBuilding = true;
            if (woodCount > 0 && isBuilding == true)
            {
                woodCount--;
                Instantiate(bridgeWood, other.gameObject.transform.position, Quaternion.Euler(-90, 90, 0));
                other.gameObject.transform.Translate(Vector3.left * 0.9f);
                woodCountText.text = woodCount.ToString();
                other.GetComponent<BridgeCompleter>().woodBridge++;
                
        
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Tree"))
        {
            isChoping = false;
            animator.SetBool("isChopping", isChoping);
        }
        if (other.CompareTag("Win"))
        {
            winText.SetActive(false);
        }
        if (other.CompareTag("Bridge"))
        {
            isBuilding = false;
        }
    }
}

