using Game1;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Maps.Tiled;
using System;
using TexturePackerLoader;

namespace Game3
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteSheet spriteSheet;
        SpriteRender spriteRender;

        private AnimationManager characterAnimationManager;
        private readonly TimeSpan timePerFrame = TimeSpan.FromSeconds(1f / 10f);

        Vector2 characterStartPosition = new Vector2(200, 200);

        bool prevKeyUpPressed = false;
        bool prevKeyLeftPressed = false;
        bool prevKeyRightPressed = false;

        TiledMap map;
        Camera2D camera;

        //Texture2D teste;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            var spriteSheetLoader = new SpriteSheetLoader(this.Content);
            this.spriteSheet = spriteSheetLoader.Load("testeDoll");

            //teste = Content.Load<Texture2D>("testeDoll");
            this.spriteRender = new SpriteRender(this.spriteBatch);

            var walkSprites = new[]{
                testedoll.Walk001,
                testedoll.Walk002,
                testedoll.Walk003,
                testedoll.Walk004
            };

            var idleSprites = new[]{
                testedoll.Idle001,
                testedoll.Idle002
            };

            var jumpSprites = new[]{
                testedoll.Jump001,
                testedoll.Jump002
            };

            var animationWalkRight = new Animation(Vector2.Zero, this.timePerFrame, SpriteEffects.FlipHorizontally, walkSprites);
            var animationWalkLeft = new Animation(Vector2.Zero, this.timePerFrame, SpriteEffects.None, walkSprites);

            var animationIdleRight = new Animation(Vector2.Zero, this.timePerFrame, SpriteEffects.FlipHorizontally, idleSprites);
            var animationIdleLeft = new Animation(Vector2.Zero, this.timePerFrame, SpriteEffects.None, idleSprites);

            var animationJumpRight = new Animation(Vector2.Zero, this.timePerFrame, SpriteEffects.FlipHorizontally, jumpSprites);
            var animationJumpLeft = new Animation(Vector2.Zero, this.timePerFrame, SpriteEffects.None, jumpSprites);

            var animations = new[] 
            { 
               animationWalkRight, animationWalkLeft, animationIdleRight, animationIdleLeft, animationJumpRight, animationJumpLeft
            };

            this.characterAnimationManager = new AnimationManager(this.spriteSheet, characterStartPosition, animations);

            characterAnimationManager.CurrentAnimation = 2;

            camera = new Camera2D(GraphicsDevice);
            map = Content.Load<TiledMap>("wood");

        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            bool KeyUpPressed = Keyboard.GetState().IsKeyDown(Keys.Up);
            bool KeyLeftPressed = Keyboard.GetState().IsKeyDown(Keys.Left);
            bool KeyRightPressed = Keyboard.GetState().IsKeyDown(Keys.Right);

            if (KeyRightPressed && !prevKeyRightPressed)
            {
                this.characterAnimationManager.CurrentAnimation = 0;
                this.characterAnimationManager.CurrentFrame = 0;
            }
            if (KeyLeftPressed && !prevKeyLeftPressed)
            {
                this.characterAnimationManager.CurrentAnimation = 1;
                this.characterAnimationManager.CurrentFrame = 0;
            }
            if (KeyUpPressed && !prevKeyUpPressed)
            {
                if (this.characterAnimationManager.CurrentAnimation.Equals(0) ||
                    this.characterAnimationManager.CurrentAnimation.Equals(2) ||
                    this.characterAnimationManager.CurrentAnimation.Equals(4))
                {
                    this.characterAnimationManager.CurrentAnimation = 4;
                    this.characterAnimationManager.CurrentFrame = 0;
                }
                else
                {
                    this.characterAnimationManager.CurrentAnimation = 5;
                    this.characterAnimationManager.CurrentFrame = 0;
                }
            }

            if ((prevKeyUpPressed || prevKeyLeftPressed || prevKeyRightPressed) && !KeyUpPressed && !KeyLeftPressed && !KeyRightPressed)
            {
                if (this.characterAnimationManager.CurrentAnimation.Equals(0) ||
                   this.characterAnimationManager.CurrentAnimation.Equals(4))
                {
                    this.characterAnimationManager.CurrentAnimation = 2;
                    this.characterAnimationManager.CurrentFrame = 0;
                }
                else
                {
                    this.characterAnimationManager.CurrentAnimation = 3;
                    this.characterAnimationManager.CurrentFrame = 0;
                }
            }

            prevKeyUpPressed = KeyUpPressed;
            prevKeyLeftPressed = KeyLeftPressed;
            prevKeyRightPressed = KeyRightPressed;

            this.characterAnimationManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            this.spriteBatch.Begin();

            spriteBatch.Draw(map, camera);

            this.spriteRender.Draw(
                this.characterAnimationManager.CurrentSprite,
                this.characterAnimationManager.CurrentPosition,
                Color.White, 0, 1,
                this.characterAnimationManager.CurrentSpriteEffects);


            //spriteBatch.Draw(teste, new Rectangle(100, 100, 100, 100), Color.White);

            this.spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
