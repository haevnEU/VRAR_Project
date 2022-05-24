using Bejeweled;
using System.Collections.Generic;
using UnityEngine;

public class GameRenderer : MonoBehaviour, IRender
{
    [SerializeField] public int fieldSize;
    [SerializeField] private GameObject[] prefabsForGemTypes;
    [SerializeField] public TMPro.TMP_Text televisonScreenText;
    [SerializeField] public TMPro.TMP_Text highscoreText;
    private GameObject[,] field;
    private float gemOffset;
    private GameObject movingGem;
    private GameObject highlightedGem;
    private Bejeweled.Bejeweled gameLogic;
    private List<int> highscores = new List<int>();
    public GameObject MovingGem { get { return movingGem; } set { movingGem = value; } }
    public GameObject HighlightedGem { get { return highlightedGem; } set { highlightedGem = value; } }
    public float GemOffset { get { return gemOffset; } }
    private bool gameOver = false;

    Vector3 backgroundSize()
    {
        var meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.mesh;
        return mesh.bounds.size;
    }

    Vector3 scaleForField()
    {
        float allOffsets = gemOffset * (fieldSize + 1);

        Vector3 bgSize = backgroundSize();

        float xSize = ((bgSize.x - allOffsets) / fieldSize);
        float zSize = ((bgSize.z - allOffsets) / fieldSize);

        return new Vector3(xSize, 1, zSize);
    }

    public void calcScaleAndPos(int x, int y, out Vector3 scale, out Vector3 pos)
    {
        scale = scaleForField();

        Vector3 bgSize = backgroundSize();

        Vector3 topLeft = new Vector3(-bgSize.x / 2, 0, bgSize.z / 2);
        pos = new Vector3(topLeft.x + gemOffset * (x + 1)+ scale.x * (x + 0.5f), 0.5f, topLeft.z - gemOffset * (y + 1) - scale.z * (y + 0.5f));
    }

    void resetField()
    {
        if (field != null)
        {
            for (int x = 0; x < fieldSize; x++)
            {
                for (int y = 0; y < fieldSize; y++)
                {
                    if (field[x,y] != null)
                    {
                        Destroy(field[x, y]);
                    }
                }
            }
        }

        field = new GameObject[fieldSize, fieldSize];
    }

    GameObject getGem(int x, int y)
    {
        return field[x, y];
    }

    void setGem(int x, int y, int gemType)
    {
        // If gem is already placed, skip
        GameObject existingGem = getGem(x, y);
        if (existingGem != null)
        {
            MovableGem mj = existingGem.GetComponent<MovableGem>();
            if (mj)
            {
                if (mj.GemType == gemType)
                {
                    return;
                }
            }
        }

        // If a gem already exists at the position, destroy it
        if (existingGem != null)
        {
            if(MovingGem == existingGem)
            {
                MovingGem = null;
            }
            if(HighlightedGem == existingGem)
            {
                HighlightedGem = null;
            }
            Destroy(existingGem);
        }

        // Place new gem at position
        GameObject prefab = prefabsForGemTypes[gemType - 1];
        GameObject obj = GameObject.Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        obj.transform.parent = gameObject.transform;

        var gemLogic = obj.AddComponent<MovableGem>();
        gemLogic.xPos = x;
        gemLogic.yPos = y;
        gemLogic.GemType = gemType;
        gemLogic.gameRenderer = this;

        Vector3 pos, scale;
        calcScaleAndPos(x, y, out scale, out pos);

        obj.transform.localPosition = pos;
        obj.transform.localScale = scale;
        obj.transform.localRotation = Quaternion.identity;

        obj.AddComponent<BoxCollider>();

        field[x, y] = obj;

        Debug.Log("placed new: " + obj);
    }

    public void render(Bejeweled.Bejeweled board)
    {
        for(int x = 0; x < fieldSize; x++)
        {
            for (int y = 0; y < fieldSize; y++)
            {
                int cellToRender = board.Board.getCell(x, y);
                setGem(x, y, cellToRender);
            }
        }

        if(board.isGameOver())
        {
            gameOver = true;
            updateScreens();
        }
    }

    public void handleMove(MovableGem originGem, MoveDirection dir)
    {
        int targetX = 0, targetY = 0;

        switch(dir)
        {
            case MoveDirection.Up:
                targetX = originGem.xPos;
                targetY = originGem.yPos - 1;
                break;
            case MoveDirection.Down:
                targetX = originGem.xPos;
                targetY = originGem.yPos + 1;
                break;
            case MoveDirection.Left:
                targetY = originGem.yPos;
                targetX = originGem.xPos - 1;
                break;
            case MoveDirection.Right:
                targetY = originGem.yPos;
                targetX = originGem.xPos + 1;
                break;
        }

        gameLogic.swap(originGem.xPos, originGem.yPos, targetX, targetY);
        updateScreens();
    }

    public void updateGameScreen(string firstLine, string secondLine)
    {
        if (televisonScreenText != null)
        {
            televisonScreenText.text = "Bejeweled\n" + firstLine + "\n" + secondLine;
        }
    }
    public void updateHighscoreScreen()
    {

        highscores.Sort();
        highscores.Reverse();

        if (highscoreText != null)
        {
            string text = "";
            text += "1)..." + highscores[0] + "\n";
            text += "2)..." + highscores[1] + "\n";
            text += "3)..." + highscores[2];
            highscoreText.text = text;
        }
    }

    public void updateScreens()
    {
        updateHighscoreScreen();
        if (gameOver)
        {
            updateGameScreen("GAME OVER", "Points: " + gameLogic.Points);
        }
        else
        {
            updateGameScreen("Points: " + gameLogic.Points, "");
        }
    }

    public void restart()
    {
        highscores.Add(gameLogic.Points);
        StartGame();
        gameLogic.Points = 0;
        updateScreens();
    }

    // Start is called before the first frame update
    void Start()
    {
        gemOffset = 0.2f;
        StartGame();
    }
    void StartGame()
    {
        gameOver = false;
        resetField();
        gameLogic = new Bejeweled.Bejeweled(fieldSize, prefabsForGemTypes.Length + 1, this);
        highscores.Add(2);
        highscores.Add(5);
        highscores.Add(1);
        highscores.Add(0);
    }
}
