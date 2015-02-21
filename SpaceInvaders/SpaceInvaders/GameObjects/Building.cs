using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace SpaceInvaders.GameObjects {

    public class Building : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Texture2D texture;
        private Rectangle rectTexture;
        private Vector2 posHome;
        private bool isDestroyed = false;
        private int healthPoints = 10;


        public Building(Game game, ref Texture2D texture, Rectangle rect, Vector2 position)
            : base(game)
        {
            this.texture = texture;
            rectTexture = rect;
            posHome = position;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        //---------------Попадание по зданию------------//
        public bool hitted() {
            --healthPoints;
            Game1.reduceScore(10);
            if (healthPoints == 0) {
                destroyed(new Rectangle(24, 222, 102, 76));
                Game1.reduceScore(100);
                return true;
            }
            return false;
        }
        //---------------Попадание по зданию------------//

        private void destroyed(Rectangle rect) { rectTexture = rect; isDestroyed = true; }

        //--------------------------GETTERS------------------------------//
        public Vector2 getPosition() { return posHome; }
        public Rectangle getRectange() { return rectTexture; }
        public bool isDestroyedBuilding() { return isDestroyed; }
        public int getHP() { return healthPoints; }
        //--------------------------GETTERS------------------------------//

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sprBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            sprBatch.Draw(texture, posHome, rectTexture, Color.White);
            base.Draw(gameTime);
        }
    }
    
}
