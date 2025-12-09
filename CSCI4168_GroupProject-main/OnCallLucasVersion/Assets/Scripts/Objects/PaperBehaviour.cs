using UnityEngine;

public class PaperBehaviour : MonoBehaviour
{
    public Sprite GetPaperSprite()
    {
        var renderer = GetComponent<Renderer>();
        var mat = renderer.material;

        Texture2D tex = mat.mainTexture as Texture2D;
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }
}
