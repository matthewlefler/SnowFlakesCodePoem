using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Timers;

namespace objects; 


public class Snowflake 
{  
    /*      
     *          _____
     *         /     \
     *   _____/   0   \_____    
     *  /     \       /     \     
     * /   5   \_____/   1   \    
     * \       /     \       /    
     *  \_____/ this  \_____/  
     *  /     \       /     \  
     * /   4   \_____/   2   \  
     * \       /     \       / 
     *  \_____/   3   \_____/  
     *        \       /
     *         \_____/
     *  ___     ___     ___
     * /   \___/   \___/   \       ___ ___ ___ ___ ___ 
     * \___/   \___/   \___/      |   |   |   |   |   |
     * /   \___/   \___/   \  __  |___|___|___|___|___|
     * \___/   \___/   \___/  __  |   |   |   |   |   |
     * /   \___/   \___/   \  __  |___|___|___|___|___|
     * \___/   \___/   \___/      |   |   |   |   |   |
     *     \___/   \___/          |___|___|___|___|___|
    */
    private float[,] hexagons;
    private float[,] receptive;
    private float[,] nonReceptive;
    private int width;
    private int height;

    private float outRadius = 0.2f;
    private float inRadius;
    private float widthDistance;

    private float constantAdd = 0.01f;

    private float backgroundValue = 0.4f;

    private float alpha = 2.06f;

    private Random random;

    private VertexPositionColorNormal[] vertices = [];

    public Vector3 position;
    public Quaternion rotation;

    public Snowflake(int width, int height, float constantAdd, float backgroundValue, float alpha, Vector3 position, Quaternion rotation) 
    {
        this.width = width + 2; // account for boundary edge
        this.height = height + 2;

        this.random = new Random(DateTime.Now.Millisecond);
        
        this.constantAdd = constantAdd;
        this.backgroundValue = backgroundValue;
        this.alpha = alpha;

        Console.WriteLine("constantAdd: " + constantAdd);
        Console.WriteLine("backgroundValue: " + backgroundValue);
        Console.WriteLine("alpha: " + alpha);
        

        inRadius = outRadius * (MathF.Sqrt(3)/2f);
        widthDistance = 3f/4f * outRadius;

        hexagons = new float[this.width, this.height];

        for(int x = 0; x < width; x++) 
        {
            for (int y = 0; y < height; y++)
            {
                hexagons[x, y] = backgroundValue + ((random.Next(10) / 10 - 0.5f) / 3f);
            }
        }
        hexagons[width / 2, height / 2] = 1.0f;

        receptive = (float[,])hexagons.Clone();
        nonReceptive = (float[,])hexagons.Clone();    

    }

    public Snowflake(int width, int height)
    {
        this.rotation = Quaternion.Identity;
        this.position = Vector3.Zero;

        this.width = width + 2; // account for boundary edge
        this.height = height + 2;

        this.random = new Random(DateTime.Now.Millisecond);
        
        this.constantAdd = random.NextSingle() / 7f;
        this.backgroundValue = random.NextSingle();
        this.alpha = random.NextSingle() * 2f;
        
        Console.WriteLine("constantAdd: " + constantAdd);
        Console.WriteLine("backgroundValue: " + backgroundValue);
        Console.WriteLine("alpha: " + alpha);

        inRadius = outRadius * (MathF.Sqrt(3)/2f);
        widthDistance = 3f/4f * outRadius;

        hexagons = new float[this.width, this.height];

        for(int x = 0; x < width; x++) 
        {
            for (int y = 0; y < height; y++)
            {
                hexagons[x, y] = backgroundValue + ((random.Next(10) / 10 - 0.5f) / 3f);
            }
        }
        hexagons[width / 2, height / 2] = 1.0f;

        receptive = (float[,])hexagons.Clone();
        nonReceptive = (float[,])hexagons.Clone();        

    }

    private void addRandomSeed(int max) {
        int randomX = random.Next(0, max);
        int randomY = random.Next(0, max);
        hexagons[width / 2 + randomX, height / 2 + randomY] = 1.0f;
    }
    
