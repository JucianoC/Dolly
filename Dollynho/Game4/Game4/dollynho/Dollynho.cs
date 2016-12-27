using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexturePackerLoader;

namespace Game4.dollynho
{
    class Dollynho
    {
        private Texture2D bloco;

        private SpriteSheet spriteSheet;
        private SpriteRender spriteRender;

        private AnimationManager characterAnimationManager;
        private readonly TimeSpan timePerFrame = TimeSpan.FromSeconds(1f / 10f);

        private Rectangle posicao;
        public Rectangle Posicao
        {
            set { posicao = value; }
            get
            {
                //return new Rectangle(posicao.X - 8, posicao.Y - 4, 16, 16);
                return posicao;
            }
        }

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            bloco = content.Load<Texture2D>("bloco");

            var spriteSheetLoader = new SpriteSheetLoader(content);
            this.spriteSheet = spriteSheetLoader.Load("testeDoll");

            //teste = Content.Load<Texture2D>("testeDoll");
            this.spriteRender = new SpriteRender(spriteBatch);

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

            this.characterAnimationManager = new AnimationManager(this.spriteSheet, new Vector2(posicao.X, posicao.Y), animations);

            characterAnimationManager.CurrentAnimation = 2;
        }

        public void moveLeft()
        {
            this.characterAnimationManager.CurrentAnimation = 1;
            this.posicao = new Rectangle(this.posicao.X - 1, this.posicao.Y, 16, 16);
        }
        public void moveRight()
        {
            this.characterAnimationManager.CurrentAnimation = 0;
            this.posicao = new Rectangle(this.posicao.X + 1, this.posicao.Y, 16, 16);
        }

        public void Update(GameTime gameTime)
        {
            this.characterAnimationManager.Update(gameTime);
            this.characterAnimationManager.CurrentPosition = new Vector2(posicao.X, posicao.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            this.spriteRender.Draw(
                this.characterAnimationManager.CurrentSprite,
                this.characterAnimationManager.CurrentPosition,
                Color.White, 0, 1,
                this.characterAnimationManager.CurrentSpriteEffects);
        }

    }
}
