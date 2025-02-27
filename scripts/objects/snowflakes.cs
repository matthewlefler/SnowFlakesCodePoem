using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace objects;

class Snowflakes
{
    private Snowflake[] snowflakes;

    private Vector3 wind; // + gravity
    private Random random;

    public Snowflakes(int num) 
    {
        random = new Random(DateTime.Now.Millisecond);
        wind = new Vector3(0.0f, -1f, 0.0f);

        snowflakes = new Snowflake[num];
        for (int i = 0; i < num; i++)
        {
            snowflakes[i] = new Snowflake(300, 300);
            snowflakes[i].position = new Vector3(random.NextSingle() - 0.5f, random.NextSingle()*10f, random.NextSingle() - 0.5f);
        }
    }

    public void falling(float dt) 
    {
        for(int i = 0; i < snowflakes.Length; i++) 
        {
            Snowflake snowflake = snowflakes[i];

            snowflake.position += wind * dt;

            snowflake.rotationX += random.NextSingle() * dt;
            snowflake.rotationY += random.NextSingle() * dt;

            if(snowflake.position.Y <= 0)
            {
                snowflakes[i] = new Snowflake(300, 300);
                snowflakes[i].position = new Vector3(random.NextSingle() - 0.5f, 10f, random.NextSingle() - 0.5f);
            }
        }
    }

    public void growing() 
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
            effect.World = Matrix.CreateScale(0.1f) * Matrix.CreateFromYawPitchRoll(snowflake.rotationY, snowflake.rotationX, 0f) * Matrix.CreateTranslation(snowflake.position);

            snowflake.calcVerts();
            snowflake.draw(effect, device);
        }
    }
}