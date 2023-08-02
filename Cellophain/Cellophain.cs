using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using Bitmap = System.Drawing.Bitmap;
using SColor = System.Drawing.Color;
using System.Collections;

namespace Cellophain
{
    public class Cellophain : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Random rand = new Random();
        private InputHandler input = new InputHandler();
        private Iterator iterator;

        private Element[,] world;
        private List<Element> activeElements;
        private int primaryElement;
        private int secondaryElement;
        private bool primarySelected;
        private bool secondarySelected;
        private int priorityType;
        private int gridSize;
        private int cellSize;
        private bool highlightVisible;
        private int mouseX;
        private int mouseY;
        private bool paused;
        private int brushSize;
        private bool precisionBrush;
        private int imageSuffix = 0;



        Texture2D square;
        Texture2D highlight;

        public Cellophain()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = false;
        }

        protected override void Initialize()
        {
            //Keep gridsize to a factor of 600, the world itself doesn't scale
            gridSize = 50;
            cellSize = 600 / gridSize;

            //priorityType determines how conflicts in requests are handled
            //0: standard array traversal, no extra performance cost, use if order of request execution is irrelevant (cells only affect themselves)
            //1: randomized array traversal, minor performance cost, use if all requests are same priority
            //2: full priority system, large performance cost, use if some requests are higher priority than others

            priorityType = 0;
            iterator = new Iterator(gridSize, priorityType);

            //Add whatever element you want to be the background as the first in the list
            activeElements = new List<Element>
            {
                new RPS(rand.Next(256), rand.Next(256), rand.Next(256), 0, 3),
                new RPS(rand.Next(256), rand.Next(256), rand.Next(256), 1, 3),
                new RPS(rand.Next(256), rand.Next(256), rand.Next(256), 2, 3)
            };

            RPS.CalculateWinners(3);
            world = new Element[gridSize, gridSize];
            CreateWorld();

            if(activeElements.Count > 1)
            {
                primaryElement = 1;
                secondaryElement = 0;
            }
            else
            {
                primaryElement = 0;
            }

            paused = true;
            brushSize = 0;

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            square = Content.Load<Texture2D>("whiteSquare");
            highlight = Content.Load<Texture2D>("transWhiteSquare");
        }

        protected override void Update(GameTime gameTime)
        {
            input.Update();
            primarySelected = false;
            secondarySelected = false;
            HandleInputs();

            if (Mouse.GetState().X > 340 && Mouse.GetState().X < 940 && Mouse.GetState().Y > 60 && Mouse.GetState().Y < 660)
            {
                highlightVisible = true;
                mouseX = (Mouse.GetState().X - 340) / cellSize;
                mouseY = (Mouse.GetState().Y - 60) / cellSize;
            }
            else
            {
                highlightVisible = false;
            }

            if (paused)
            {
                if (input.KeyPressed(Keys.OemPeriod))
                {
                    world = iterator.Iterate(world);
                }
            }
            else
            {
                world = iterator.Iterate(world);
            }


            base.Update(gameTime);
        }

        public void CreateWorld()
        {
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    Element placed = (Element)activeElements[rand.Next(activeElements.Count)].DeepCopy();
                    placed.SetLocation(new Point(x, y));
                    world[x, y] = placed;
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.MidnightBlue);
            _spriteBatch.Begin();

            //UI and background drawing
            _spriteBatch.Draw(square, new Rectangle(new Point(330, 50), new Point(620, 620)), Color.DarkSlateBlue);
            _spriteBatch.Draw(square, new Rectangle(new Point(40, 50), new Point(70, 10 + activeElements.Count * 60)), Color.DarkSlateBlue);
            for (int i = 0; i < activeElements.Count; i++)
            {
                _spriteBatch.Draw(square, new Rectangle(new Point(60, 70 + 60 * i), new Point(30, 30)), activeElements[i].GetColor());
            }

            if (primarySelected || secondarySelected)
            {
                _spriteBatch.Draw(square, new Rectangle(new Point(60, 70 + 60 * primaryElement), new Point(30, 30)), new Color(100,100,100,100));
            }

            //Draw the cells
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    Rectangle squarePos = new Rectangle(new Point((x * cellSize) + 340, (y * cellSize) + 60), new Point(cellSize, cellSize));
                    _spriteBatch.Draw(square, squarePos, world[x,y].GetColor());
                }
            }

            //Draw the highlight from the cursor
            if (highlightVisible && Mouse.GetState().LeftButton == ButtonState.Released && Mouse.GetState().RightButton == ButtonState.Released)
            {
                for (int x = -1 * brushSize; x <= brushSize; x++)
                {
                    for (int y = -1 * brushSize; y <= brushSize; y++)
                    {
                        if (mouseX + x >= 0 && mouseX + x < world.GetLength(0) && mouseY + y >= 0 && mouseY + y < world.GetLength(0))
                        {
                            Rectangle highlightPos = new Rectangle(new Point(((mouseX + x) * cellSize) + 340, ((mouseY + y) * cellSize) + 60), new Point(cellSize, cellSize));
                            _spriteBatch.Draw(highlight, highlightPos, Color.White);
                        }
                    }
                }
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        private void placeCells(int element)
        {
            for (int x = -1 * brushSize; x <= brushSize; x++)
            {
                for (int y = -1 * brushSize; y <= brushSize; y++)
                {
                    if (mouseX + x >= 0 && mouseX + x < world.GetLength(0) && mouseY + y >= 0 && mouseY + y < world.GetLength(0))
                    {
                        Element placed = (Element) activeElements[element].DeepCopy();
                        placed.SetLocation(new Point (mouseX + x, mouseY + y));
                        world[mouseX + x, mouseY + y] = placed;
                    }
                }
            }
        }

        private void SaveImage()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Bitmap img = new Bitmap(600, 600);

                for (int x = 0; x < 600; x++)
                {
                    for (int y = 0; y < 600; y++)
                    {
                        img.SetPixel(x, y, ConvertColor(world[x/cellSize, y/cellSize].GetColor()));
                    }
                }

                while (File.Exists(@"ScreenCap" + imageSuffix + ".png"))
                {
                    imageSuffix++;
                }
                img.Save(@"ScreenCap" + imageSuffix + ".png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        private SColor ConvertColor(Color col)
        {
            SColor newCol =  SColor.FromArgb(col.A, col.R, col.G, col.B);
            return newCol;
        }

        private void HandleInputs()
        {
            //Reset grid bind
            if (input.KeyPressed(Keys.Escape))
            {
                CreateWorld();
            }

            //Re-initialize (useful for re-rolling random colours)
            if (input.KeyPressed(Keys.R))
            {
                Initialize();
            }

            //Pause the iterations
            if (input.KeyPressed(Keys.Space))
            {
                paused = !paused;
            }

            //Do a single iteration

            //Toggle precision brush
            if (input.KeyPressed(Keys.P))
            {
                precisionBrush = !precisionBrush;
            }

            //Change brush size
            if (input.MWheelUp())
            {
                brushSize++;
            }
            if (input.MWheelDown() && brushSize > 0)
            {
                brushSize--;
            }

            //Drawing
            if (precisionBrush)
            {
                if (input.LeftClick() && highlightVisible)
                {
                    placeCells(primaryElement);
                }
                if (input.RightClick() && highlightVisible)
                {
                    placeCells(secondaryElement);
                }
            }
            else
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && highlightVisible)
                {
                    placeCells(primaryElement);
                }
                if (Mouse.GetState().RightButton == ButtonState.Pressed && highlightVisible)
                {
                    placeCells(secondaryElement);
                }
            }

            //Selecting a cell type from menu
            for (int i = 0; i < activeElements.Count; i++)
            {
                if (Mouse.GetState().X > 60 && Mouse.GetState().X < 90 && Mouse.GetState().Y > 70 + (60 * i) && Mouse.GetState().Y < 100 + (60 * i))
                {
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        primaryElement = i;
                        primarySelected = true;
                    }
                    if (Mouse.GetState().RightButton == ButtonState.Pressed)
                    {
                        secondaryElement = i;
                        secondarySelected = true;
                    }
                }
            }

            if (input.KeyPressed(Keys.F12))
            {
                SaveImage();
            }
        }
    }
}
