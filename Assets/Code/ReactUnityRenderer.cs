using UnityEngine;
using UnityEngine.UI;

public class ReactUnityRenderer : MonoBehaviour
{
  public static ReactUnityRenderer Instance
  {
    get
    {
      if (FindObjectOfType<ReactUnityRenderer>() == null)
      {
        instance = new GameObject(nameof(ReactUnityRenderer)).AddComponent<ReactUnityRenderer>();
      }

      return instance;
    }
  }

  private static ReactUnityRenderer instance;

  private Canvas Canvas
  {
    get
    {
      if (canvas == null)
      {
        canvas = new GameObject($"{nameof(ReactUnityRenderer)}Canvas").AddComponent<Canvas>();

        canvas.gameObject.AddComponent<CanvasScaler>();
        canvas.gameObject.AddComponent<CanvasRenderer>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
      }

      return canvas;
    }
  }

  private Canvas canvas;

  private GameObject textPrefab;
  private GameObject divPrefab;

  private int index;

  private void Awake()
  {
    textPrefab = Resources.Load<GameObject>("Text");
    divPrefab = Resources.Load<GameObject>("Div");
  }

  public GameObject RenderText(Element textElement, GameObject parent = null)
  {
    Text text = Instantiate(textPrefab, parent == null ? Canvas.transform : parent.transform).GetComponent<Text>();

    text.text = textElement.TextValue;
    text.gameObject.name += index;

    index++;

    return text.gameObject;
  }

  public GameObject RenderDiv(Element element, GameObject parent = null)
  {
    GameObject div = Instantiate(divPrefab, parent == null ? Canvas.transform : parent.transform);

    div.name += index;

    index++;

    return div;
  }
}