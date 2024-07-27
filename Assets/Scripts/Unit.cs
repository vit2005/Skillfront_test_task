using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    [SerializeField] private SpritesConfig spritesConfig;
    [SerializeField] private Image image;
    [SerializeField] private Transform forwardParalelPosition;
    [SerializeField] private Transform forwardPerpendicularPosition;
    [SerializeField] private Transform leftParalelPosition;
    [SerializeField] private Transform rightParalelPosition;
    [SerializeField] private Transform leftTurnParalelPosition;
    [SerializeField] private Transform leftTurnPerpendicularPosition;
    [SerializeField] private Transform rightTurnParalelPosition;
    [SerializeField] private Transform rightTurnPerpendicularPosition;

    public bool isThisPerpendicular = false;
    public int[] dots;


    public void SetDomino(int[] dots)
    {
        RotateIfDouble(dots[0] == dots[1]);
        SetFlippedSprite(dots);
        this.dots = dots;
        gameObject.name += $" [{dots[0]},{dots[1]}]";
    }

    private void RotateIfDouble(bool isDouble)
    {
        isThisPerpendicular = isDouble;
        transform.eulerAngles = isDouble ? new Vector3(0, 0, 90) : Vector3.zero;
    }

    private void SetFlippedSprite(int[] dots)
    {
        SpriteConfig single = spritesConfig.List.FirstOrDefault(x => x.indexes[0] == dots[0] && x.indexes[1] == dots[1]);
        if (single != null)
        {
            image.sprite = single.sprite;
            image.transform.localScale = Vector3.one;
            return;
        }

        single = spritesConfig.List.FirstOrDefault(x => x.indexes[0] == dots[1] && x.indexes[1] == dots[0]);
        image.sprite = single.sprite;
        image.transform.localScale = new Vector3(1, -1, 1);
    }

    public void Flip()
    {
        float sign = image.transform.localScale.y;
        sign *= -1;
        image.transform.localScale = new Vector3(1, sign, 1);
        if (isThisPerpendicular)
        {
            Vector3 rotation = transform.eulerAngles;
            rotation.z += 180f;
            transform.eulerAngles = rotation;
        }
    }

    public Transform GetParentTransform(bool isParalel = true, bool? isLeftTurn = null, bool isBackward = false)
    {
        if (isParalel)
        {
            if (isLeftTurn.HasValue) 
                return isLeftTurn.Value ? rightTurnParalelPosition : leftTurnParalelPosition;
            if (isThisPerpendicular)
                return isBackward ? leftParalelPosition : rightParalelPosition;
            
            return forwardParalelPosition;
        }

        return isLeftTurn.HasValue ? 
            isLeftTurn.Value ? rightTurnPerpendicularPosition : leftTurnPerpendicularPosition
            : forwardPerpendicularPosition;

    }
}
