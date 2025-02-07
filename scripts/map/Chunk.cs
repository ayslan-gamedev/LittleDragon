using Godot;

namespace LittleDragon.scripts.map;

/// <summary>
/// Represents a room in the game, which can have up to four neighboring rooms.
/// </summary>
public partial class Chunk : Node
{
    /// <summary>
    /// References to the doors or pathways leading to adjacent rooms.
    /// </summary>
    [Export] private Node2D _left, _up, _right, _down;

    /// <summary>
    /// Properties for accessing the adjacent room nodes.
    /// </summary>
    private Node2D Left => _left;
    private Node2D Up => _up;
    private Node2D Right => _right;
    private Node2D Down => _down;

    /// <summary>
    /// Stores references to neighboring Room instances.
    /// </summary>
    public Chunk[] NeighborRooms = new Chunk[4];
    
    /// <summary>
    /// Assigns a neighboring room in a specific direction.
    /// </summary>
    /// <param name="io">Direction index (0: Left, 1: Up, 2: Right, 3: Down).</param>
    /// <param name="chunk">The room to set as a neighbor.</param>
    public void SetNeighbor(int io, Chunk chunk)
    {
        NeighborRooms[io] = chunk;
    }

    /// <summary>
    /// Binary representation of the room's open sides.
    /// </summary>
    private int RoomInputs { get; set; }

    /// <summary>
    /// Called when the node enters the scene tree. Initializes room connections.
    /// </summary>
    public override void _Ready()
    {
        if (_left != null) RoomInputs |= 0b1000;  // Left door open
        if (_up != null) RoomInputs |= 0b0100;    // Up door open
        if (_right != null) RoomInputs |= 0b0010; // Right door open
        if (_down != null) RoomInputs |= 0b0001;  // Down door open
    }

    /// <summary>
    /// Returns a binary string representation of the room's connections.
    /// </summary>
    /// <returns>A 4-bit string indicating open paths (Left-Up-Right-Down).</returns>
    public override string ToString()
    {
        return $"{(Left != null ? "1" : "0")}{(Up != null ? "1" : "0")}" +
               $"{(Right != null ? "1" : "0")}{(Down != null ? "1" : "0")}";
    }
}
