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
using SpaceInvaders.GameObjects;
using SpaceInvaders.Menu;
using System.IO;

namespace SpaceInvaders
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {

        public static List<Rocket> listRockets;
        private static int score = 0;
        public static bool isKeyboard = false;

        private int frequencyInvaderShots = 1;
        private bool fullScreen = false;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private static Turret turret;
        private static Building buildingA;
        private static Building buildingB;
        private static Building buildingC;
        private Texture2D texture;
        private List<InvaderA> listInvadersA;

        private SpriteFont gameFont;

        private MainMenu menu;
        private static GameState gameState = GameState.Menu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize() {
            using (StreamWriter sw = new StreamWriter("scores.sc", true)) {
                sw.Flush();
            };
            //-------------Разрешение экрана--------------//
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
            //-------------Разрешение экрана--------------//

            //------------Инициализация меню игры---------------//
            menu = new MainMenu();
            MenuItem newGame = new MenuItem("Start Game");
            MenuItem resumeGame = new MenuItem("Resume Game");
            OptionsMenu optionsGame = new OptionsMenu("Options");
            Scores scoresGame = new Scores("Scores");
            MenuItem exitGame = new MenuItem("Exit");
            resumeGame.Active = false;

            newGame.Click += new EventHandler(newGame_Click);
            resumeGame.Click += new EventHandler(resumeGame_Click);
            optionsGame.Click += new EventHandler(options_Click);
            scoresGame.Click += new EventHandler(scoresGame_Click);
            exitGame.Click += new EventHandler(exitGame_Click);

            menu.Items.Add(newGame);
            menu.Items.Add(resumeGame);
            menu.Items.Add(optionsGame);
            menu.Items.Add(scoresGame);
            menu.Items.Add(exitGame);
            //------------Инициализация меню игры---------------//

            //------------Инициализация опций игры---------------//
            MenuItem fullScreenOption = new MenuItem("Full Screen");
            MenuItem inputModeOption = new MenuItem("Mouse");
            MenuItem backOption = new MenuItem("Back");

            fullScreenOption.SName = "Window Mode";
            inputModeOption.SName = "Keyboard";

            fullScreenOption.Click += new EventHandler(fullScreenOption_Click);
            inputModeOption.Click += new EventHandler(inputModeOption_Click);
            backOption.Click += new EventHandler(backOption_Click);

            ((OptionsMenu)menu.Items.ElementAt<MenuItem>(2)).Items.Add(fullScreenOption);
            ((OptionsMenu)menu.Items.ElementAt<MenuItem>(2)).Items.Add(inputModeOption);
            ((OptionsMenu)menu.Items.ElementAt<MenuItem>(2)).Items.Add(backOption);
            //------------Инициализация опций игры---------------//

            IsMouseVisible = true;
            base.Initialize();
        }


      
        //----------Инициализация кнопок опций игры-------------//
        void inputModeOption_Click(object sender, EventArgs e) {
            if (isKeyboard)
                isKeyboard = false;
            else
                isKeyboard = true;
        }

        void fullScreenOption_Click(object sender, EventArgs e) {
            if (fullScreen == false) {
                graphics.IsFullScreen = true;
                fullScreen = true;
            }
            else {
                graphics.IsFullScreen = false;
                fullScreen = false;
            }
            graphics.ApplyChanges();
        }

        void backOption_Click(object sender, EventArgs e) {
            gameState = GameState.Menu;
        }
        //----------Инициализация кнопок опций игры-------------//

        //----------Инициализация кнопок меню игры-------------//
        void exitGame_Click(object sender, EventArgs e) {
            this.Exit();
        }

        void resumeGame_Click(object sender, EventArgs e) {
            gameState = GameState.Game;
        }

        void options_Click(object sender, EventArgs e) {
            gameState = GameState.Options;
        }
        void scoresGame_Click(object sender, EventArgs e) {
            gameState = GameState.Scores;
        }
        void newGame_Click(object sender, EventArgs e) {
            menu.Items[1].Active = true;
            gameState = GameState.Game;
            setToZeroObjects();
            createGameObjects();
        }
        //----------Инициализация кнопок меню игры-------------//

        //---------Очистка объектов для создания новой игры------//
        private void setToZeroObjects() {
            score = 0;
            clearInvaders();
            listInvadersA = new List<InvaderA>();
            clearRockets();
            listRockets = new List<Rocket>();
            if (turret != null)
                turret.Dispose();
            if (buildingA != null)
                buildingA.Dispose();
            if (buildingB != null)
                buildingB.Dispose();
            if (buildingC != null) 
                buildingC.Dispose();
        }

        private void clearInvaders() {
            if (listInvadersA != null)
                foreach (InvaderA invader in listInvadersA) {
                    invader.Dispose();
                }
        }

        private void clearRockets() {
            if (listRockets != null)
                foreach (Rocket rocket in listRockets) {
                    rocket.Dispose();
                }
        }
        //---------Очистка объектов для создания новой игры------//

        protected override void LoadContent()
        {  
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);
            texture = Content.Load<Texture2D>(@"spaceinvaders");
            menu.setTexture(ref texture);
            menu.LoadContent(Content);
            ((OptionsMenu)menu.Items.ElementAt<MenuItem>(2)).LoadContent(Content);
            ((Scores)menu.Items.ElementAt<MenuItem>(3)).LoadContent(Content);
            gameFont = Content.Load<SpriteFont>(@"DebugFont");

        }

        private void createGameObjects()
        {
            //-----Создание захватчиков для захвата земли-----//
            Vector2 posInvader = new Vector2(140, 100);
            for (int j = 0; j < 6; j++) {
                InvaderA invader = new InvaderA(this, ref texture, 
                    new Rectangle(144, 75, 94, 69), 
                    new Rectangle(272, 75, 94, 69), 
                    posInvader, 4);
                listInvadersA.Add(invader);
                posInvader.X += 20 + 94;
                Components.Add(invader);
            }
            posInvader.X = 140;
            posInvader.Y += 20 + 69;

            for (int j = 0; j < 8; j++) {
                InvaderA invader = new InvaderA(this, ref texture,
                    new Rectangle(404, 75, 69, 69), 
                    new Rectangle(509, 75, 69, 69), 
                    posInvader, 3);
                listInvadersA.Add(invader);
                posInvader.X += 15 + 69;
                Components.Add(invader);
            }
            posInvader.X = 140;
            posInvader.Y += 20 + 69;

            for (int j = 0; j < 8; j++) {
                InvaderA invader = new InvaderA(this, ref texture, 
                    new Rectangle(407, 227, 60, 69),
                    new Rectangle(503, 227, 60, 69),
                    posInvader, 2);
                listInvadersA.Add(invader);
                posInvader.X += 15 + 69;
                Components.Add(invader);
            }
            //-----Создание захватчиков для захвата земли-----//

            //------Создание зданий беззащитных людишек-------//
            buildingA = new Building(this, ref texture,
                new Rectangle(614, 71, 102, 73),
                new Vector2(164, 500));
            Components.Add(buildingA);
            buildingB = new Building(this, ref texture,
                new Rectangle(614, 71, 102, 73),
                new Vector2(366, 500));
            Components.Add(buildingB);
            buildingC = new Building(this, ref texture,
                new Rectangle(614, 71, 102, 73), 
                new Vector2(568, 500));
            Components.Add(buildingC);
            //------Создание зданий беззащитных людишек-------//

            //-----------Лист для хранения снярядов-----------//
            listRockets = new List<Rocket>();
            //-----------Лист для хранения снярядов-----------//

            //-----------Последняя надежда землян-------------//
            turret = new Turret(this, ref texture, 
                new Rectangle(176, 251, 63, 45),
                new Vector2(300, 555), 
                ref listInvadersA);
            Components.Add(turret);
            //-----------Последняя надежда землян-------------//
        }

        protected override void UnloadContent()
        {
            texture.Dispose();
        }
        
        MouseState mouseOldState = Mouse.GetState();
        protected override void Update(GameTime gameTime)
        {   
            fullScreenMode(); //Переход между режимами экрана

            if (gameState == GameState.Game) {
                UpdateGameLogic(gameTime);
            }
            else if (gameState == GameState.Menu) menu.Update();
            else if (gameState == GameState.Options)
                ((OptionsMenu)menu.Items.ElementAt<MenuItem>(2)).Update();
            else if (gameState == GameState.Scores)
                ((Scores)menu.Items.ElementAt<MenuItem>(3)).Update();

            //--------Управления игрой, после победы или поражения-------//
            else if (gameState == GameState.Victory || gameState == GameState.Defeat) {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    gameState = GameState.Menu; // Меню
                else if ((Keyboard.GetState().IsKeyDown(Keys.Enter)) ||  // Новая игра
                    ((Mouse.GetState().RightButton == ButtonState.Pressed) && (mouseOldState.LeftButton == ButtonState.Released))) {
                    menu.Items[1].Active = true;
                    gameState = GameState.Game;
                    setToZeroObjects();
                    createGameObjects();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.X)) // Выход
                    this.Exit();
                mouseOldState = Mouse.GetState();
            }
            //--------Управления игрой, после победы или поражения-------//
        }

        private void UpdateGameLogic(GameTime gameTime) {

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                gameState = GameState.Menu;
            //----------------------Выстрел захватчика------------------//
            if (gameTime.TotalGameTime.Seconds >= frequencyInvaderShots) {
                frequencyInvaderShots = gameTime.TotalGameTime.Seconds + new Random().Next(0, 2);
                if (listInvadersA.Count != 0)
                    listInvadersA.ElementAt<InvaderA>(new Random().Next(0, listInvadersA.Count)).invaderShot();
            }
            //----------------------Выстрел захватчика------------------//

            //--------------Проверка на победу или поражение------------//
            if (checkIfDefeat()) {
                menu.Items[1].Active = false;
                gameState = GameState.Defeat;
            }
            else if (checkIfWon()) {
                menu.Items[1].Active = false;
                gameState = GameState.Victory;
                writeScoreToFile();
                ((Scores)menu.Items.ElementAt<MenuItem>(3)).readed = true;
            }
            //--------------Проверка на победу или поражение------------//
            base.Update(gameTime);
        }

        //-----------------Запись очков в фаил-------------------------//
        private void writeScoreToFile() {
            using (StreamWriter sw = new StreamWriter("scores.sc", true)) {
                sw.WriteLine(score + "");
                sw.Flush();
            };
        }
        //-----------------Запись очков в фаил-------------------------//

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightSkyBlue);

            spriteBatch.Begin();
            //-----------Отрисовка информации при победе и поражении--------//
            if (gameState == GameState.Victory || gameState == GameState.Defeat) {
                clearInvaders();
                clearRockets();
                spriteBatch.DrawString(gameFont, defeatOrVictoryText(), new Vector2(400, 100), Color.Black,
                    0, gameFont.MeasureString(defeatOrVictoryText()) / 2, 1.0f, SpriteEffects.None, 0.5f);
            }
            //-----------Отрисовка информации при победе и поражении--------//
            base.Draw(gameTime);
            if (gameState == GameState.Game) //Очки и здоровье
                showScoreAndHP();
            spriteBatch.End();

            // Отрисовка меню
            if (gameState == GameState.Menu) menu.Draw(spriteBatch);

            if (gameState == GameState.Options) ((OptionsMenu)menu.Items.ElementAt<MenuItem>(2)).Draw(spriteBatch);

            if (gameState == GameState.Scores) ((Scores)menu.Items.ElementAt<MenuItem>(3)).Draw(spriteBatch);
            
        }
        //--------------------------GETTERS------------------------------//
        public static Turret getTurret() { return turret; }
        public static Building getBuilding(int i) {
            switch (i) {
                case 0: return buildingA;
                case 1: return buildingB;
                case 2: return buildingC;
                default: return null;
            }
        }
        //--------------------------GETTERS------------------------------//

        //--------------------------SETTERS------------------------------//
        public static void addScore(int plus) { score += plus; }
        public static void reduceScore(int plus) { score -= plus; }
        //--------------------------SETTERS------------------------------//

        //--------------Проверка на победу или поражение------------//
        private bool checkIfDefeat() {
            if (buildingA.isDestroyedBuilding() && 
                buildingB.isDestroyedBuilding() &&
                buildingC.isDestroyedBuilding())
                return true;
            else if (turret.isDestroyedTurret())
                return true;
            return false;
        }
        private bool checkIfWon() {
            if (listInvadersA.Count == 0)
                return true;
            return false;
        }
        //--------------Проверка на победу или поражение------------//

        //--------------Возврат текста при победе или поражении----------//
        private string defeatOrVictoryText() {
            if (gameState == GameState.Victory)
                return "Congratulations! You WON!\nYour score: " + score +
                    "\nPress Enter or Click for the new game, Esc for the Main Menu or X for Exit.";
            else
                return "You were DESTROYED! But don't panic the next time you win!\n" +
                    "\nPress Enter for the new game, Esc for the Main Menu or X for Exit.";
        }
        //--------------Возврат текста при победе или поражении----------//

        //-------------------Отрисовка очков и здоровья-----------------------//
        private void showScoreAndHP() {
            string dummyStr = "Score: " + score;
            spriteBatch.DrawString(gameFont, dummyStr, new Vector2(740, 20), Color.Black,
                    0, gameFont.MeasureString(dummyStr) / 2, 1.0f, SpriteEffects.None, 0.5f);
            if (turret != null)
                dummyStr = "Your HP: " + turret.getHP();
            spriteBatch.DrawString(gameFont, dummyStr, new Vector2(10, 20), Color.Black,
                    0, gameFont.MeasureString(dummyStr) / 2, 1.0f, SpriteEffects.None, 0.5f);
            if (buildingA != null && buildingA.getHP() > 0) {
                dummyStr = buildingA.getHP() + "/10";
                Vector2 dummyPos = new Vector2((buildingA.getPosition().X + buildingA.getRectange().Width / 2),
                    (buildingA.getPosition().Y + buildingA.getRectange().Height - 8));
                spriteBatch.DrawString(gameFont, dummyStr, dummyPos, Color.Black,
                    0, gameFont.MeasureString(dummyStr) / 2, 1.0f, SpriteEffects.None, 0.5f);
            }
            if (buildingB != null && buildingB.getHP() > 0) {
                dummyStr = buildingB.getHP() + "/10";
                Vector2 dummyPos = new Vector2((buildingB.getPosition().X + buildingB.getRectange().Width / 2),
                    (buildingB.getPosition().Y + buildingB.getRectange().Height - 8));
                spriteBatch.DrawString(gameFont, dummyStr, dummyPos, Color.Black,
                    0, gameFont.MeasureString(dummyStr) / 2, 1.0f, SpriteEffects.None, 0.5f);
            }
            if (buildingC != null && buildingC.getHP() > 0) {
                dummyStr = buildingC.getHP() + "/10";
                Vector2 dummyPos = new Vector2((buildingC.getPosition().X + buildingC.getRectange().Width / 2),
                    (buildingC.getPosition().Y + buildingC.getRectange().Height - 8));
                spriteBatch.DrawString(gameFont, dummyStr, dummyPos, Color.Black,
                    0, gameFont.MeasureString(dummyStr) / 2, 1.0f, SpriteEffects.None, 0.5f);
            }           
        }
        //-------------------Отрисовка очков и здоровья-----------------------//

        private void fullScreenMode() { //Установка режима экрана
            if (Keyboard.GetState().IsKeyDown(Keys.RightAlt) &&
                Keyboard.GetState().IsKeyDown(Keys.Enter)) {

                if (fullScreen == false) {
                    graphics.IsFullScreen = true;
                    fullScreen = true;
                }
                else {
                    graphics.IsFullScreen = false;
                    fullScreen = false;
                }
                graphics.ApplyChanges();
            }
        }

        public static void setGameStateMenu() {
            gameState = GameState.Menu;
        }
       
    }
    enum GameState { //Состояния игры
        Game,
        Menu,
        Options,
        Scores,
        Victory,
        Defeat
    }
}
