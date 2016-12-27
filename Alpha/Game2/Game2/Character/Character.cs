using Alpha.Letters;
using Alpha.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TexturePackerLoader;
using TexturePackerMonoGameDefinitions;

namespace Alpha
{
    class Character
    {

        private Texture2D bloco;
        private Rectangle posicao;
        public Rectangle Posicao {
            set { posicao = value; }
            get {
                    return new Rectangle(posicao.X - 8, posicao.Y - 4, 16, 16);
                }  
            }

        private SpriteSheet spriteSheet;
        private SpriteRender spriteRender;
        private AnimationManager characterAnimationManager;
        private readonly TimeSpan timePerFrame = TimeSpan.FromSeconds(1f / 30f);

        private List<char> letras = new List<char> { 'A', 'B', 'E', 'F', 'Z' };
        public List<char> Letras { get { return letras; }}
        private Forma forma = null;
        public Forma Forma { get { return forma; } set { this.forma = value; } }

        private Weapon weapon;

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            //sprites
            var spriteSheetLoader = new SpriteSheetLoader(content);
            this.spriteSheet = spriteSheetLoader.Load("alphaBoySpriteSheet");
            this.spriteRender = new SpriteRender(spriteBatch);

            String[] idleFrontSprites = new String[] {
                alphaBoySpriteSheet.WalkFront3
            };
            String[] walkFrontSprites = new String[] {
                alphaBoySpriteSheet.WalkFront0,
                alphaBoySpriteSheet.WalkFront1,
                alphaBoySpriteSheet.WalkFront2,
                alphaBoySpriteSheet.WalkFront3,
                alphaBoySpriteSheet.WalkFront5,
                alphaBoySpriteSheet.WalkFront6,
                //alphaBoySpriteSheet.WalkFront7,
                //alphaBoySpriteSheet.WalkFront8,
                //alphaBoySpriteSheet.WalkFront9,

                //alphaBoySpriteSheet.WalkFront8,
                //alphaBoySpriteSheet.WalkFront7,
                alphaBoySpriteSheet.WalkFront6,
                alphaBoySpriteSheet.WalkFront5,
                alphaBoySpriteSheet.WalkFront3,
                alphaBoySpriteSheet.WalkFront2,
                alphaBoySpriteSheet.WalkFront1

            };

             String[] walkBackSprites = new String[] {
                //alphaBoySpriteSheet.WalkBack0,
                alphaBoySpriteSheet.WalkBack1,
                alphaBoySpriteSheet.WalkBack2,
                alphaBoySpriteSheet.WalkBack3,
                alphaBoySpriteSheet.WalkBack4,
                alphaBoySpriteSheet.WalkBack5,
                alphaBoySpriteSheet.WalkBack6,
                //alphaBoySpriteSheet.WalkBack7,
                //alphaBoySpriteSheet.WalkBack8,
                alphaBoySpriteSheet.WalkBack7,
                alphaBoySpriteSheet.WalkBack6,
                alphaBoySpriteSheet.WalkBack5,
                alphaBoySpriteSheet.WalkBack4,
                alphaBoySpriteSheet.WalkBack3,
                alphaBoySpriteSheet.WalkBack2,
                alphaBoySpriteSheet.WalkBack1

            };

            String[] walkLeftSprites = new String[] {
                alphaBoySpriteSheet.WalkLeft8,
                alphaBoySpriteSheet.WalkLeft7,
                alphaBoySpriteSheet.WalkLeft6,
                alphaBoySpriteSheet.WalkLeft5,
                alphaBoySpriteSheet.WalkLeft4,
                alphaBoySpriteSheet.WalkLeft3,
                alphaBoySpriteSheet.WalkLeft2,
                alphaBoySpriteSheet.WalkLeft1,
                
                alphaBoySpriteSheet.WalkLeft0,
                alphaBoySpriteSheet.WalkLeft1,
                alphaBoySpriteSheet.WalkLeft2,
                alphaBoySpriteSheet.WalkLeft3,
                alphaBoySpriteSheet.WalkLeft4,
                alphaBoySpriteSheet.WalkLeft5,
                alphaBoySpriteSheet.WalkLeft6,
                alphaBoySpriteSheet.WalkLeft7


            };

            String[] walkRightSprites = new String[] {
                alphaBoySpriteSheet.WalkRight8,
                alphaBoySpriteSheet.WalkRight7,
                alphaBoySpriteSheet.WalkRight6,
                alphaBoySpriteSheet.WalkRight5,
                alphaBoySpriteSheet.WalkRight4,
                alphaBoySpriteSheet.WalkRight3,
                alphaBoySpriteSheet.WalkRight2,
                alphaBoySpriteSheet.WalkRight1,
                
                alphaBoySpriteSheet.WalkRight0,
                alphaBoySpriteSheet.WalkRight1,
                alphaBoySpriteSheet.WalkRight2,
                alphaBoySpriteSheet.WalkRight3,
                alphaBoySpriteSheet.WalkRight4,
                alphaBoySpriteSheet.WalkRight5,
                alphaBoySpriteSheet.WalkRight6,
                alphaBoySpriteSheet.WalkRight7

            };

