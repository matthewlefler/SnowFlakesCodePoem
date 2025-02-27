using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using Cameras;
using objects;

namespace SnowFlakesCodePoem;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private int screen_x;
    private int screen_y;
    Point screen_middle;

    VertexPositionColorNormal[] origin_axii_lines = new VertexPositionColorNormal[]
    {
        new VertexPositionColorNormal(Vector3.Up, Color.Red, Vector3.One),
        new VertexPositionColorNormal(Vector3.Zero, Color.Red, Vector3.One),
        
        new VertexPositionColorNormal(Vector3.Right, Color.Green, Vector3.One),
        new VertexPositionColorNormal(Vector3.Zero, Color.Green, Vector3.One),
        
        new VertexPositionColorNormal(Vector3.Forward, Color.Blue, Vector3.One),
        new VertexPositionColorNormal(Vector3.Zero, Color.Blue, Vector3.One),
    };

    private const float top = 10f;
    VertexPositionColorNormal[] box = new VertexPositionColorNormal[]
    {
        // bottom square
        new VertexPositionColorNormal(new Vector3(-0.5f,0f,-0.5f), Color.White, Vector3.One),
        new VertexPositionColorNormal(new Vector3(0.5f,0f,-0.5f), Color.White, Vector3.One),
        
        new VertexPositionColorNormal(new Vector3(0.5f,0f,-0.5f), Color.White, Vector3.One),
        new VertexPositionColorNormal(new Vector3(0.5f,0f,0.5f), Color.White, Vector3.One),
        
        new VertexPositionColorNormal(new Vector3(0.5f,0f,0.5f), Color.White, Vector3.One),
        new VertexPositionColorNormal(new Vector3(-0.5f,0f,0.5f), Color.White, Vector3.One),

        new VertexPositionColorNormal(new Vector3(-0.5f,0f,0.5f), Color.White, Vector3.One),
        new VertexPositionColorNormal(new Vector3(-0.5f,0f,-0.5f), Color.White, Vector3.One),

        // vertical lines
        new VertexPositionColorNormal(new Vector3(-0.5f,0f,-0.5f), Color.White, Vector3.One),
        new VertexPositionColorNormal(new Vector3(-0.5f,top,-0.5f), Color.White, Vector3.One),
        
        new VertexPositionColorNormal(new Vector3(0.5f,0f,-0.5f), Color.White, Vector3.One),
        new VertexPositionColorNormal(new Vector3(0.5f,top,-0.5f), Color.White, Vector3.One),

        new VertexPositionColorNormal(new Vector3(-0.5f,0f,0.5f), Color.White, Vector3.One),
        new VertexPositionColorNormal(new Vector3(-0.5f,top,0.5f), Color.White, Vector3.One),

        new VertexPositionColorNormal(new Vector3(0.5f,0f,0.5f), Color.White, Vector3.One),
        new VertexPositionColorNormal(new Vector3(0.5f,top,0.5f), Color.White, Vector3.One),

        // top square
        new VertexPositionColorNormal(new Vector3(-0.5f,top,-0.5f), Color.White, Vector3.One),
        new VertexPositionColorNormal(new Vector3(0.5f,top,-0.5f), Color.White, Vector3.One),
        
        new VertexPositionColorNormal(new Vector3(0.5f,top,-0.5f), Color.White, Vector3.One),
        new VertexPositionColorNormal(new Vector3(0.5f,top,0.5f), Color.White, Vector3.One),
        
        new VertexPositionColorNormal(new Vector3(0.5f,top,0.5f), Color.White, Vector3.One),
        new VertexPositionColorNormal(new Vector3(-0.5f,top,0.5f), Color.White, Vector3.One),

        new VertexPositionColorNormal(new Vector3(-0.5f,top,0.5f), Color.White, Vector3.One),
        new VertexPositionColorNormal(new Vector3(-0.5f,top,-0.5f), Color.White, Vector3.One),
    };

    private SpriteFont font;

    private SimpleCamera camera;

    private Snowflakes snowflakes = new Snowflakes(20);

    private const string snow = " ftwzdcbae";
    private Random random = new Random(0);

    private BasicEffect basicEffect;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferHeight = 1300;
        _graphics.PreferredBackBufferWidth = 1300;
        _graphics.ApplyChanges();
        
        screen_x = _graphics.PreferredBackBufferWidth;
        screen_y = _graphics.PreferredBackBufferHeight;
    
        screen_middle = new Point(screen_x / 2, screen_y / 2);

        Console.WriteLine("screen width: " + screen_x + " screen height: " + screen_y);

        // TODO: Add your initialization logic here

        camera = new SimpleCamera(Vector3.Backward * 3);

        textHeight = screen_y/pixelsPerChar;
        textWidth = screen_x/pixelsPerChar;
        screenText = new string[textHeight * textWidth];

        basicEffect = new BasicEffect(GraphicsDevice); // basic effect
        // primitive color
        basicEffect.AmbientLightColor = new Vector3(0.1f, 0.1f, 0.1f);
        basicEffect.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
        basicEffect.SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
        basicEffect.SpecularPower = 5.0f;
        basicEffect.Alpha = 1.0f;
        // The following MUST be enabled if you want to color your vertices
        basicEffect.VertexColorEnabled = true;

        basicEffect.EnableDefaultLighting();

        base.Initialize();
        Console.WriteLine("Initialization done");
    }




    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        font = Content.Load<SpriteFont>("File");
    }




    float dt = 0; // delta time in seconds

    bool nDown = false;

    MouseState last_mouse = Mouse.GetState();
    MouseState mouse = Mouse.GetState();
    KeyboardState last_keyboard = Keyboard.GetState();
    KeyboardState keyboard = Keyboard.GetState();
    protected override void Update(GameTime gameTime)
    {
        dt = (float) gameTime.ElapsedGameTime.TotalSeconds;

        last_keyboard = keyboard;
        keyboard = Keyboard.GetState();
        last_mouse = mouse;
        mouse = Mouse.GetState();

        Mouse.SetPosition(screen_middle.X, screen_middle.Y); // lock mouse to screen

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Escape))
        { Exit(); }

        if(keyboard.IsKeyDown(Keys.W))
        {
            camera.Move(Vector3.Forward * dt);
        }
        if(keyboard.IsKeyDown(Keys.S))
        {
            camera.Move(Vector3.Backward * dt);
        }

        if(keyboard.IsKeyDown(Keys.A))
        {
            camera.Move(Vector3.Left * dt);
        }
        if(keyboard.IsKeyDown(Keys.D))
        {
            camera.Move(Vector3.Right * dt);
        }

        if(keyboard.IsKeyDown(Keys.Q))
        {
            camera.Move(Vector3.Down * dt);
        }
        if(keyboard.IsKeyDown(Keys.E))
        {
            camera.Move(Vector3.Up * dt);
        }

        if(keyboard.IsKeyDown(Keys.LeftShift))
        {
            camera.speed = 10f;
        }
        else
        {
            camera.speed = 1f;
        }

        camera.Rotate((screen_middle.X - mouse.X) * dt * 0.2f, (screen_middle.Y - mouse.Y) * dt * 0.2f);

        snowflakes.falling(dt);
        snowflakes.growing();

        base.Update(gameTime);
    }



    Color background_color = new Color(0.0f,0.0f,0.0f);
    string[] screenText;
    int pixelsPerChar = 5;
    int textHeight;
    int textWidth;
    protected override void Draw(GameTime gameTime)
    {
        double frame_rate = 1.0 / gameTime.ElapsedGameTime.TotalSeconds;
        GraphicsDevice.Clear(background_color);

        GraphicsDevice.DepthStencilState = DepthStencilState.Default;

        RasterizerState rs = new RasterizerState();
        rs.CullMode = CullMode.None;
        GraphicsDevice.RasterizerState = rs;

        _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

        basicEffect.View = camera.view_matrix; //camera projections
        basicEffect.Projection = camera.projection_matrix;

        snowflakes.draw(basicEffect, GraphicsDevice);
        
        setScreenText();

        GraphicsDevice.Clear(background_color);

        for (int x = 0; x < screen_x - pixelsPerChar; x+=pixelsPerChar)
        {
            for (int y = 0; y < screen_y - pixelsPerChar; y+=pixelsPerChar)
            {
                _spriteBatch.DrawString(font, screenText[x/pixelsPerChar + y/pixelsPerChar * textWidth], new Vector2(x, y), random.NextDouble() > 0.98f ? Color.White : Color.PowderBlue, 0, Vector2.Zero, scale: 0.3f, SpriteEffects.None, 0.0f);
            }
        }

        basicEffect.World = Matrix.Identity;
        foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();

            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, origin_axii_lines, 0, 3);
        }
        
        basicEffect.World = Matrix.Identity;
        foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();

            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, box, 0, box.Length / 2);
        }

        Color frameRateColor = Color.Green;
        if(frame_rate < 50) { frameRateColor = Color.Yellow; }
        if(frame_rate < 40) { frameRateColor = Color.Red; }
        _spriteBatch.DrawString(font, frame_rate.ToString(), Vector2.One, frameRateColor, 0, Vector2.One, 0.8f, SpriteEffects.None, 0.0f);

        _spriteBatch.End();
        base.Draw(gameTime);
    }

    private void setScreenText() 
    {
        Color[] screenColors = new Color[screen_x * screen_y];
        
        GraphicsDevice.GetBackBufferData<Color>(screenColors);
        
        float brightness;

        for (int x = 0; x < screen_x - pixelsPerChar; x+=pixelsPerChar)
        {
            for (int y = 0; y < screen_y - pixelsPerChar; y+=pixelsPerChar)
            {
                brightness = screenColors[x + y * screen_x].R;
                //brightness = avgBrightness(x, x + pixelsPerChar - 1, y, y + pixelsPerChar - 1, screenColors);

                if(brightness > 255f) { brightness = 255f;}
                if(brightness < 0f) { brightness = 0f;}

                brightness *= (float) (snow.Length - 1) / 255f;

                if(brightness > snow.Length - 1) { brightness = snow.Length - 1; }
                if(brightness < 0) { brightness = 0; }




                screenText[x/pixelsPerChar + y/pixelsPerChar * textWidth] = snow[(int) brightness].ToString();
            }
        }
    }

    private float avgBrightness(int minX, int maxX, int minY, int maxY, Color[] screen)
    {
        float r = 0;
        float g = 0;
        float b = 0;
        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                Color color = screen[x + y * screen_x];
                r += color.R;
                g += color.G;
                b += color.B;
            }
        }

        int count = (maxX - minX) * (maxY - minY);

        r = r / count;
        g = g / count;
        b = b / count;

        return (r + g + b) / 3.0f;
    }

    private float maxBrightness(int minX, int maxX, int minY, int maxY, Color[] screen)
    {
        float r = 0;
        float g = 0;
        float b = 0;
        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                Color color = screen[x + y * screen_x];
                if(r < color.R) {
                    r = color.R;
                }

                if(g < color.G) {
                    g = color.G;
                }

                if(b < color.B) {
                    b = color.B;
                }
            }
        }

        return (r + g + b) / 3.0f;
    }
}
