using System.Collections.Generic;

public class Element
{
  public string Type { get; }
  public Element[] Children { get; }
  public Dictionary<string, object> Attributes { get; }
  public string TextValue { get; set; }

  public Element(string type, Element[] children, Dictionary<string, object> attributes, string textValue = null)
  {
    Type = type;
    Children = children;
    Attributes = attributes;
    TextValue = textValue;
  }
}