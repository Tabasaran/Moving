using LionStudios;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BoxController : MonoBehaviour
{

    public GameObject winEffect;

    public GameObject box;
    private Vector3 landingPosition;
    

    private Vector3 lastPosition;
    private bool isMoving;
    private bool isDropping;
    public float moveSpeed = 0.5f;

    private Vector3 max = new Vector3(0.8f, 0.85f, 0.9f);
    private Vector3 min = new Vector3(-0.9f, 0.125f, -0.8f);

    public GameObject boxOpenAnimation;
    public Item[] items;
    private int index = 0;
    private GameObject currentContour;
    private Rigidbody rb;

    private bool inContour;

    public Slider lvlSlider;

    [SerializeField]
    public LevelsCount levelsCount;

    private void Awake()
    {
        int sceneIndex = levelsCount.Lvl % (SceneManager.sceneCountInBuildSettings - 1);

        if ( sceneIndex == 0 && levelsCount.Lvl >= SceneManager.sceneCountInBuildSettings - 1)
        {
            sceneIndex = SceneManager.sceneCountInBuildSettings - 1;
        }

        if (sceneIndex != SceneManager.GetActiveScene().buildIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }

    private void Start()
    {
        LionKit.Initialize();

        landingPosition = transform.position;

        min.y = landingPosition.y;
        max.y += landingPosition.y;

        rb = GetComponent<Rigidbody>();
        StartCoroutine(DropBox()); //----------------
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastPosition = Input.mousePosition;
            isMoving = true;
        }
        else if (Input.GetMouseButtonUp (0))
        {
            isMoving = false;
            if (inContour)
            {
                SpawnItem();
                inContour = false;
            }
        }
    }

    private IEnumerator ChangeLvlSliderValue(float value)
    {
        float changeTime = 0.8f;
        int changesPerSecond = (int)(60 * changeTime);
        float changeValue = 1f / changesPerSecond;
        WaitForSeconds waitForSeconds = new WaitForSeconds(changeValue * changeTime);

        for (int i = 0; i < changesPerSecond; i++)
        {
            lvlSlider.value += changeValue;
            if (lvlSlider.value >= value)
            {
                break;
            }
            yield return waitForSeconds;
        }
    }

    private void SpawnItem()
    {
        HideIcon();

        float lvlSliderValue = (float)(index + 1) / items.Length;
        StartCoroutine(ChangeLvlSliderValue(lvlSliderValue));

        boxOpenAnimation.transform.position = transform.position;
        boxOpenAnimation.transform.rotation = transform.rotation;

        transform.rotation = Quaternion.identity;
        box.SetActive(false);


        boxOpenAnimation.SetActive(true);

        items[index].contour.SetActive(false);
        items[index++].item.SetActive(true);

        if (index == items.Length)
        {
            StartCoroutine(LvlComplete());
            index = 0;
            return;
        }
        StartCoroutine(DropBox());
    }

    private IEnumerator LvlComplete()
    {
        yield return new WaitForSeconds(1f);
        boxOpenAnimation.SetActive(false);
        yield return new WaitForSeconds(0.8f);
        winEffect.SetActive(true);
        yield return new WaitForSeconds(1.5f);

        Analytics.Events.LevelComplete(levelsCount.Lvl, null);

        //Load Next Scene
        int sceneIndex = ++levelsCount.Lvl % SceneManager.sceneCountInBuildSettings;

        if (sceneIndex == 0)
        {
            sceneIndex = SceneManager.sceneCountInBuildSettings - 1;
        }
        SceneManager.LoadScene(sceneIndex);
    }

    private void FixedUpdate()
    {
        if (isMoving && isDropping == false)
        {
            MoveBox(Input.mousePosition);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Contour"))
        {
            if (other.gameObject == currentContour)
            {
                currentContour.GetComponent<SpriteRenderer>().color = Color.green;
                inContour = true;
            }
            else
            {
                other.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Contour"))
        {
            other.gameObject.GetComponent<SpriteRenderer>().color = Color.white;

            if (other.gameObject == currentContour)
            {
                inContour = false;
            }
        }
    }

    private void ShowIcon()
    {
        items[index].Icon.SetActive(true);
    }

    public void HideIcon()
    {
        items[index].Icon.SetActive(false);
    }

    private void MoveBox(Vector3 position)
    {
        Vector3 move = Input.mousePosition - lastPosition;
        Vector3 newPosition = transform.position;
        newPosition += new Vector3(move.y - move.x, 0f, -move.y - move.x) * moveSpeed * Time.deltaTime;

        if (newPosition.y <= min.y)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            newPosition.y = min.y;
        }

        if ((newPosition.x > max.x || newPosition.y > min.y && Mathf.Abs(max.x - transform.position.x) < 0.001f) && newPosition.x - max.x > min.z - newPosition.z)
        {
            newPosition.y = Mathf.Clamp(newPosition.y + newPosition.x - max.x, min.y, max.y);
            newPosition.x = max.x;

            if (transform.position.y - min.y > 0.03f)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            }
        }
        else if (newPosition.z < min.z || newPosition.y > min.y && Mathf.Abs(min.z - transform.position.z) < 0.001f)
        {
            newPosition.y = Mathf.Clamp(newPosition.y - newPosition.z + min.z, min.y, max.y);
            newPosition.z = min.z;

            if (transform.position.y - min.y > 0.03f)
            {
                transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
            }
        }

        newPosition.x = Mathf.Clamp(newPosition.x, min.x, max.x);
        newPosition.y = Mathf.Clamp(newPosition.y, min.y, max.y);
        newPosition.z = Mathf.Clamp(newPosition.z, min.z, max.z);

        rb.MovePosition(newPosition);
        lastPosition = position;
    }

    public IEnumerator DropBox()
    {
        isDropping = true;

        yield return new WaitForSeconds(1.4f);
        boxOpenAnimation.SetActive(false);

        currentContour = items[index].contour;

        transform.position = landingPosition;
        box.SetActive(true);

        yield return new WaitForSeconds(1f);

        ShowIcon();
        yield return new WaitForSeconds(0.5f);
        isDropping = false;


    }
}

[System.Serializable]
public class Item
{
    public GameObject Icon;
    public GameObject item;
    public GameObject contour;
}