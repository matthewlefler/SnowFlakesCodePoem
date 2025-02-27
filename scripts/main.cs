using System;
using Microsoft.Xna.Framework;
using objects;

namespace SnowFlakesCodePoem;


public class Snow : ing
{
    protected override void Initialize()
    {
        snowflakes = /*a collection of*/ new Snowflakes();

        snowflakes.add([
            "one snowflake", 
            "two snowflakes",
            "three", "four", "five", "six", "seven", "eight", "nine", "..."]);

        base.Initialize();
    }

    protected override void Update(GameTime gameTime)
    {
        snowflakes.falling(/*from above*/ time);// and time again
        float timePassed = time;
        timeCounted += timePassed;

        /* looking and waiting */

        snowflakes.growing(); /* and */        
        snowflakes.floatingAndTumbling(/* while the */ time); // ticks down 
        
        /* all until the */snowflakes.hitTheGround();

        /* looking and waiting */
        // for that perfect snowflake

        // and the loop repeats
        base.Update(gameTime);
    }
}