            Animation animationIdleFront = new Animation(Vector2.Zero, this.timePerFrame, SpriteEffects.None, idleFrontSprites);
            Animation animationWalkFront = new Animation(Vector2.Zero, this.timePerFrame, SpriteEffects.None, walkFrontSprites);
            Animation animationWalkBack = new Animation(Vector2.Zero, this.timePerFrame, SpriteEffects.None, walkBackSprites);
            Animation animationWalkLeft = new Animation(Vector2.Zero, this.timePerFrame, SpriteEffects.None, walkRightSprites);
            Animation animationWalkRight = new Animation(Vector2.Zero, this.timePerFrame, SpriteEffects.None, walkLeftSprites);

            Animation[] animations = new Animation[] {
               animationIdleFront, animationWalkFront, animationWalkBack, animationWalkLeft, animationWalkRight
            };

            this.characterAnimationManager = new AnimationManager(this.spriteSheet, new Vector2(posicao.X, posicao.Y), animations);

            this.characterAnimationManager.CurrentAnimation = 0;
            this.characterAnimationManager.CurrentFrame = 0;

            bloco = content.Load<Texture2D>("bloco");

            weapon = new Axe();

            weapon.setSprite(content.Load<Texture2D>("axe_defalult"));
        }

        private int velocidade = 1;

        private Vector2 getTelaAtual(Rectangle posicao)
        {
            return new Vector2(posicao.X / 256, posicao.Y / 176);
        }
        public void setIdleAnimation()
        {
            this.characterAnimationManager.CurrentAnimation = 0;
            this.characterAnimationManager.CurrentFrame = 0;
        }
        public void moveUp()
        {
            posicao.Y -= velocidade;
            this.characterAnimationManager.CurrentAnimation = 2;
            this.weapon.setUp();
        }
        public void moveDown()
        {
            posicao.Y += velocidade;
            this.characterAnimationManager.CurrentAnimation = 1;
            this.weapon.setDown();
        }
        public void moveLeft()
        {
            posicao.X -= velocidade;
            this.characterAnimationManager.CurrentAnimation = 3;
            this.weapon.setLeft();
        }
        public void moveRight()
        {
            posicao.X += velocidade;
            this.characterAnimationManager.CurrentAnimation = 4;
            this.weapon.setRight();
        }

        bool qPressP = false;
        bool wPressP = false;
        bool aPressP = false;
        bool sPressP = false;

        public void Update(GameTime gameTime)
        {
            this.weapon.Update();
            this.weapon.setPosicao(posicao);
            
            bool qPress = Keyboard.GetState().IsKeyDown(Keys.Q);
            bool wPress = Keyboard.GetState().IsKeyDown(Keys.W);

            if (qPress && !qPressP)
            {
                char l = letras[letras.Count -1];

                List<char> letrasN = new List<char>();
                letrasN.Add(l);

                for (int i = 0; i < letras.Count - 1; i++)
                {
                    letrasN.Add(letras[i]);
                }

                letras = letrasN;
            }

            qPressP = qPress;


            if (wPress && !wPressP)
            {
                char l = letras[0];

                List<char> letrasN = new List<char>();
                for (int i = 1; i < letras.Count; i++)
                {
                    letrasN.Add(letras[i]);
                }
                letrasN.Add(l);

                letras = letrasN;
            }

            wPressP = wPress;

            bool aPress = Keyboard.GetState().IsKeyDown(Keys.A);

            if (aPress && !aPressP)
            {
                if (forma != null && letras.Count > 0)
                {
                    forma.putLetter(letras[0]);
                    letras.Remove(letras[0]);
                }
            }

            aPressP = aPress;
            
            //forja
            bool sPress = Keyboard.GetState().IsKeyDown(Keys.S);

            if (sPress && !sPressP)
            {
                if (forma != null)
                {
                    forma.forge();
                }
            }

            sPressP = sPress;

            this.characterAnimationManager.CurrentPosition = new Vector2(posicao.X, posicao.Y);
            this.characterAnimationManager.Update(gameTime);
        }
        public void atk()
        {
            this.weapon.Atk();
        }

        public List<Rectangle> getAtk()
        {
            return this.weapon.getAtk();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(bloco, posicao, Color.Blue);

            this.spriteRender.Draw(
                this.characterAnimationManager.CurrentSprite,
                this.characterAnimationManager.CurrentPosition,
                Color.White, 0, 0.12f,
                this.characterAnimationManager.CurrentSpriteEffects);

            weapon.Draw(spriteBatch);
            //spriteBatch.Draw(bloco, new Rectangle(posicao.X + 8, posicao.Y + 8, 10, 8), null, Color.Red, rotacao, new Vector2(origem.X, origem.Y + 16), SpriteEffects.None, 1f);
        }
    }
}
