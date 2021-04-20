using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameOver : MonoBehaviour
{
    [SerializeField] private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GetIsGameOver())
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Time.timeScale = 1;
                GameManager.Instance.LoadScene(GameManager.Instance.GetCurrentSceneIndex(), UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
        }
    }
}
