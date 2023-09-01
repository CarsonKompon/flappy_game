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
public class Player : Panel
{
    public static float GRAVITY = 1600f;
    public float PixelWidth { get => Box.Rect.Width * ScaleFromScreen; }
    public float PixelHeight { get => Box.Rect.Height * ScaleFromScreen; }
    
    
    public Friend Member { get; set; }
    public Vector2 Position;
    public float Vspd = 0f;


    Image IconImage;
    Label NameLabel;

    public Player()
    {
        var iconContainer = Add.Panel("icon-container");
        IconImage = iconContainer.Add.Image("", "icon");
        NameLabel = Add.Label("", "nametag");

        Position = new Vector2(GameMenu.ScreenWidth/2f, GameMenu.ScreenHeight/3f);
    }

    public void SetMember(Friend member)
    {
        Member = member;
        IconImage.SetTexture($"avatar:{Member.Id}");
        NameLabel.Text = Member.Name;

        if(IsLocalPlayer()) AddClass("me");
    }

	public override void Tick()
    {
        Vspd += GRAVITY * Time.Delta;

        if((Input.Pressed("jump") || Input.Pressed("attack1")) && IsLocalPlayer())
        {
            Jump();
        }

        IconImage.SetClass("jumping", Vspd < 0);
        IconImage.SetClass("falling", Vspd > 0);

        Position.x += 200 * Time.Delta;
        Position.y += Vspd * Time.Delta;

        Style.Left = Position.x - GameMenu.CameraX;
        Style.Top = Position.y;
        
        if(IsLocalPlayer())
        {
            if(Position.y > GameMenu.ScreenHeight - PixelHeight/2 || Position.y < PixelHeight/2)
            {
                GameMenu.Kill(Member.Id);
            }
        }
    }

    public void Jump()
    {
        Vspd = -550f;
        GameMenu.Instance.Flaps++;
        GameMenu.Instance.NetworkPlayerUpdate(this);
    }

    public bool IsLocalPlayer()
    {
        return (Member.Id == Game.SteamId);
    }

}