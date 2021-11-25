using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public static float MaxSize = 20;
    public Rect Rectangle { get; set; }

    public Block(Rect rectangle)
    {
        this.Rectangle = rectangle;
    }

    // Function that splits the rectangle and returns two smaller rectangles
    public Block[] Grow()
    {
        // declare two smaller blocks
        Block[] result = new Block[2];
        Rect rectangle1, rectangle2;

        // randomly split the rectangle
        float split;

        if (Rectangle.width > Rectangle.height)
        {
            // split the rectangle vertically
            split = Random.Range(MaxSize * 0.5f, Rectangle.width - MaxSize * 0.5f);

            // configure the new rectangles
            rectangle1 = new Rect(Rectangle.x, Rectangle.y, split, Rectangle.height);
            rectangle2 = new Rect(Rectangle.x + split, Rectangle.y, Rectangle.width - split, Rectangle.height);
        }
        else
        {
            // split the rectangle horizontally
            split = Random.Range(MaxSize * 0.5f, Rectangle.height - MaxSize * 0.5f);

            // configure the new rectangles
            rectangle1 = new Rect(Rectangle.x, Rectangle.y, Rectangle.width, split);
            rectangle2 = new Rect(Rectangle.x, Rectangle.y + split, Rectangle.width, Rectangle.height - split);
        }
        result[0] = new Block(rectangle1);
        result[1] = new Block(rectangle2);

        return result;
    }

    // Check if the block lies within another block
    public bool IsIn(Block other)
    {
        return other.Rectangle.Contains(Rectangle.position) && other.Rectangle.Contains(Rectangle.position + Rectangle.size);
    }
}
