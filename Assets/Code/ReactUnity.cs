using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class ReactUnity
{
  private readonly List<GameObject> renderedElements = new List<GameObject>();

  [UsedImplicitly]
  public Element CreateElement(string type, IDictionary<string, object> attributes, params object[] children)
  {
    List<Element> childrenElements = new List<Element>();

    if (children != null)
    {
      foreach (object child in children)
      {
        if (child is string || child is double || child is bool)
        {
          childrenElements.Add(new Element("p", null, null, child.ToString()));
        }

        if (child is Element)
        {
          Element childElement = (Element) child;

          if (childElement.Type == "span" || childElement.Type == "p")
          {
            childElement.TextValue = childElement.Children[0].TextValue;
          }

          childrenElements.Add((Element) child);
        }
      }
    }

    return new Element(type, childrenElements.ToArray(), attributes == null ? null : new Dictionary<string, object>(attributes));
  }

  [UsedImplicitly]
  public void Render(Element root)
  {
    var a = RenderElement(new Element(root.Type, new Element[0], root.Attributes, root.TextValue), renderParent: true);

    RenderElement(root, a);
  }

  private GameObject RenderElement(Element element, GameObject parent = null, bool renderParent = false)
  {
    List<Element> children = element.Children?.ToList() ?? new List<Element>();
    GameObject elementGameObject = null;

    if (renderParent)
    {
      children.Insert(0, element);
    }

    if (children.Count > 0)
    {
      foreach (Element child in children)
      {
        GameObject renderedElement = null;

        if (parent != null && parent.GetComponent<HorizontalOrVerticalLayoutGroup>() == null)
        {
          if (child.Type == "span")
          {
            HorizontalLayoutGroup layoutGroup = parent.AddComponent<HorizontalLayoutGroup>();

            layoutGroup.childForceExpandWidth = false;
            layoutGroup.childForceExpandHeight = false;
          }
          else
          {
            VerticalLayoutGroup layoutGroup = parent.AddComponent<VerticalLayoutGroup>();

            layoutGroup.childForceExpandHeight = false;
          }
        }

        if (child.TextValue != null)
        {
          renderedElement = ReactUnityRenderer.Instance.RenderText(child, parent);

          if (elementGameObject == null)
          {
            elementGameObject = renderedElement;
          }
        }
        else if (child.Type == "div")
        {
          renderedElement = ReactUnityRenderer.Instance.RenderDiv(child, parent);

          if (elementGameObject == null)
          {
            elementGameObject = renderedElement;
          }

          if (child != element)
          {
            RenderElement(child, renderedElement);
          }
        }
        else
        {
          if (child != element)
          {
            RenderElement(child, parent);
          }
        }

        Debug.Assert(renderedElement != null, "Nothing was rendered");
      }
    }

    return elementGameObject;
  }
}