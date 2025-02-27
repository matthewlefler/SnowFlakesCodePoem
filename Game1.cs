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

    private SpriteFont font;

    private SimpleCamera camera;

    private Snowflake snowflake;
    private float limit = 1.0f;

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

        snowflake = new Snowflake(400, 400, 4.8448797E-05f, 0.65644354f, 1.6405386f, Vector3.Zero, Quaternion.Identity);

        int times = 100;
        for (int j = 0; j < times; ++j)
        {
            for(int i = 0; i < 10; ++i) {
                snowflake.tick();
            }
            if(snowflake.filled()) { break; }
        }
        snowflake.calcVerts();

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

        if(keyboard.IsKeyDown(Keys.OemOpenBrackets))
        {
            limit += 0.01f;
            snowflake.calcVerts(limit);
        }
        if(keyboard.IsKeyDown(Keys.OemCloseBrackets))
        {
            limit -= 0.01f;
            if(limit < 0f) { limit = 0f; }
            snowflake.calcVerts(limit);
        }

        camera.Rotate((screen_middle.X - mouse.X) * dt * 0.2f, (screen_middle.Y - mouse.Y) * dt * 0.2f);

        // TODO: Add your update logic here
        if(keyboard.IsKeyDown(Keys.Space))
        {
            snowflake.tick();
            snowflake.calcVerts(limit);
        }

        if(keyboard.IsKeyDown(Keys.N)) {
            if(!nDown) {
                snowflake = new Snowflake(400, 400);
                snowflake.calcVerts();
            }
            nDown = true;
        }
        else
        {
            nDown = false;
        }
    
        snowflake.rotation *= Quaternion.CreateFromAxisAngle(Vector3.Up, dt * 20f);

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
        basicEffect.World = Matrix.Identity;

        foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();

            GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, origin_axii_lines, 0, 3);
        }

        basicEffect.World = Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(Vector3.Forward * 4) * Matrix.CreateFromQuaternion(snowflake.rotation);
        snowflake.draw(basicEffect, GraphicsDevice);
        
        setScreenText();

        GraphicsDevice.Clear(background_color);

        for (int x = 0; x < screen_x - pixelsPerChar; x+=pixelsPerChar)
        {
            for (int y = 0; y < screen_y - pixelsPerChar; y+=pixelsPerChar)
            {
                _spriteBatch.DrawString(font, screenText[x/pixelsPerChar + y/pixelsPerChar * textWidth], new Vector2(x, y), random.NextDouble() > 0.98f ? Color.White : Color.PowderBlue, 0, Vector2.Zero, scale: 0.3f, SpriteEffects.None, 0.0f);
            }
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
        
        for (int x = 0; x < screen_x - pixelsPerChar; x+=pixelsPerChar)
        {
            for (int y = 0; y < screen_y - pixelsPerChar; y+=pixelsPerChar)
            {
                float brightness = screenColors[x + y * screen_x].R; // maxBrightness(x, x + pixelsPerChar - 1, y, y + pixelsPerChar - 1, screenColors);

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
