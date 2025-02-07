using Godot;

namespace LittleDragon.scripts.map.grid;

/// <summary>
/// Defines a blueprint for creating rooms within a specific area of the grid.
/// </summary>
public class RoomBuilder
{
    /// <summary>
    /// The minimum boundary of the room's position.
    /// </summary>
    public readonly Vector2I Min;

    /// <summary>
    /// The maximum boundary of the room's position.
    /// </summary>
    public readonly Vector2I Max;

    /// <summary>
    /// The identifier or name of the room.
    /// </summary>
    public readonly string Name;

    /// <summary>
    /// Initializes a room builder with a specified rectangular area.
    /// </summary>
    /// <param name="name">The name or type of the room.</param>
    /// <param name="minX">The minimum X-coordinate of the room.</param>
    /// <param name="maxX">The maximum X-coordinate of the room.</param>
    /// <param name="minY">The minimum Y-coordinate of the room.</param>
    /// <param name="maxY">The maximum Y-coordinate of the room.</param>
    public RoomBuilder(string name, int minX, int maxX, int minY, int maxY)
    {
        Min = new Vector2I(minX, minY);
        Max = new Vector2I(maxX, maxY);
        Name = name;
    }

    /// <summary>
    /// Initializes a room builder for a single tile (1x1 room).
    /// </summary>
    /// <param name="name">The name or type of the room.</param>
    /// <param name="x">The X-coordinate of the room.</param>
    /// <param name="y">The Y-coordinate of the room.</param>
    public RoomBuilder(string name, int x, int y)
    {
        Min = new Vector2I(x, y);
        Max = new Vector2I(x, y);
        Name = name;
    }
}