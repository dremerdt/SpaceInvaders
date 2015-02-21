using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace SpaceInvaders.Menu {
    class Scores : MenuItem {
        public List<int> Items { get; set; }
        private SpriteFont font;
        private Texture2D background;
        public bool readed = true;

        public Scores(string name)
            : base(name) {
                Items = new List<int>();
            Active = true;
        }
        private void getScoresFromFile() {
            if (readed) {
                using (StreamReader sr = new StreamReader("scores.sc")) {
                    while (!sr.EndOfStream)
                        Items.Add(int.Parse(sr.ReadLine()));
                    }
                readed = false;
                Items.Sort(delegate(int i1, int i2) { return i2.CompareTo(i1); });
                if (Items.Count > 10)
                    Items.RemoveRange(10, Items.Count - 10);
                using (StreamWriter sw = new StreamWriter("scores.sc")) {
                    foreach (int i in Items)
                        sw.WriteLine(i.ToString());
                    sw.Flush();
                };
                
            }
        }
        public void Update() {
            getScoresFromFile();
            //---------------Выбор с помощью клавиатуры------------------//
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Game1.setGameStateMenu();
            //---------------Выбор с помощью клавиатуры------------------//

            //---------------Выбор с помощью мышки------------------//
            MouseState mState = Mouse.GetState();
            if ((mState.X >= 5 && mState.X <= 55) && (mState.Y >= 5 && mState.Y <= 30))
                if (mState.LeftButton == ButtonState.Pressed)
                    Game1.setGameStateMenu();     
            //---------------Выбор с помощью мышки------------------//
        }
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(font, "Back", new Vector2(5, 5), Color.Red);
            int y = 120;
            foreach (int item in Items) {
                spriteBatch.DrawString(font, item.ToString(), new Vector2(375, y), Color.Black);
                y += 40;
            }
            spriteBatch.End();
        }

        public void LoadContent(ContentManager Content) {
            font = Content.Load<SpriteFont>(@"MenuFont");
            background = Content.Load<Texture2D>(@"backgroundscore");
        }
    }
}
