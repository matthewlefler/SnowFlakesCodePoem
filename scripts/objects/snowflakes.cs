using System;
using System.Data;
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

    public void update(float dt) 
    {
        foreach(Snowflake snowflake in snowflakes) 
        {
            snowflake.position += wind * dt;
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