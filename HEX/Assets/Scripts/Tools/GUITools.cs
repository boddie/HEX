using UnityEngine;

/// <summary>
/// GUI tools used for drawing several Unity GUI window objects
/// not featured in their current library such as lines and bordered squares.
/// 
/// NOTE:
/// All calls must be make within the OnGUI() call of a MonoBehaviour class
/// </summary>
public class GUITools
{
	/// <summary>
	/// Draws a non-filled line border of a square
	/// </summary>
	/// <param name='lineTexture'>
	/// Texture for the line border
	/// </param>
	/// <param name='rect'>
	/// Location to draw and size of square to draw
	/// </param>
	/// <param name='lineWidth'>
	/// Line width of the border
	/// </param>
	public static void emptySquare(Texture2D lineTexture, Rect rect, float lineWidth)
	{
		// draw lines
		horizontalLine(lineTexture, new Vector2(rect.x, rect.y), rect.width + lineWidth, lineWidth);
		horizontalLine(lineTexture, new Vector2(rect.x, rect.y + rect.height), rect.width + lineWidth, lineWidth);
		verticalLine(lineTexture, new Vector2(rect.x, rect.y), rect.height + lineWidth, lineWidth);
		verticalLine(lineTexture, new Vector2(rect.x + rect.width, rect.y), rect.height + lineWidth, lineWidth);
	}
	
	/// <summary>
	/// Draws a filled square
	/// </summary>
	/// <param name='lineTexture'>
	/// Texture of line border of square
	/// </param>
	/// <param name='fillTexture'>
	/// Texture to fill square with
	/// </param>
	/// <param name='rect'>
	/// Size and location of square
	/// </param>
	/// <param name='lineWidth'>
	/// Line width for square border
	/// </param>
	public static void filledSquare(Texture2D lineTexture, Texture2D fillTexture, Rect rect, float lineWidth)
	{
        if (lineTexture == null) Debug.Log("Linetexture null");
        if (fillTexture == null) Debug.Log("Filletexture null");
        if (rect == null) Debug.Log("Rect null");

		// Draw fill
		GUI.DrawTexture(rect, fillTexture, ScaleMode.StretchToFill);
		
		// draw lines
		horizontalLine(lineTexture, new Vector2(rect.x, rect.y), rect.width + lineWidth, lineWidth);
		horizontalLine(lineTexture, new Vector2(rect.x, rect.y + rect.height), rect.width + lineWidth, lineWidth);
		verticalLine(lineTexture, new Vector2(rect.x, rect.y), rect.height + lineWidth, lineWidth);
		verticalLine(lineTexture, new Vector2(rect.x + rect.width, rect.y), rect.height + lineWidth, lineWidth);
	}
	
	/// <summary>
	/// Draws a filled and bordered square with text at its center
	/// </summary>
	/// <param name='lineTexture'>
	/// Texture for square border.
	/// </param>
	/// <param name='fillTexture'>
	/// Texture to fill square with.
	/// </param>
	/// <param name='location'>
	/// Top-left location to draw the square
	/// </param>
	/// <param name='height'>
	/// Height of the square
	/// </param>
	/// <param name='lineWidth'>
	/// Line width of the border
	/// </param>
	/// <param name='text'>
	/// Text to put in the square
	/// </param>
	public static void filledTextSquare(Texture lineTexture, Texture fillTexture, Vector2 location, float height, float lineWidth, string text)
	{
		// Get size of text
		Vector2 textSize = GUI.skin.GetStyle("Label").CalcSize(new GUIContent(text));
		
		// Calculate square rect
		float padding = (height - textSize.y) / 2;
		Rect rect = new Rect(location.x, location.y, padding*2 + textSize.x, height);
		
		// Caculate rect for text
		Rect textRect = new Rect(location.x + padding, location.y + padding, textSize.x, textSize.y);
		
		// draw filling and text
		GUI.DrawTexture(rect, fillTexture, ScaleMode.StretchToFill);
		Color current = GUI.color;
		GUI.color = Color.black;
		GUI.Label(textRect, text);
		GUI.color = current;
		
		// draw lines
		horizontalLine(lineTexture, new Vector2(rect.x, rect.y), rect.width + lineWidth, lineWidth);
		horizontalLine(lineTexture, new Vector2(rect.x, rect.y + rect.height), rect.width + lineWidth, lineWidth);
		verticalLine(lineTexture, new Vector2(rect.x, rect.y), rect.height + lineWidth, lineWidth);
		verticalLine(lineTexture, new Vector2(rect.x + rect.width, rect.y), rect.height + lineWidth, lineWidth);
	}
	
	
	public static void filledTextSquare(Texture lineTexture, Texture fillTexture, Rect rect, float lineWidth, string text)
	{
		// Get size of text
		Vector2 textSize = GUI.skin.GetStyle("Label").CalcSize(new GUIContent(text));
		
		// Caculate rect for text
		Rect textRect = new Rect(rect.x + (rect.width/2) - (textSize.x/2), 
			rect.y + (rect.height/2) - (textSize.y/2), textSize.x, textSize.y);
		
		// draw filling and text
		GUI.DrawTexture(rect, fillTexture, ScaleMode.StretchToFill);
		Color current = GUI.color;
		GUI.color = Color.black;
		GUI.Label(textRect, text);
		GUI.color = current;
		
		// draw lines
		horizontalLine(lineTexture, new Vector2(rect.x, rect.y), rect.width + lineWidth, lineWidth);
		horizontalLine(lineTexture, new Vector2(rect.x, rect.y + rect.height), rect.width + lineWidth, lineWidth);
		verticalLine(lineTexture, new Vector2(rect.x, rect.y), rect.height + lineWidth, lineWidth);
		verticalLine(lineTexture, new Vector2(rect.x + rect.width, rect.y), rect.height + lineWidth, lineWidth);
	}
	
