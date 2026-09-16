using UnityEngine;

[System.Serializable]
public class LevelData
{
    public PlatformData[] platforms;
    public CoinData[] coins;
}

[System.Serializable]
public class PlatformData
{
    public float x;
    public float y;
    public float w;
    public float h;
    public bool moving = false;
    public float ax; // pointA x
    public float ay; // pointA y
    public float bx; // pointB x
    public float by; // pointB y
    public float speed = 2f;
}

[System.Serializable]
public class CoinData
{
    public float x;
    public float y;
    public int value = 10;
}

public class LevelLoader : MonoBehaviour
{
    public TextAsset levelJson; // 把 assets/Resources/level1.json 或直接拖 TextAsset
    public GameObject platformPrefab;       // prefab: has BoxCollider2D, optionally MovingPlatform + bridge
    public GameObject movingPlatformPrefab; // prefab: has MovingPlatform, BoxCollider2D, PlatformColliderBridge
    public GameObject coinPrefab;

    public float pixelsPerUnit = 100f; // 如果你的数据是像素坐标（来自 Pygame），用此值转换

    void Start()
    {
        if (levelJson == null)
        {
            Debug.LogWarning("No levelJson assigned");
            return;
        }

        LevelData data = JsonUtility.FromJson<LevelData>(levelJson.text);
        if (data == null) return;

        // create platforms
        if (data.platforms != null)
        {
            foreach (var p in data.platforms)
            {
                if (p.moving)
                {
                    var go = Instantiate(movingPlatformPrefab);
                    // interpret incoming coords as Unity units OR pixels depending on your JSON
                    Vector2 a = new Vector2(p.ax, p.ay) / pixelsPerUnit;
                    Vector2 b = new Vector2(p.bx, p.by) / pixelsPerUnit;
                    go.transform.position = a;
                    var mp = go.GetComponent<MovingPlatform>();
                    mp.pointA = a;
                    mp.pointB = b;
                    mp.speed = p.speed;
                    // scale platform collider / sprite
                    go.transform.localScale = new Vector3(p.w / pixelsPerUnit, p.h / pixelsPerUnit, 1f);
                }
                else
                {
                    var go = Instantiate(platformPrefab);
                    go.transform.position = new Vector3(p.x / pixelsPerUnit, p.y / pixelsPerUnit, 0f);
                    go.transform.localScale = new Vector3(p.w / pixelsPerUnit, p.h / pixelsPerUnit, 1f);
                }
            }
        }

        // create coins
        if (data.coins != null)
        {
            foreach (var c in data.coins)
            {
                var go = Instantiate(coinPrefab);
                go.transform.position = new Vector3(c.x / pixelsPerUnit, c.y / pixelsPerUnit, 0f);
            }
        }
    }
}
