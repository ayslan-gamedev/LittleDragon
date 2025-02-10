using Godot;
using LittleDragon.scripts.map;
using LittleDragon.scripts.map.grid;

namespace LittleDragon.scripts;

public partial class Test : Node2D
{
    [Export] private Node2D _node2D;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("starting");
		
        const int width = 5;
        const int height = 4;	
		
        var chunkBuilders = new ChunkBuilder[]
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
        
        var a = ChunkManager.InstantiateGrid(_node2D, grid);

        GD.Print(a.Count);
        GD.Print("printing: ");
        foreach (var room in a)
        {
            GD.Print(room.ToString());
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
		
    }
}