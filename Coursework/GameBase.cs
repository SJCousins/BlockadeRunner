using Coursework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace Coursework
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameBase : Game
    {

        public enum gameStates { mainMenu, Playing } //possible game states
        private gameStates state; //current state

        public enum level { one,transitionOne, two, transitionTwo, three, end } //possible levels/transitions
        public static level currentLevel; //current level

        Background background = new Background();
        menuMain MainMenu = new menuMain();

        public static Rectangle playerHitbox;
        public Vector2 PlayerPos;

        Vector2 origin = new Vector2(0, 0);
        int windowWidth = 600;
        int windowHeight = 800;




        public static GameBase Instance { get; private set; }
        public static Texture2D defaultBullet { get; private set; }
        public static Texture2D ship { get; private set; }
        public static Texture2D eShot { get; private set; }
        public static Texture2D eShotBeam { get; private set; }
        public static Texture2D enemyShot { get; private set; }
        public static Texture2D eShotEpicenter { get; private set; }
        public static Texture2D x1 { get; private set; }
        public static Texture2D dualShot { get; private set; }
        public static Texture2D dualShot2 { get; private set; }
        public static Texture2D seekerMine { get; private set; }
        public static Texture2D Boss { get; private set; }
        public static Texture2D BossHurt { get; private set; }
        public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
        public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }
        private static readonly Random rand = new Random();
        private static SoundEffect[] explosions;
        public static SoundEffect Explosion { get { return explosions[rand.Next(explosions.Length)]; } }
        public static SoundEffect eShotLaunch,baseShotLaunch,enemyShotLaunch;


        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public GameBase()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            state = gameStates.mainMenu;


            background.loadContent(Content);
            UserInterface.loadContent(Content);
            MainMenu.loadContent(Content);


            base.Initialize();
            background.Initialize();
            MainMenu.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ship = (Content.Load<Texture2D>("ship"));
            enemyShot = (Content.Load<Texture2D>("enemyShot"));
            defaultBullet = Content.Load<Texture2D>("Blast");
            eShot = Content.Load<Texture2D>("eShot");
            eShotBeam = Content.Load<Texture2D>("eBeam");
            eShotEpicenter = Content.Load<Texture2D>("eShotEpicenter");
            dualShot = Content.Load<Texture2D>("Enemy0");
            dualShot2 = Content.Load<Texture2D>("EnemyHurt");
            Boss = Content.Load<Texture2D>("Boss");
            BossHurt = Content.Load<Texture2D>("BossHurt");
            seekerMine = Content.Load<Texture2D>("SeekerMine");
            x1 = Content.Load<Texture2D>("1x1");
            explosions = Enumerable.Range(1, 8).Select(x => Content.Load<SoundEffect>("Sound/explosion-0" + x)).ToArray();
            baseShotLaunch = Content.Load<SoundEffect>("Sound/BaseShotLaunch");
            eShotLaunch = Content.Load<SoundEffect>("Sound/eShotLaunch");
            enemyShotLaunch = Content.Load<SoundEffect>("Sound/enemyShoot");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            if (state == gameStates.mainMenu) //display menu when gamestate is menu
            {
                this.IsMouseVisible = true; //mouse needs to be seen to click buttons
                MainMenu.Update();
            }

            if (state == gameStates.Playing) //stop displaying menu and begin game loop
            {
                this.IsMouseVisible = false; //mouse not needed anymore
                PlayerInput.Update();
                EntityManager.Add(Player.Instance);
                background.Update();
                UserInterface.Update(gameTime);
                EnemySpawner.Update();
                EntityManager.Update();
                base.Update(gameTime);
            }
        }

        public static void setState(gameStates inputState) //allows other classes to change game state
        {
            Instance.state = inputState;
        }
        public static void setLevel(level inputLevel) //allows other classes to change current level
        {
          currentLevel = inputLevel;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            
            if (state == gameStates.mainMenu)
            {
                spriteBatch.Begin();
                MainMenu.Draw(spriteBatch);
                spriteBatch.End();
            }

            if (state == gameStates.Playing)
            {
                spriteBatch.Begin();
                background.Draw(spriteBatch);
                UserInterface.Draw(spriteBatch);
                EntityManager.Draw(spriteBatch);
                spriteBatch.End();
            }

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
