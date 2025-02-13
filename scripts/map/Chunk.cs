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
    private Node2D _left, _up, _right, _down;

    /// <summary>
    /// Stores references to neighboring Room instances.
    /// </summary>
    public Chunk[] NeighborRooms = new Chunk[4];

    private Area2D _area2D;
    
    /// <summary>
    /// Assigns a neighboring room in a specific direction.
    /// </summary>
    /// <param name="io">Direction index (0: Left, 1: Up, 2: Right, 3: Down).</param>
    /// <param name="chunk">The room to set as a neighbor.</param>
    public void SetNeighbor(int io, Chunk chunk)
    {
        NeighborRooms[io] = chunk;
        GD.Print($"sated {chunk}");
    }

    public void ActiveNeighbor()
    {
        NeighborRooms[0].GetOwner<Node2D>().Visible = true;

        var distance = NeighborRooms[0]._right.GlobalPosition - _left.GlobalPosition;
        NeighborRooms[0].GetOwner<Node2D>().GlobalPosition += distance;
        GD.Print(distance);
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
        // get exits (case exist)
        _left = GetNodeOrNull<Node2D>("left");
        _up = GetNodeOrNull<Node2D>("up");
        _right = GetNodeOrNull<Node2D>("right");
        _down = GetNodeOrNull<Node2D>("down");
        
        // set binary chunk value
        if (_left != null) RoomInputs |= 0b1000;
        if (_up != null) RoomInputs |= 0b0100;
        if (_right != null) RoomInputs |= 0b0010;
        if (_down != null) RoomInputs |= 0b0001;

        // connect ObBodyEntered from Area2D
        _area2D = Owner.GetNode<Area2D>("Area2D");
        _area2D.BodyEntered += OnBodyEntered;
    }

    private static void OnBodyEntered(Node2D body)
    {
        //GD.Print("CURRENT");
    }
    
    /// <summary>
    /// Returns a binary string representation of the room's connections.
    /// </summary>
    /// <returns>A 4-bit string indicating open paths (Left-Up-Right-Down).</returns>
    public override string ToString()
    {
        return $"{(_left != null ? "1" : "0")}{(_up != null ? "1" : "0")}" +
               $"{(_right != null ? "1" : "0")}{(_down != null ? "1" : "0")}";
    }
}
