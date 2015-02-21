using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using SpaceInvaders.Menu;

namespace SpaceInvaders.Menu {
    class OptionsMenu : MenuItem {
        public List<MenuItem> Items { get; set; }
        private SpriteFont font;
        private Texture2D background;
        private int currentItem = 1;
        private KeyboardState kbOldState;
        private MouseState mouseOldState;

        public OptionsMenu(string name) : base(name) {
            Items = new List<MenuItem>();
            Active = true;
        }

        public void Update() {
            //---------------Выбор с помощью клавиатуры------------------//
            KeyboardState kbState = Keyboard.GetState();
            if (kbState.IsKeyDown(Keys.Enter) && kbState.IsKeyUp(Keys.RightAlt) && kbOldState.IsKeyUp(Keys.Enter)) {
                Items[currentItem].onClick();
            }
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
            if ((mState.X >= 100 && mState.X <= 250) && (mState.Y >= 100 && mState.Y <= 135)) {
                currentItem = 0;
                if ((mState.LeftButton == ButtonState.Pressed) && (mouseOldState.LeftButton == ButtonState.Released))
                    Items[currentItem].onClick();
            }
            if ((mState.X >= 100 && mState.X <= 200) && (mState.Y >= 140 && mState.Y <= 165)) {
                currentItem = 1;
                if ((mState.LeftButton == ButtonState.Pressed) && (mouseOldState.LeftButton == ButtonState.Released))
                    Items[currentItem].onClick();
            }
            if ((mState.X >= 100 && mState.X <= 150) && (mState.Y >= 180 && mState.Y <= 205)) {
                currentItem = 2;
                if ((mState.LeftButton == ButtonState.Pressed) && (mouseOldState.LeftButton == ButtonState.Released))
                    Items[currentItem].onClick();
            }
            mouseOldState = mState;
            //---------------Выбор с помощью мышки------------------//
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            int y = 100;
            foreach (MenuItem item in Items) {
                Color color = Color.Black;
                if (item.Active == false)
                    color = Color.White;
                else if (item == Items[currentItem])
                    color = Color.Red;
                spriteBatch.DrawString(font, item.Name, new Vector2(100, y), color);
                y += 40;
            }
            spriteBatch.End();
        }

        public void LoadContent(ContentManager Content) {
            font = Content.Load<SpriteFont>(@"MenuFont");
            background = Content.Load<Texture2D>(@"background");
        }
    }
}
