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

    [Header("Title Texts")]
    public TextMeshProUGUI subtitleText;
    public TextMeshProUGUI titleText;

    [Header("Hover Settings")]
    public float hoverScale = 1.05f;
    public float transitionSpeed = 8f;

    // Colors
    private Color normalBgColor     = new Color(0f, 0f, 0f, 0f);
    private Color hoverBgColor      = new Color(0.6f, 0.05f, 0.05f, 0.4f);
    private Color clickBgColor      = new Color(0.8f, 0.05f, 0.05f, 0.6f);
    private Color normalTextColor   = new Color(1f, 1f, 1f, 0.85f);
    private Color hoverTextColor    = new Color(1f, 0.3f, 0.3f, 1f);
    private Color borderNormalColor = new Color(0.6f, 0.05f, 0.05f, 0.8f);
    private Color borderHoverColor  = new Color(1f, 0.2f, 0.2f, 1f);

    void Start()
    {
        StyleTitleTexts();
        SetupButton(playButton,     "PLAY ↗");
        SetupButton(settingsButton, "SETTINGS ↗");
        SetupButton(creditsButton,  "CREDITS ↗");
        SetupButton(quitButton,     "QUIT ↗");

        playButton.onClick.AddListener(PlayGame);
        settingsButton.onClick.AddListener(OpenSettings);
        creditsButton.onClick.AddListener(OpenCredits);
        quitButton.onClick.AddListener(QuitGame);

            // Show cursor on Main Menu
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;

    // ... rest of your Start() code
    }

    // ---- TITLE STYLING ----
    void StyleTitleTexts()
    {
        if (subtitleText != null)
        {
            subtitleText.color          = new Color(1f, 1f, 1f, 0.5f);
            subtitleText.fontSize       = 14f;
            subtitleText.characterSpacing = 6f;
            subtitleText.fontStyle      = FontStyles.UpperCase;
        }

        if (titleText != null)
        {
            titleText.color    = new Color(0.85f, 0.15f, 0.1f, 1f);
            titleText.fontSize = 72f;
            titleText.fontStyle = FontStyles.Bold;
        }
    }

    // ---- BUTTON SETUP ----
    void SetupButton(Button btn, string label)
    {
        if (btn == null) return;

        // Remove default button transition
        btn.transition = Selectable.Transition.None;

        // Style button image (background)
        Image btnImage = btn.GetComponent<Image>();
        if (btnImage != null)
        {
            btnImage.color = normalBgColor;
        }

        // Add red border image
        GameObject borderObj = new GameObject("Border");
        borderObj.transform.SetParent(btn.transform, false);
        borderObj.transform.SetAsFirstSibling();

        RectTransform borderRect = borderObj.AddComponent<RectTransform>();
        borderRect.anchorMin = Vector2.zero;
        borderRect.anchorMax = Vector2.one;
        borderRect.offsetMin = Vector2.zero;
        borderRect.offsetMax = Vector2.zero;

        Image borderImage = borderObj.AddComponent<Image>();
        borderImage.color = borderNormalColor;

        // Add inner fill on top of border
        GameObject fillObj = new GameObject("Fill");
        fillObj.transform.SetParent(btn.transform, false);
        fillObj.transform.SetSiblingIndex(1);

        RectTransform fillRect = fillObj.AddComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.offsetMin = new Vector2(2, 2);
        fillRect.offsetMax = new Vector2(-2, -2);

        Image fillImage = fillObj.AddComponent<Image>();
        fillImage.color = new Color(0f, 0f, 0f, 0.85f);

        // Style button text
        TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
        if (btnText != null)
        {
            btnText.text             = label;
            btnText.color            = normalTextColor;
            btnText.fontSize         = 18f;
            btnText.fontStyle        = FontStyles.Bold;
            btnText.characterSpacing = 4f;
            btnText.alignment        = TextAlignmentOptions.Center;
        }

        // Add hover handler
        ButtonHoverHandler hover = btn.gameObject.AddComponent<ButtonHoverHandler>();
        hover.normalBgColor     = normalBgColor;
        hover.hoverBgColor      = hoverBgColor;
        hover.clickBgColor      = clickBgColor;
        hover.normalTextColor   = normalTextColor;
        hover.hoverTextColor    = hoverTextColor;
        hover.borderNormalColor = borderNormalColor;
        hover.borderHoverColor  = borderHoverColor;
        hover.hoverScale        = hoverScale;
        hover.transitionSpeed   = transitionSpeed;
        hover.borderImage       = borderImage;
        hover.fillImage         = fillImage;
    }

    // ---- MENU FUNCTIONS ----
    public void PlayGame()
    {
            // Hide cursor when entering game
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
        SceneManager.LoadScene("terrain2");
    }

    public void OpenSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    public void OpenCredits()
    {
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit!");
    }
}

// ---- HOVER HANDLER ----
public class ButtonHoverHandler : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [HideInInspector] public Color normalBgColor;
    [HideInInspector] public Color hoverBgColor;
    [HideInInspector] public Color clickBgColor;
    [HideInInspector] public Color normalTextColor;
    [HideInInspector] public Color hoverTextColor;
    [HideInInspector] public Color borderNormalColor;
    [HideInInspector] public Color borderHoverColor;
    [HideInInspector] public float hoverScale = 1.05f;
    [HideInInspector] public float transitionSpeed = 8f;
    [HideInInspector] public Image borderImage;
    [HideInInspector] public Image fillImage;

    private Image buttonImage;
    private TextMeshProUGUI buttonText;
    private Vector3 originalScale;
    private Color targetBgColor;
    private Color targetBorderColor;
    private bool isHovering = false;

    void Start()
    {
        buttonImage   = GetComponent<Image>();
        buttonText    = GetComponentInChildren<TextMeshProUGUI>();
        originalScale = transform.localScale;
        targetBgColor     = normalBgColor;
        targetBorderColor = borderNormalColor;
    }

    void Update()
    {
        // Smooth background color
        if (buttonImage != null)
            buttonImage.color = Color.Lerp(
                buttonImage.color, targetBgColor,
                Time.deltaTime * transitionSpeed);

        // Smooth border color
        if (borderImage != null)
            borderImage.color = Color.Lerp(
                borderImage.color, targetBorderColor,
                Time.deltaTime * transitionSpeed);

        // Smooth scale
        Vector3 targetScale = isHovering ?
            originalScale * hoverScale : originalScale;
        transform.localScale = Vector3.Lerp(
            transform.localScale, targetScale,
            Time.deltaTime * transitionSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering        = true;
        targetBgColor     = hoverBgColor;
        targetBorderColor = borderHoverColor;
        if (buttonText != null)
            buttonText.color = hoverTextColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering        = false;
        targetBgColor     = normalBgColor;
        targetBorderColor = borderNormalColor;
        if (buttonText != null)
            buttonText.color = normalTextColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        targetBgColor = clickBgColor;
    }
}