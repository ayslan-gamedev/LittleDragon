using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using LittleDragon.scripts.map;
using LittleDragon.scripts.map.grid;
using static Godot.GD;

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
            new("B", 2, 10, 2 , 20),
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
        //PrintRooms(rooms);
        
        rooms[1].GetOwner<Node2D>().Visible = true;
        rooms[1].GetOwner<Node2D>().Position = new Vector2(400, 0);
        rooms[1].ActiveNeighbors();

        var chunkCount = rooms.Count(room => room.GetOwner<Node2D>().Visible);
        Print(chunkCount);
    }
    
    private static void PrintGrid(Grid grid)
    {
        Print(grid.ToString());
    }

    private static void PrintRooms(List<Chunk> a)
    {
        if (a.Count == 0) Print("No rooms found!");

        Print(a.Count);
        Print("printing: ");
        foreach (var room in a)
        {
            Print(room.ToString());
        }
    }
}