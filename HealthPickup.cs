using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;

public class HealthPickup : GameObject
{
    private readonly Sprite _sprite;
    private readonly Collider _collider;
    private int _defaultHealAmount = 5;

    private int _itemAliveMs = 5000;
    private int _itemAliveTimer = 0;

    public int HealAmount { get; set; }
    public bool HasTimeout { get; set; }

    private Vector2 _position;
    public Vector2 Position
    {
        get => _position;
        set
        {
            _position = value;
            if (_sprite != null) _sprite.Position = value;
            if (_collider != null) _collider.Position = value;
        }
    }

    public HealthPickup (Texture2D texture2D)
    {
        Name = "Health";
        HealAmount = _defaultHealAmount;
        _position = Vector2.Zero;

        _sprite = new Sprite(texture2D);
        _collider = new Collider(this);

        _collider.OnCollisionAction = OnCollision;
        _collider.Position = _position;
        _collider.Width = texture2D.Width;
        _collider.Height = texture2D.Height;
        _collider.Enabled = false;
        _collider.EnablePhysicsCollions = false;

        _itemAliveTimer = _itemAliveMs;
    }

    public override void Update(GameTime gameTime)
    {
        UpdateColliderPosition();
        SetSpritePosition();
        HandleAliveTimeout(gameTime);
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _sprite.Draw(spriteBatch);
        base.Draw(spriteBatch);
    }


    public void OnCollision(Collider collider)
    {
        if (collider.Parent.Name == "Player")
        {
            var healable = (IHealable)collider.Parent;
            if (healable != null)
            {
                healable.Heal(HealAmount);
                Destroy();
            }
        }
    }

    public void Enable()
    {
        _collider.Enabled = true;
        HasTimeout = true;
    }

    private void UpdateColliderPosition()
    {
        _collider.Position = Position;
    }

    private void SetSpritePosition()
    {
        _sprite.Position = this.Position;
    }

    private void HandleAliveTimeout(GameTime gameTime)
    {
        if (HasTimeout == false) return;

        _itemAliveTimer -= gameTime.ElapsedGameTime.Milliseconds;

        if (_itemAliveTimer <= 0)
        {
            Destroy();
        }
    }
}
