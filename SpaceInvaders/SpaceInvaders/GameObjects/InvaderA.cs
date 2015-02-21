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


namespace SpaceInvaders.GameObjects
{

    public class InvaderA : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Texture2D invaderTexture;
        private Rectangle rectTexture;
        private Rectangle rectTextureA;
        private Rectangle rectTextureB;
        private Vector2 posInvader;
        private Rectangle gameBounds;
        bool trigger = true;
        private int verocityX = 2;
        private int accum = 0;
        private int healthPoints = 0;
        private int defaultBias = 140;
        private int score = 0;
        Game game;
        

        public InvaderA(Game game, ref Texture2D texture, Rectangle rectA, Rectangle rectB, Vector2 position, int hp)
            : base(game) {
            invaderTexture = texture;
            rectTexture = rectB;
            rectTextureA = rectA;
            rectTextureB = rectB;
            posInvader = position;
            healthPoints = hp;
            score = healthPoints * 10;
            this.game = game;
            gameBounds = new Rectangle(0, 0,
                game.Window.ClientBounds.Width,
                game.Window.ClientBounds.Height);
        }

        

        public override void Initialize() {
            
            base.Initialize();
        }

        public override void Update(GameTime gameTime) {
            moveInvaderA();
            checkOutOfBounds();
            animateInvader();
            base.Update(gameTime);
        }
        //------------Движения захватчика по состоянию триггера-----//
        private void moveInvaderA() {
            if (trigger) {
                posInvader.X -= verocityX;
                accum += verocityX;
                if (accum >= defaultBias) {
                    trigger = false;
                    accum = 0;
                }
            }
            else {
                posInvader.X += verocityX;
                accum += verocityX;
                if (accum >= defaultBias) {
                    trigger = true;
                    accum = 0;
                }
            }
        }
        //------------Движения захватчика по состоянию триггера-----//

        //------------Проверка выхода за пределы экрана--------------//
        private void checkOutOfBounds() {
            if (posInvader.X < gameBounds.Left)
            {
                posInvader.X = gameBounds.Left;
            }
            if (posInvader.X > gameBounds.Width - rectTexture.Width)
            {
                posInvader.X = gameBounds.Width - rectTexture.Width;
            }
            if (posInvader.Y < gameBounds.Top)
            {
                posInvader.Y = gameBounds.Top;
            }
            if (posInvader.Y > gameBounds.Height - rectTexture.Height)
            {
                posInvader.Y = gameBounds.Height - rectTexture.Height;
            }
        }
        //------------Проверка выхода за пределы экрана--------------//

        //----Начисление очков и уменьшение здоровья при попадании----//
        public bool hitted() { 
            --healthPoints;
            if (healthPoints == 0) {
                Game1.addScore(score);
                Dispose(true);
                return true;
            }
            invaderShot();
            return false;
        }
        //----Начисление очков и уменьшение здоровья при попадании----//

        private void animateInvader() { //Топорная анимация
            if (accum <= defaultBias/4)
                rectTexture = rectTextureB;
            else if (accum >= defaultBias/2)
                rectTexture = rectTextureA;
        }

        //-----------------------Выстрел-----------------------------//
        public void invaderShot() {
            Turret turret = Game1.getTurret();
            Rocket rocket = new Rocket(game, ref invaderTexture,
                        new Rectangle(184, 96, 10, 10),
                        new Vector2(posInvader.X + (rectTexture.Width / 2 - 10), posInvader.Y + rectTexture.Height));
            Game1.listRockets.Add(rocket);
            game.Components.Add(rocket);
        }
        //-----------------------Выстрел-----------------------------//

        //--------------------------GETTERS------------------------------//
        public Vector2 getPosition() { return posInvader; }
        public Rectangle getRectange() { return rectTexture; }
        //--------------------------GETTERS------------------------------//     

        public override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.LightSkyBlue);
            SpriteBatch sprBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            sprBatch.Draw(invaderTexture, posInvader, rectTexture, Color.White);
            base.Draw(gameTime);
        }
    }
}