	/// <summary>
	/// Draws a grid
	/// </summary>
	/// <param name='texture'>
	/// Texture of the borders and lines of grid
	/// </param>
	/// <param name='boundary'>
	/// Overall size of the grid
	/// </param>
	/// <param name='unitLength'>
	/// Length of a unit with grid. All grid elements are unit length and width
	/// </param>
	/// <param name='lineWidth'>
	/// Width of lines and borders of the grid
	/// </param>
	public static void grid(Texture texture, Rect boundary, float unitLength, float lineWidth)
	{
		// draw horizontal lines
		for(float i = 0, j = 0; i <= boundary.width / unitLength; i++, j += unitLength)
		{
			horizontalLine(texture, new Vector2(boundary.x, boundary.y + j), boundary.width + lineWidth, lineWidth);
		}
		
		// draw vertical lines
		for(float i = 0, j = 0; i <= boundary.height / unitLength; i++, j += unitLength)
		{
			verticalLine(texture, new Vector2(boundary.x + j, boundary.y), boundary.height + lineWidth, lineWidth);
		}
	}
	
	/// <summary>
	/// Draws a horizontal line
	/// </summary>
	/// <param name='texture'>
	/// The texture of the line
	/// </param>
	/// <param name='startPos'>
	/// Location the line should start at
	/// </param>
	/// <param name='length'>
	/// Length of the line
	/// </param>
	/// <param name='width'>
	/// Width of the line
	/// </param>
	public static void horizontalLine(Texture texture, Vector2 startPos, float length, float width)
	{
		GUI.DrawTexture(new Rect(startPos.x, startPos.y, length, width), texture, ScaleMode.StretchToFill);
	}
	
	/// <summary>
	/// Draws a vertical line
	/// </summary>
	/// <param name='texture'>
	/// Texture for the line
	/// </param>
	/// <param name='startPos'>
	/// Start position of the line
	/// </param>
	/// <param name='length'>
	/// Length of the line
	/// </param>
	/// <param name='width'>
	/// Width of the line
	/// </param>
	public static void verticalLine(Texture texture, Vector2 startPos, float length, float width)
	{
		GUI.DrawTexture(new Rect(startPos.x, startPos.y, width, length), texture, ScaleMode.StretchToFill);
	}
	
	/// <summary>
	/// Draws a progress bar
	/// </summary>
	/// <param name='foreground'>
	/// Foreground texture
	/// </param>
	/// <param name='background'>
	/// Background texture
	/// </param>
	/// <param name='line'>
	/// Texture to draw line with
	/// </param>
	/// <param name='lineWidth'>
	/// With of inner and outer line
	/// </param>
	/// <param name='location'>
	/// Rectangular location for the progress bar to be displayed
	/// </param>
	/// <param name='progress'>
	/// Percent of inner rect to spawn within the outer rect for progress bar representation.
	/// </param>
	public static void progressBar(Texture2D foreground, Texture2D background, Texture2D line, float lineWidth, Rect location, float progress)
	{
        if (foreground == null) Debug.Log("Foreground null");
        if (background == null) Debug.Log("Background null");
        if (line == null) Debug.Log("Line null");
        if (location == null) Debug.Log("Location null");
      //  Debug.Log("Progress : " + progress);

		filledSquare((Texture2D)line, (Texture2D)background, location, lineWidth);
		filledSquare((Texture2D)line, (Texture2D)foreground, new Rect(location.x, location.y, location.width * progress, location.height), lineWidth);
	}
}
