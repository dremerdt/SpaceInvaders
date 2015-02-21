using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace SpaceInvaders.Menu {
    class MainMenu {
        public List<MenuItem> Items { get; set; }
        private SpriteFont font;
        private Texture2D texture;
        private Texture2D backGround;

        private int currentItem = 0;
        KeyboardState kbOldState;

        public MainMenu() {
            Items = new List<MenuItem>();
        }
        public void setTexture(ref Texture2D texture) { this.texture = texture; }
        public void Update() {
            //---------------Выбор с помощью клавиатуры------------------//
            KeyboardState kbState = Keyboard.GetState();
            if (kbState.IsKeyDown(Keys.Enter) && kbState.IsKeyUp(Keys.RightAlt) && kbOldState.IsKeyUp(Keys.Enter))
                Items[currentItem].onClick();
            int delta = 0;
            if (kbState.IsKeyDown(Keys.Up) && kbOldState.IsKeyUp(Keys.Up))
                delta = -1;
            if (kbState.IsKeyDown(Keys.Down) && kbOldState.IsKeyUp(Keys.Down))
                delta = 1;

            currentItem += delta;
            bool ok = false;

            while (!ok) {
                if (currentItem < 0)
                    currentItem = Items.Count - 1;
                else if (currentItem > Items.Count - 1)
                    currentItem = 0;
                else if (Items[currentItem].Active == false)
                    currentItem += delta;
                ok = true;
            }
            kbOldState = kbState;
            //---------------Выбор с помощью клавиатуры------------------//

            //---------------Выбор с помощью мышки------------------//
            MouseState mState = Mouse.GetState();
            if ((mState.X >= 300 && mState.X <= 430) && (mState.Y >= 250 && mState.Y <= 275)) {
                currentItem = 0;
                if (mState.LeftButton == ButtonState.Pressed)
                    Items[currentItem].onClick();
            }
            if ((mState.X >= 300 && mState.X <= 450) && (mState.Y >= 290 && mState.Y <= 325)) {
                currentItem = 1;
                if (mState.LeftButton == ButtonState.Pressed && Items[currentItem].Active)
                    Items[currentItem].onClick();
            }
            if ((mState.X >= 300 && mState.X <= 390) && (mState.Y >= 330 && mState.Y <= 355)) {
                currentItem = 2;
                if (mState.LeftButton == ButtonState.Pressed)
                    Items[currentItem].onClick();
            }
            if ((mState.X >= 300 && mState.X <= 375) && (mState.Y >= 370 && mState.Y <= 395)) {
                currentItem = 3;
                if (mState.LeftButton == ButtonState.Pressed)
                    Items[currentItem].onClick();
            }
            if ((mState.X >= 300 && mState.X <= 355) && (mState.Y >= 410 && mState.Y <= 435)) {
                currentItem = 4;
                if (mState.LeftButton == ButtonState.Pressed)
                    Items[currentItem].onClick();
            }
            //---------------Выбор с помощью мышки------------------//
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Begin();
            spriteBatch.Draw(backGround, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(texture, new Vector2(300, 150), new Rectangle(22, 92, 90, 42), Color.White);
            int y = 250;
            foreach (MenuItem item in Items) {
                Color color = Color.Black;
                if (item.Active == false)
                    color = Color.White;
                else if (item == Items[currentItem])
                    color = Color.Red;
                spriteBatch.DrawString(font, item.Name, new Vector2(300, y), color);
                y += 40;
            }
            spriteBatch.End();
        }

        public void LoadContent(ContentManager Content) {
            font = Content.Load<SpriteFont>(@"MenuFont");
            backGround = Content.Load<Texture2D>(@"background");
        }

    }
}
