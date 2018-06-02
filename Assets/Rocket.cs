using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Rocket : MonoBehaviour
{

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;

    bool isTransitioning = false;
    bool ignoreCollision = false;
    bool allowAdvancingLevel = false;

    public Light wayLight;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        wayLight = GameObject.Find("WayLight").GetComponent<Light>();

        CheckIfOrbExists();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (!isTransitioning)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKey(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKey(KeyCode.C))
        {
            ignoreCollision = !ignoreCollision; //toggle
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || ignoreCollision) { return; }//ignore collisions while not alive or while variable is toggled

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                break;

            case "Finish":
                StartSuccessSequence();
                break;

            case "Orb":
                Invoke("CheckIfOrbExists", 0.1f); //checks if orb exists in scene, if not you can advance
                break;

            default:
                StartDeathSequence();
                break;
        }
    }

    private void CheckIfOrbExists()
    {
        if (GameObject.FindWithTag("Orb"))
        {
            //do not allow advancing in the next scene
            wayLight.intensity = 0;
            print("orbfound");
        }
        else
        {
            allowAdvancingLevel = true; //if there is no orbs left, allow advancing level
            wayLight.intensity = 10;
            print("advance level activated");
        }
    }

    private void StartSuccessSequence()
    {
        if (allowAdvancingLevel)
        {
            isTransitioning = true;
            audioSource.Stop();
            mainEngineParticles.Stop();
            //transform.Find("Spot Light").gameObject.SetActive(false); //stop audio source, thrust and spot light on rocket, 3 coded lines above
            audioSource.PlayOneShot(success);
            successParticles.Play();
            Invoke("LoadNextLevel", levelLoadDelay);
        }
    }

    private void StartDeathSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        mainEngineParticles.Stop();
        audioSource.PlayOneShot(death);
        deathParticles.Play();
        RocketShipDisappear();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void RocketShipDisappear()
    {
        GameObject rocketShip = GameObject.Find("Rocket Ship");
        rocketShip.transform.localScale = new Vector3(0, 0, 0); //make rocket invisible after touching wall or exploding
        rocketShip.transform.Find("Rocket Light").gameObject.SetActive(false); //setting both of the light attached on rocket inactive while dying
        rocketShip.transform.Find("Spot Light").gameObject.SetActive(false);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);

        if (GameObject.Find("RedLevelIdentificator"))
        {
            SceneManager.LoadScene("Level 1");
        }
        else if (GameObject.Find("BlueLevelIdentificator"))
        {
            SceneManager.LoadScene("Level 4");
        }
        else
        {
            //todo go to the first purple level
        }
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            StopApplyingThrust();
        }
    }

    private void StopApplyingThrust()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust /* Time.deltaTime*/);
        if (!audioSource.isPlaying) //so it doesn't layer
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }

    private void RespondToRotateInput()
    {
        rigidBody.angularVelocity = Vector3.zero; //remove rotation due to physics

        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
    }
}
