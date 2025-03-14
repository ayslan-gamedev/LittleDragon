using System.Linq;
using Godot;
using Godot.Collections;
using LittleDragon.scripts.map.grid;

namespace LittleDragon.scripts.map;

public partial class Test : Node2D
{
    [Export] private Array<RoomBuilder> _rooms;
    [Export] private Array<Chunk> _resources;
    
    public override void _Ready()
    {
        Generate();
    }

    private void Generate()
    {
        const int width = 5;
        const int height = 5; 
        
        var (grid, dots) = GridFactory.CreateGrid(width, height, _rooms.ToArray());
        GridFactory.Connect(grid,dots[0], dots[1]);
        
        var rooms = ChunkManager.InstantiateGrid(this, grid);

        ChunkManager.Chunks = rooms;
        ChunkManager.UpdateChunks(rooms[0]);
        rooms[0].GetOwner<Node2D>().Position = Vector2.Zero;
    }
}