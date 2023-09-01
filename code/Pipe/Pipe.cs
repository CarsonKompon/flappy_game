using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandbox.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

namespace FlappyGame;

[StyleSheet]
public class Pipe : Panel
{
    public Vector2 Position;
    public int Gap = 250;
    public bool Passed = false;


    Image IconImage;
    Label NameLabel;
    public float PixelWidth { get => Box.Rect.Width * ScaleFromScreen; }

    public Pipe()
    {
        Add.Panel("shaft-top");
        Add.Panel("end-top");
        var gap = Add.Panel("gap");
        gap.Style.Height = Gap;
        Add.Panel("end-bottom");
        Add.Panel("shaft-bottom");
    }

    public void Init(ulong x, int y)
    {
        Position = new Vector2(x, y);
    }

	public override void Tick()
	{
		Style.Left = Position.x - GameMenu.CameraX;
        Style.Height = 2048 + 120 + Gap;
        Style.Top = Position.y - (Style.Height?.Value/2);

        // if(Position.x < GameMenu.CameraX)
        // {
        //     //Delete();
        // }
	}


}