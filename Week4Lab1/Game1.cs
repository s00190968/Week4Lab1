using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprites;

namespace Week4Lab1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        Viewport mainViewport;
        Viewport mapViewport;
        SpriteBatch spriteBatch;
        SimpleSprite background, redghost, blueghost, ground;
        string colMsg = "";
        SpriteFont msgFont;
        float jumpforce;

        readonly Vector2 gravity = new Vector2(0, 9.81f);

        float viewHeight, viewWidth;
        public Game1()
        {
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
            // TODO: Add your initialization logic here
            viewHeight = GraphicsDevice.Viewport.Height;
            viewWidth = GraphicsDevice.Viewport.Width;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            msgFont = Content.Load<SpriteFont>("colMsg");

            // TODO: use this.Content to load your game content here
            Texture2D bg = Content.Load<Texture2D>(@"Sprites/download");
            background = new SimpleSprite(bg, Vector2.Zero);
            Texture2D rGhost = Content.Load<Texture2D>(@"Sprites/redghost");
            redghost = new SimpleSprite(rGhost, Vector2.Zero);
            Texture2D bGhost = Content.Load<Texture2D>(@"Sprites/blueghost");
            blueghost = new SimpleSprite(bGhost, new Vector2(viewWidth / 2 - bGhost.Width / 2, viewHeight - bGhost.Height - 30f));
            Texture2D gr = Content.Load<Texture2D>(@"Sprites/ground");
            ground = new SimpleSprite(gr, new Vector2(viewWidth / 2 - gr.Width / 2, viewHeight - gr.Height));

            //viewports
            mainViewport = GraphicsDevice.Viewport;
            GraphicsDevice.Viewport = mainViewport;
            mapViewport.Bounds = new Rectangle(0, 0, mainViewport.Width / 10, mainViewport.Height / 10);
            mapViewport.X = 0;
            mapViewport.Y = 0;
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
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float speed = 5f;
            Vector2 previousPos = redghost.Position;
            Vector2 bluePrevPos = blueghost.Position;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //redghost
            if (Keyboard.GetState().IsKeyDown(Keys.A))//move right
            {
                redghost.Move(new Vector2(-1, 0) * speed);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))//move left
            {
                redghost.Move(new Vector2(1, 0) * speed);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))//move up
            {
                redghost.Move(new Vector2(0, -1) * speed);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))//move down
            {
                redghost.Move(new Vector2(0, 1) * speed);
            }
            if (!GraphicsDevice.Viewport.Bounds.Contains(redghost.BoundingRect))
            {
                redghost.Move(previousPos - redghost.Position);
            }
            //blueghost
            if (Keyboard.GetState().IsKeyDown(Keys.Right))//move right
            {
                blueghost.Move(new Vector2(1, 0) * speed);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))//move left
            {
                blueghost.Move(new Vector2(-1, 0) * speed);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))//jump up
            {
                jumpforce = 5f;
                blueghost.Move(new Vector2(blueghost.Velocity.X, blueghost.Velocity.Y = -jumpforce));
            }
            if (!GraphicsDevice.Viewport.Bounds.Contains(blueghost.BoundingRect))
            {
                blueghost.Move(bluePrevPos - blueghost.Position);
            }
            blueghost.Move(gravity * time);
            blueghost.Move(blueghost.Velocity * time);

            //collisions
            if (ground.BoundingRect.Intersects(blueghost.BoundingRect))//blue shouldn't fall through the ground
            {
                blueghost.Move(new Vector2(blueghost.Velocity.X, 0));
            }
            if (blueghost.inCollision(ground))
            {
                colMsg = "I'm touching the ground.";
            }
            else
            {
                colMsg = "We're not in collision.";
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // main
            GraphicsDevice.Viewport = mainViewport;
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            background.draw(spriteBatch);
            redghost.draw(spriteBatch);
            blueghost.draw(spriteBatch);
            ground.draw(spriteBatch);

            redghost.displayMessage(colMsg, spriteBatch, msgFont, Color.PaleVioletRed);
            blueghost.displayMessage(colMsg, spriteBatch, msgFont, Color.BlueViolet);
            spriteBatch.DrawString(msgFont, colMsg, new Vector2(blueghost.Position.X + 40f, blueghost.Position.Y + 10f), Color.AntiqueWhite);

            spriteBatch.End();

            //map
            GraphicsDevice.Viewport = mainViewport;

            spriteBatch.Begin();

            background.draw(spriteBatch);
            redghost.draw(spriteBatch);
            blueghost.draw(spriteBatch);
            ground.draw(spriteBatch);


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
