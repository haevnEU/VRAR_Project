using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class MovableGem : MonoBehaviour
{
    public GameRenderer gameRenderer;
    public int xPos;
    public int yPos;

    private Vector3 rootPosition;

    private bool isHighlighted;
    private bool isMoving;

    public int GemType;

    bool canMoveToDirection(MoveDirection direction)
    {
        switch (direction)
        {
            case MoveDirection.Left: //left
                return xPos > 0;
            case MoveDirection.Right: //right
                return xPos < gameRenderer.fieldSize - 1;
            case MoveDirection.Up: //up
                return yPos < gameRenderer.fieldSize - 1;
            case MoveDirection.Down: //down
                return yPos > 0;
        }
        return false;
    }

    public void SetHighlighted(bool highlighted)
    {
        GameObject movingGem = gameRenderer.MovingGem;
        GameObject highlightedGem = gameRenderer.HighlightedGem;

        if ((highlightedGem || movingGem) && highlighted)
        {
            return;
        }

        isHighlighted = highlighted;

        if (highlighted)
        {
            gameRenderer.HighlightedGem = gameObject;
        }
        else if (highlightedGem == gameObject)
        {
            gameRenderer.HighlightedGem = null;
        }
    }

    public void SetMoving(bool moving)
    {
        GameObject movingGem = gameRenderer.MovingGem;

        if (movingGem && moving)
        {
            return;
        }

        isMoving = moving;

        if (moving)
        {
            gameRenderer.MovingGem = gameObject;
        }
        else if (movingGem == gameObject)
        {
            gameRenderer.MovingGem = null;
        }

        if (moving)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.rootPosition.z + 0.01f);
        }
        else
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.rootPosition.z);
        }
    }

    public void handleRelease()
    {
        Vector3 posRelativeToRoot = transform.position - rootPosition;
        var neededDistanceForMoveX = this.gameRenderer.GemOffset + (gameObject.transform.lossyScale.x / 2);
        var neededDistanceForMoveY = this.gameRenderer.GemOffset + (gameObject.transform.lossyScale.y / 2);

        if (posRelativeToRoot.x > neededDistanceForMoveX)
        {
            // move right
            gameRenderer.handleMove(this, MoveDirection.Right);
        }
        else if (posRelativeToRoot.x < -neededDistanceForMoveX)
        {
            // move left
            gameRenderer.handleMove(this, MoveDirection.Left);
        }
        else if (posRelativeToRoot.y < -neededDistanceForMoveY)
        {
            // move up
            gameRenderer.handleMove(this, MoveDirection.Up);
        }
        else if (posRelativeToRoot.y > neededDistanceForMoveY)
        {
            // move down
            gameRenderer.handleMove(this, MoveDirection.Down);
        }

        // snap back to root position
        gameObject.transform.position = rootPosition;
    }

    public void handleMove(Vector3 newPos)
    {
        // prevent move from edge gems off the plane (horizontal)
        if (newPos.x < rootPosition.x) // LEFT
        {
            if (!canMoveToDirection(MoveDirection.Left))
            {
                newPos.x = rootPosition.x;
            }
        }
        else if (newPos.x > rootPosition.x) // RIGHT
        {
            if (!canMoveToDirection(MoveDirection.Right))
            {
                newPos.x = rootPosition.x;
            }
        }

        // prevent move from edge gems off the plane (vertical)
        if (newPos.y > rootPosition.y) // UP
        {
            if (!canMoveToDirection(MoveDirection.Up))
            {
                newPos.y = rootPosition.y;
            }
        }
        else if (newPos.y < rootPosition.y) // DOWN
        {
            if (!canMoveToDirection(MoveDirection.Down))
            {
                newPos.y = rootPosition.y;
            }
        }

        Vector3 newPosRelativeToRoot = newPos - rootPosition;
        float scaleToUse = 0;

        // Only move on horizontal or vertical axis depending on what is further away
        if (Mathf.Abs(newPosRelativeToRoot.x) > Mathf.Abs(newPosRelativeToRoot.y))
        {
            newPos.y = rootPosition.y;
            scaleToUse = gameObject.transform.localScale.x * gameRenderer.transform.lossyScale.x;
        }
        else
        {
            newPos.x = rootPosition.x;
            scaleToUse = gameObject.transform.localScale.y * gameRenderer.transform.lossyScale.y;
        }

        // restrict to moving over neighbor gems
        var maxRange = this.gameRenderer.GemOffset + scaleToUse;
        if (newPosRelativeToRoot.x > maxRange)
        {
            newPos.x = rootPosition.x + maxRange;
        }
        if (newPosRelativeToRoot.x < -maxRange)
        {
            newPos.x = rootPosition.x - maxRange;
        }
        if (newPosRelativeToRoot.y > maxRange)
        {
            newPos.y = rootPosition.y + maxRange;
        }
        if (newPosRelativeToRoot.y < -maxRange)
        {
            newPos.y = rootPosition.y - maxRange;
        }

        transform.position = newPos;
    }

    // Start is called before the first frame update
    void Start()
    {
        rootPosition = transform.position;

        InteractableGem si = gameObject.AddComponent<InteractableGem>();
        si.setMovableGemScript(this);
        si.setGameRenderer(gameRenderer);
    }
}
