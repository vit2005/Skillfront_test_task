using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    [SerializeField] private SpritesConfig spritesConfig;
    [SerializeField] private Image image;
    [SerializeField] private Transform forwardPoint;
    [SerializeField] private Transform middlePoint;
    [SerializeField] private float nextDominoParalelOffset;
    [SerializeField] private float nextDominoPerpendicularOffset;

    public bool isThisPerpendicular = false;
    public bool isThisFlipped = false;
    public bool isTurned = false;
    public int[] dots;

    public Image Image => image;

    // Sets the domino to display the correct dots and sprite
    public void SetDomino(int[] dots)
    {
        SetFlippedSprite(dots);
        this.dots = dots;
        gameObject.name += $" [{dots[0]},{dots[1]}]";
    }

    // Rotates the domino by 90 degrees if it is a double
    public void RotateIfDouble(bool isDouble)
    {
        isThisPerpendicular = isDouble;
        image.transform.eulerAngles = isDouble ? new Vector3(0, 0, 90) : Vector3.zero;
    }

    // Sets the sprite based on the dots, flipping it if necessary
    private void SetFlippedSprite(int[] dots)
    {
        SpriteConfig single = spritesConfig.List.FirstOrDefault(x => x.indexes[0] == dots[0] && x.indexes[1] == dots[1]);
        if (single != null)
        {
            image.sprite = single.sprite;
            image.transform.localScale = Vector3.one;
            return;
        }

        // Flip the sprite if no direct match is found
        single = spritesConfig.List.FirstOrDefault(x => x.indexes[0] == dots[1] && x.indexes[1] == dots[0]);
        image.sprite = single.sprite;
        image.transform.localScale = new Vector3(1, -1, 1);
    }

    // Flips the domino upside down
    public void Flip()
    {
        // Flip the y-axis of the image
        image.transform.localScale = new Vector3(1, -image.transform.localScale.y, 1);

        // Reverse the direction of the transform's 'up' vector
        transform.up = -transform.up;

        isThisFlipped = true;
    }

    // Calculates the position for the next domino, considering the direction and offset
    public Vector3 GetNextDominoPosition(out Vector3 direction, bool isParalel = true, bool? isLeftTurn = null, bool isBackward = false)
    {
        // Determine the offset based on whether the domino is placed parallel or perpendicular
        float offset = isParalel ? nextDominoParalelOffset : nextDominoPerpendicularOffset;
        if (isLeftTurn.HasValue && (!isParalel || isThisPerpendicular)) offset += nextDominoParalelOffset - nextDominoPerpendicularOffset;
        // Choose the point to base the positioning on
        Transform point = isThisPerpendicular ? middlePoint : forwardPoint;
        direction = Vector3.zero;

        // Determine the direction based on turn and flip states
        if (isLeftTurn.HasValue)
        {
            //direction = isLeftTurn.Value ? -transform.right : transform.right;
            direction = -transform.right;
            if (isThisFlipped) direction *= -1;
            if (!isLeftTurn.Value && isTurned) direction *= -1;
            return point.position + direction * offset;
        }

        // Calculate the direction based on the backward state
        direction = isBackward ? -transform.up : transform.up;
        if (isThisFlipped) direction *= -1;
        return point.position + direction * offset;
    }
}
