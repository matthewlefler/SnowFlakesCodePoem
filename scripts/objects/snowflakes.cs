using System;
using System.Numerics;
using Microsoft.Xna.Framework.Graphics;

namespace objects;

class Snowflakes
{
    private Snowflake[] snowflakes;

    private Vector3 wind; // + gravity
    private Random random;

    public Snowflakes(int num) 
    {
        random = new Random(DateTime.Now.Millisecond);
        wind = new Vector3(random.NextSingle() - 0.5f, -1f * random.NextSingle(), random.NextSingle() - 0.5f);

        snowflakes = new Snowflake[num];
        for (int i = 0; i < num; i++)
        {
            snowflakes[i] = new Snowflake(300, 300);
        }
    }

    public void falling(float dt) 
    {
        foreach(Snowflake snowflake in snowflakes) 
        {
            snowflake.position += wind * dt * 0.1f;
            
            snowflake.rotationX += random.NextSingle() * dt;
            snowflake.rotationY += random.NextSingle() * dt;
        }
    }

    public void growing() 
    {
        foreach(Snowflake snowflake in snowflakes) 
        {
            if(random.NextSingle() > 0.5f) 
            {
                snowflake.tick();
            }
        }

    }

    public void draw(BasicEffect effect, GraphicsDevice device)
    {
        foreach(Snowflake snowflake in snowflakes) 
        {
            snowflake.draw(effect, device);
        }
    }
}