using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;

public class Arrow : GameObject
{
    private readonly Sprite _arrowRightSprite;
    private readonly Sprite _arrowLeftSprite;
    private readonly Collider _collider;

    private int _selfDestructMs = 2000;
    private int _selfDestructTimer = 0;

    private Vector2 _position;
    public Vector2 Position
    {
        get => _position;
        set
        {
            _position = value;
            if (_arrowRightSprite != null) _arrowRightSprite.Position = value;
            if (_arrowLeftSprite != null) _arrowLeftSprite.Position = value;
            if (_collider != null) _collider.Position = value;
        }
    }

    public Vector2 Velocity { get; set; }

    public Arrow()
    {
        var arrowRightAsset = AssetManager.Textures.Get("ArrowRight");
        var arrowRightSprite = arrowRightAsset!.AssetObject;
        if (arrowRightSprite == null) return;

        _arrowRightSprite = new Sprite(arrowRightSprite);
        _arrowRightSprite.Scale = 1f;

        var arrowLeftAsset = AssetManager.Textures.Get("ArrowLeft");
        var arrowLeftSprite = arrowLeftAsset!.AssetObject;
        if (arrowLeftSprite == null) return;

        _arrowLeftSprite = new Sprite(arrowLeftSprite);
        _arrowLeftSprite.Scale = 1f;

        _selfDestructTimer = _selfDestructMs;

        _collider = new Collider(this);
        _collider.OnCollisionAction = OnCollision;
        _collider.Width = Convert.ToInt32(arrowLeftSprite.Width * 0.75f);
        _collider.Height = Convert.ToInt32(arrowLeftSprite.Height * 0.5f);
        _collider.Enabled = true;
    }

    public override void Update(GameTime gameTime)
    {
        UpdateArrowSprite();
        HandleSelfDestruct(gameTime);
        Move(gameTime);
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _arrowLeftSprite.Draw(spriteBatch);
        _arrowRightSprite.Draw(spriteBatch);
        base.Draw(spriteBatch);
    }

    private void Move(GameTime gameTime)
    {
        Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    private void UpdateArrowSprite()
    {
        if (Velocity.X >= 0)
        {
            _arrowLeftSprite.Enabled = false;
            _arrowRightSprite.Enabled = true;
        }
        else
        {
            _arrowLeftSprite.Enabled = true;
            _arrowRightSprite.Enabled = false;
        }
    }

    private void HandleSelfDestruct(GameTime gameTime)
    {
        _selfDestructTimer -= gameTime.ElapsedGameTime.Milliseconds;

        if (_selfDestructTimer <=0)
        {
            Destroy();
        }
    }

    private void OnCollision(Collider collider)
    {
        if (collider.Parent.Name == "Player")
        {
            var player = collider.Parent;
            var damagable = (IDamagable)player;

            if (damagable != null)
            {
                var rand = new Random();
                damagable.ApplyDamage(rand.Next(5, 12));
            }

            Destroy();
        }
    }
}
