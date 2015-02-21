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

    public class Rocket : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Texture2D rocketTexture;
        private Rectangle rectTexture;
        private Vector2 posRocket;
        private Rectangle gameBounds;
        private List<InvaderA> invadersList;
        private int verocityYturret = 4;
        private int verocityYenemy = 2;
        private Turret turret;

        private bool isTurret;
        public Rocket(Game game, ref Texture2D texture, Rectangle rect, Vector2 position, ref List<InvaderA> invaders)
            : base(game)
        {
            rocketTexture = texture;
            rectTexture = rect;
            posRocket = position;
            invadersList = invaders;
            isTurret = false;
            gameBounds = new Rectangle(0, 0,
                game.Window.ClientBounds.Width,
                game.Window.ClientBounds.Height);
        }
        public Rocket(Game game, ref Texture2D texture, Rectangle rect, Vector2 position)
            : base(game) {
            rocketTexture = texture;
            rectTexture = rect;
            posRocket = position;
            this.turret = Game1.getTurret();
            isTurret = true;
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
            moveRocket();
            checkOutOfBounds();
            checkCollide();
            base.Update(gameTime);
        }
        //---------Движение снаряда--------//
        private void moveRocket()
        {
            if (!isTurret)
                posRocket.Y -= verocityYturret;
            else
                posRocket.Y += verocityYenemy;
        }
        //---------Движение снаряда--------//

        //--------Выход за границы---------//
        private void checkOutOfBounds()
        {
            if (posRocket.Y <= gameBounds.Top || posRocket.Y >= gameBounds.Bottom)
            {
                Dispose(true);
            }

        }
        //--------Выход за границы---------//

        //-------Взаимодействие с объектами----------//
        private void checkCollide()
        {
            if (isTurret) { //Попадание по турельке
                    Vector2 posTur = turret.getPosition();
                    Rectangle rectTur = turret.getRectange();
                    if (posTur.X < posRocket.X + rectTexture.Width &&
                    posTur.X + rectTur.Width > posRocket.X &&
                    posTur.Y < posRocket.Y + rectTexture.Height &&
                    posTur.Y + rectTur.Height > posRocket.Y) {
                        turret.hitted();
                        Game1.listRockets.Remove(this);
                        Dispose(true);
                    }
                    for (int i = 0; i < 3; i++) { //Попадание по зданиям
                        Vector2 posBuild = Game1.getBuilding(i).getPosition();
                        Rectangle rectBuild = Game1.getBuilding(i).getRectange();
                        if (!Game1.getBuilding(i).isDestroyedBuilding()) {
                            if (posBuild.X < posRocket.X + rectTexture.Width &&
                            posBuild.X + rectBuild.Width > posRocket.X &&
                            posBuild.Y < posRocket.Y + rectTexture.Height &&
                            posBuild.Y + rectBuild.Height > posRocket.Y) {
                                Game1.getBuilding(i).hitted();
                                Game1.listRockets.Remove(this);
                                Dispose(true);
                            }
                        }
                    }
            }
            else { //Попадание по захватчику
                for (int i = 0; i < invadersList.Count; i++) {
                    Vector2 posInv = invadersList.ElementAt<InvaderA>(i).getPosition();
                    Rectangle rectInv = invadersList.ElementAt<InvaderA>(i).getRectange();
                    if (posInv.X < posRocket.X + rectTexture.Width &&
                    posInv.X + rectInv.Width > posRocket.X &&
                    posInv.Y < posRocket.Y + rectTexture.Height &&
                    posInv.Y + rectInv.Height > posRocket.Y) {
                        if (invadersList.ElementAt<InvaderA>(i).hitted())
                            invadersList.RemoveAt(i);
                        Dispose(true);

                    }
                }
            }
        }
        //-------Взаимодействие с объектами----------//

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sprBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            sprBatch.Draw(rocketTexture, posRocket, rectTexture, Color.White);
            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            
            base.Dispose(disposing);
            
        }
    }
}
