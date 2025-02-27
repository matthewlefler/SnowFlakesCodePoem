using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace objects;

public class Snowflakes
{
    private Snowflake[] snowflakes;

    private Vector3 wind; // + gravity
    private Random random;

    private float top = 15f;

    public Snowflakes(int num) 
    {
        random = new Random(DateTime.Now.Millisecond);
        wind = new Vector3(0.0f, -0.7f, 0.0f);

        snowflakes = new Snowflake[num];
        for (int i = 0; i < num; i++)
        {
            Snowflake flake = newSnowflake(i * (top / num) + (random.NextSingle() / 10f));
            flake.active = false;
            snowflakes[i] = flake;
        }
    }

    public Snowflakes() 
    {
        random = new Random(DateTime.Now.Millisecond);
        wind = new Vector3(0.0f, -0.7f, 0.0f);

        snowflakes = [];
    }

    /// <summary>
    /// makes the snowflakes fall 
    /// </summary>
    /// <param name="dt"> delta time in seconds </param>
    public void fall(float dt) 
    {
        for(int i = 0; i < snowflakes.Length; i++) 
        {
            Snowflake snowflake = snowflakes[i];

            snowflake.position += wind * dt;

            if(snowflake.rotationX > MathF.PI)
            {
                snowflake.rotationX -= MathF.PI;
            }
        }
    }

    /// <summary>
    /// rotates the snowflakes
    /// </summary>
    /// <param name="dt"> delta time in seconds </param>
    public void floatAndTumble(float dt)
    {
        for(int i = 0; i < snowflakes.Length; i++) 
        {
            Snowflake snowflake = snowflakes[i];

            snowflake.rotationX += snowflake.rotationdX * dt;
            snowflake.rotationY += snowflake.rotationdY * dt;

            if(snowflake.rotationX > MathF.PI)
            {
                snowflake.rotationX -= MathF.PI;
            }
        }
    }

    public void hitTheGround() {
        for(int i = 0; i < snowflakes.Length; i++) 
        {
            Snowflake snowflake = snowflakes[i];

            if(snowflake.position.Y <= 0)
            {
                Snowflake f = newSnowflake(top);
                f.active = snowflakes[i].active;
                snowflakes[i] = f;
            }
        }
    }

    public void add(Snowflake[] newSnowflakes) {
        Snowflake[] temp = new Snowflake[this.snowflakes.Length + newSnowflakes.Length];
        
        for (int i = 0; i < this.snowflakes.Length; i++)
        {
            temp[i] = this.snowflakes[i];
        }

        for (int i = 0; i < newSnowflakes.Length; i++)
        {
            temp[i + this.snowflakes.Length] = newSnowflakes[i];
        }

        this.snowflakes = temp;
    }

    public void counting(String[] names) {
        Snowflake[] temp = new Snowflake[this.snowflakes.Length + names.Length];
        
        for (int i = 0; i < this.snowflakes.Length; i++)
        {
            temp[i] = this.snowflakes[i];
        }

        for (int i = 0; i < names.Length; i++)
        {
            Snowflake f = newSnowflake(((float)i/(float)names.Length) * top);
            f.active = true;
            temp[i + this.snowflakes.Length] = f;
        }

        this.snowflakes = temp;
    }

    public void add(Snowflake snowflake) {
        Snowflake[] temp = new Snowflake[this.snowflakes.Length + 1];

        for (int i = 0; i < this.snowflakes.Length; i++)
        {
            temp[i] = this.snowflakes[i];
        }

        temp[temp.Length - 1] = snowflake;

        this.snowflakes = temp;
    }

    

    private Snowflake newSnowflake(float yPos)
    {
        Snowflake snowflake = new Snowflake(200, 200);
        
        snowflake.position = new Vector3(random.NextSingle() - 0.5f, yPos, random.NextSingle() - 0.5f);
        snowflake.rotationdX = random.NextSingle();
        snowflake.rotationdY = random.NextSingle();
        snowflake.rotationX = MathF.PI / 2f;
        snowflake.rotationY = random.NextSingle();

        return snowflake;
    }

    public void grow() 
    {
        foreach(Snowflake snowflake in snowflakes) 
        {
            if(random.NextSingle() > 0.9f) 
            {
                snowflake.tick();
            }
        }

    }

    public void draw(BasicEffect effect, GraphicsDevice device)
    {
        foreach(Snowflake snowflake in snowflakes) 
        {
            effect.World = Matrix.CreateScale(0.03f) * Matrix.CreateFromYawPitchRoll(snowflake.rotationY, snowflake.rotationX, 0f) * Matrix.CreateTranslation(snowflake.position);

            snowflake.calcVerts();
            snowflake.draw(effect, device);
        }
    }
}