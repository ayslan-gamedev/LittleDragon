using Godot;
using LittleDragon.scripts.map;
using LittleDragon.scripts.map.grid;

namespace LittleDragon.scripts;

public partial class Test : Node2D
{
    [Export] private Node2D _node2D;
    
    public override void _Ready()
    {
        ChunkManager.Test_InstantiateRoom(_node2D);
        TestGen();
    }

    private void TestGen()
    {
        const int width = 15;
        const int height = 15;
		
        var chunkBuilders = new RoomBuilder[]
        {
            new("A", 0, 0 ),
            new("B", 2, 2 ),
            new("C", 2, 3 ),
            new("D", 4, 2 ),
        };
        
        var (grid, dots) = GridFactory.CreateGrid(width, height, chunkBuilders);
        GridFactory.Connect(grid,dots[0], dots[1]);
        GridFactory.Connect(grid,dots[1], dots[2]);
        GridFactory.Connect(grid,dots[0], dots[3]);
        
        var rooms = ChunkManager.InstantiateGrid(_node2D, grid);
        
        ChunkManager.UpdateChunks(rooms, rooms[1]);
    }
}