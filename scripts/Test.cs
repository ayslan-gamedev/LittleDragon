using System.Collections.Generic;
using Godot;
using LittleDragon.scripts.map;
using LittleDragon.scripts.map.grid;

namespace LittleDragon.scripts;

public partial class Test : Node2D
{
    [Export] private Node2D _node2D;
    
    public override void _Ready()
    {
        const int width = 5;
        const int height = 4;
		
        var chunkBuilders = new RoomBuilder[]
        {
            new("A", 0, 0 ),
            new("B", 2, 2 ),
            new("C", 2, 3 ),
            new("D", 4, 2 ),
            //new("bonus", 1, 4, 0, 3 ),
        };

        var (grid, dots) = GridFactory.CreateGrid(width, height, chunkBuilders);
        GridFactory.Connect(grid,dots[0], dots[1]);
        GridFactory.Connect(grid,dots[1], dots[2]);
        GridFactory.Connect(grid,dots[0], dots[3]);
        //GridBuilder.Connect(grid,dots[new Random().Next(0, 3)], dots[4]);

        PrintGrid(grid);
        
        var rooms = ChunkManager.InstantiateGrid(_node2D, grid);

        // make all invisible
        foreach (var room in rooms)
        {
            room.GetOwner<Node2D>().Visible = false;
        }
        
        PrintRooms(rooms);
        
        foreach (var room in rooms)
        {
            room.GetOwner<Node2D>().Visible = true;
        }
    }

    private static void PrintGrid(Grid grid)
    {
        GD.Print(grid.ToString());
    }

    private static void PrintRooms(List<Chunk> a)
    {
        if (a.Count == 0) GD.Print("No rooms found!");

        GD.Print(a.Count);
        GD.Print("printing: ");
        foreach (var room in a)
        {
            GD.Print(room.ToString());
        }
    }
}