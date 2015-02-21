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

    public class Turret : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Texture2D turretTexture;
        private Rectangle rectTexture;
        private Vector2 posTurret;
        private Rectangle gameBounds;
        private Game game;
        private List<InvaderA> invadersList;
        private MouseState oldMouseState;

        private double time = 0;
        private double timeBetweenShots = 15;
        private bool isShoted = false;
        private bool isDestroyed = false;

        private int healthPoints = 3;


        public Turret(Game game, ref Texture2D texture, Rectangle rect, Vector2 position, ref List<InvaderA> invaders)
            : base(game)
        {
            turretTexture = texture;
            rectTexture = rect;
            posTurret = position;
            this.game = game;
            invadersList = invaders;
            gameBounds = new Rectangle(0, 0,
                game.Window.ClientBounds.Width,
                game.Window.ClientBounds.Height);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (!isDestroyed) {
                moveTurret();
                checkOutOfBounds();
                turretShot();
                if (isShoted) {
                    time++;
                    if (time >= timeBetweenShots) {
                        time = 0;
                        isShoted = false;
                    }
                }
            }
            
            base.Update(gameTime);
        }

        //------------------------Передвижение турельки------//
        private void moveTurret()
        {
            if (Game1.isKeyboard == false) {
                //--------------------Мышкой-----------------------//
                MouseState mouseState = Mouse.GetState();
                if (mouseState.X > (posTurret.X + rectTexture.Width / 2)) {
                    posTurret.X += 5;
                }
                else if (mouseState.X < posTurret.X + rectTexture.Width / 2) {
                    posTurret.X -= 5;
                }
                oldMouseState = mouseState;
                //--------------------Мышкой----------------------//
            }
            else {
                //-----------------Клавиатурой--------------------//
                KeyboardState kbState = Keyboard.GetState();
                if (kbState.IsKeyDown(Keys.Right))
                    posTurret.X += 5;
                else if (kbState.IsKeyDown(Keys.Left))
                    posTurret.X -= 5;
                //-----------------Клавиатурой--------------------//
            }

        }
        //------------------------Передвижение турельки------//

        //-------Проверка выхода за пределы экрана-----------//
        private void checkOutOfBounds()
        {
            if (posTurret.X < gameBounds.Left)
            {
                posTurret.X = gameBounds.Left;
            }
            if (posTurret.X > gameBounds.Width - rectTexture.Width)
            {
                posTurret.X = gameBounds.Width - rectTexture.Width;
            }
        }
        //-------Проверка выхода за пределы экрана-----------//


        //------------------Выстрел турельки----------------//
        private void turretShot()
        {
            if (Game1.isKeyboard == false) {
                MouseState mouseState = Mouse.GetState();
                if (mouseState.LeftButton == ButtonState.Pressed) {
                    isShoted = true;
                    if (time == 0) {
                        Rocket rocket = new Rocket(game, ref turretTexture,
                            new Rectangle(184, 96, 10, 25),
                            new Vector2(posTurret.X + (rectTexture.Width / 2 - 5), posTurret.Y),
                            ref invadersList);
                        Game1.listRockets.Add(rocket);
                        game.Components.Add(rocket);
                    }
                }
            }
            else {
                KeyboardState kbState = Keyboard.GetState();
                if (kbState.IsKeyDown(Keys.Space)) {
                    isShoted = true;
                    if (time == 0) {
                        Rocket rocket = new Rocket(game, ref turretTexture,
                            new Rectangle(184, 96, 10, 25),
                            new Vector2(posTurret.X + (rectTexture.Width / 2 - 5), posTurret.Y),
                            ref invadersList);
                        Game1.listRockets.Add(rocket);
                        game.Components.Add(rocket);
                    }
                }
            }

        }
        //------------------Выстрел турельки----------------//

        //------------------Попадание по турельке----------------//
        public bool hitted() {
            --healthPoints;
            if (healthPoints == 0) {
                destroyed(new Rectangle(283, 243, 90, 54));
                return true;
            }
            return false;
        }
        //------------------Попадание по турельке----------------//

        private void destroyed(Rectangle rect) { rectTexture = rect; isDestroyed = true; }

        //--------------------------GETTERS------------------------------//
        public Vector2 getPosition() { return posTurret; }
        public Rectangle getRectange() { return rectTexture; }
        public Game getGame() { return game; }
        public bool isDestroyedTurret() { return isDestroyed; }
        public int getHP(){ return healthPoints; }
        //--------------------------GETTERS------------------------------//

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sprBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            sprBatch.Draw(turretTexture, posTurret, rectTexture, Color.White);
            base.Draw(gameTime);
        }
    }
}
