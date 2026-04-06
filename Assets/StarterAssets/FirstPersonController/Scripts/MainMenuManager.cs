using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject settingsPanel;
    public GameObject creditsPanel;

    [Header("Buttons")]
    public Button playButton;
    public Button settingsButton;
    public Button creditsButton;
    public Button quitButton;

    [Header("Hover Settings")]
    public Color normalColor = new Color(0, 0, 0, 0);
    public Color hoverColor = new Color(0.6f, 0.1f, 0.1f, 0.3f);
    public Color clickColor = new Color(0.8f, 0.1f, 0.1f, 0.5f);
    public float hoverScale = 1.05f;
    public float transitionSpeed = 8f;

    void Start()
    {
        // Link button functions
        playButton.onClick.AddListener(PlayGame);
        settingsButton.onClick.AddListener(OpenSettings);
        creditsButton.onClick.AddListener(OpenCredits);
        quitButton.onClick.AddListener(QuitGame);

        // Add hover effects automatically to all buttons
        AddHoverEffect(playButton);
        AddHoverEffect(settingsButton);
        AddHoverEffect(creditsButton);
        AddHoverEffect(quitButton);
    }

    // ---- MENU FUNCTIONS ----
    void PlayGame()
    {
        SceneManager.LoadScene("terrain2");
    }

    void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    void OpenCredits()
    {
        if (creditsPanel != null)
            creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        if (creditsPanel != null)
            creditsPanel.SetActive(false);
    }

    void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit!");
    }

    // ---- HOVER EFFECT SETUP ----
    void AddHoverEffect(Button btn)
    {
        HoverHandler hover = btn.gameObject.AddComponent<HoverHandler>();
        hover.normalColor = normalColor;
        hover.hoverColor = hoverColor;
        hover.clickColor = clickColor;
        hover.hoverScale = hoverScale;
        hover.transitionSpeed = transitionSpeed;
    }
}

// ---- HOVER HANDLER (auto attached, no manual setup needed) ----
public class HoverHandler : MonoBehaviour, 
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Color normalColor;
    public Color hoverColor;
    public Color clickColor;
    public float hoverScale = 1.05f;
    public float transitionSpeed = 8f;

    private Image buttonImage;
    private Vector3 originalScale;
    private Color targetColor;
    private bool isHovering = false;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        originalScale = transform.localScale;
        targetColor = normalColor;
        if (buttonImage != null)
            buttonImage.color = normalColor;
    }

    void Update()
    {
        if (buttonImage != null)
            buttonImage.color = Color.Lerp(
                buttonImage.color, targetColor, 
                Time.deltaTime * transitionSpeed);

        Vector3 targetScale = isHovering ? 
            originalScale * hoverScale : originalScale;
        transform.localScale = Vector3.Lerp(
            transform.localScale, targetScale, 
            Time.deltaTime * transitionSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        targetColor = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        targetColor = normalColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        targetColor = clickColor;
    }
}