    public void tick()
    {
        bool[,] is_receptive = new bool[width, height];
        for(int x = 1; x < width - 1; ++x) 
        {
            for (int y = 1; y < height - 1; ++y)
            {
                receptive[x, y] = 0f;
                nonReceptive[x, y] = 0f;
                /*      ___
                 *  ___/ 0 \___
                 * / 5 \___/ 1 \  
                 * \___/   \___/ 
                 * / 4 \___/ 2 \   
                 * \___/ 3 \___/  
                 *     \___/
                */
                if(hexagons[x, y] >= 1.0f) {
                    is_receptive[x, y] = true;

                    is_receptive[x, y + 1] = true; // 0
                    is_receptive[x, y - 1] = true; // 3
                    
                    if(x % 2 == 0) 
                    {
                        is_receptive[x + 1, y + 1] = true; // 1
                        is_receptive[x + 1, y] = true;     // 2
                        is_receptive[x - 1, y] = true;     // 4
                        is_receptive[x - 1, y + 1] = true; // 5
                    } 
                    else
                    {
                        is_receptive[x + 1, y] = true;     // 1
                        is_receptive[x + 1, y - 1] = true; // 2
                        is_receptive[x - 1, y - 1] = true; // 4
                        is_receptive[x - 1, y] = true;     // 5
                    }
                }
                               

            }
        }

        for(int x = 1; x < width - 1; ++x) 
        {
            for (int y = 1; y < height - 1; ++y)
            {
                if(is_receptive[x, y])
                {
                    receptive[x, y] = hexagons[x, y] + constantAdd;
                }
                else
                {
                    nonReceptive[x,y] = hexagons[x, y];
                }
            }
        }

        nonReceptive = avgHexGrid(nonReceptive);

        for(int x = 1; x < width - 1; x++) 
        {
            for (int y = 1; y < height - 1; y++)
            {
                hexagons[x, y] = nonReceptive[x, y] + receptive[x, y];
                maxVal = hexagons[x, y] > maxVal ? hexagons[x, y] : maxVal;
            }
        }
    }

    private float[,] avgHexGrid(float[,] grid) 
    {
        float[,] temp_grid = (float[,]) grid.Clone();

        float neighbor0; 
        float neighbor1; 
        float neighbor2; 
        float neighbor3; 
        float neighbor4; 
        float neighbor5; 

        for(int x = 1; x < width - 1; x++) 
        {
            for (int y = 1; y < height - 1; y++)
            {
                /*      ___
                 *  ___/ 0 \___
                 * / 5 \___/ 1 \  
                 * \___/   \___/ 
                 * / 4 \___/ 2 \   
                 * \___/ 3 \___/  
                 *     \___/
                */
                neighbor0 = grid[x, y + 1];                        
                neighbor3 = grid[x, y - 1];    

                if(x % 2 == 0) 
                {
                    neighbor1 = grid[x + 1, y + 1];
                    neighbor2 = grid[x + 1, y];    
                    neighbor4 = grid[x - 1, y];    
                    neighbor5 = grid[x - 1, y + 1];
                } 
                else
                {
                    neighbor1 = grid[x + 1, y];     
                    neighbor2 = grid[x + 1, y - 1]; 
                    neighbor4 = grid[x - 1, y - 1]; 
                    neighbor5 = grid[x - 1, y];     
                }

                float weight = alpha/12f;
                temp_grid[x,y] = (1 - (alpha * 0.5f)) * grid[x,y];
                temp_grid[x,y] += weight * (neighbor0 + neighbor1 + neighbor2 + neighbor3 + neighbor4 + neighbor5);
            }
        }
        return temp_grid;
    }

    public bool filled()
    {
        return hexagons[(int) (width / 1.2f), height / 2] > 1.0f ? true : false;
    }

    private float maxVal = 0.0f;
    public void calcVerts()
    {
        this.calcVerts(1.0f);
    }

    public void calcVerts(float cutoff) 
    {
        List<VertexPositionColorNormal> temp = new List<VertexPositionColorNormal>();
        Color color = Color.White;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(hexagons[x, y] > cutoff) {
                    Vector3 center;
                    if(x % 2 == 0)
                    {
                        center = new Vector3((x - width/2) * widthDistance, (y - width/2) * inRadius + inRadius/2f, 0);
                    }
                    else 
                    {
                        center = new Vector3((x - width/2) * widthDistance, (y - width/2) * inRadius, 0);
                    }
                    for (int i = 0; i < 6; i++)
                    {
                        Vector3 point1 = new Vector3(MathF.Cos((i/6f) * 2f*MathF.PI) * (outRadius / 2f),       MathF.Sin((i/6f) * 2f*MathF.PI) * (outRadius / 2f),       0) + center;
                        Vector3 point2 = new Vector3(MathF.Cos(((i + 1)/6f) * 2f*MathF.PI) * (outRadius / 2f), MathF.Sin(((i + 1)/6f) * 2f*MathF.PI) * (outRadius / 2f), 0) + center;

                        temp.Add(new VertexPositionColorNormal(center, color, Vector3.Backward));
                        temp.Add(new VertexPositionColorNormal(point1, color, Vector3.Backward));
                        temp.Add(new VertexPositionColorNormal(point2, color, Vector3.Backward));
                    }
                }
            }
        }

        vertices = temp.ToArray();
    }

    public void draw(BasicEffect effect, GraphicsDevice device) 
    {
        if(vertices.Length <= 0) { return; }

        foreach(EffectPass pass in effect.CurrentTechnique.Passes) 
        {
            pass.Apply();
            device.DrawUserPrimitives<VertexPositionColorNormal>(PrimitiveType.TriangleList, vertices, 0, vertices.Length / 3, VertexPositionColorNormal.VertexDeclaration);
        }
    }
}
