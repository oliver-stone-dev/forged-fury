using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;

public class Timer
{
    public int Period { get; set; }
    public bool Running { get; private set;  }
    public bool Finished { get; private set; }

    private int _timerValue = 0;

    public Timer(int period)
    {
        Period = period;
    }

    public void Update(GameTime gameTime)
    {
        if (Running)
        {
            _timerValue -= gameTime.ElapsedGameTime.Milliseconds;
            if (_timerValue <= 0)
            {
                Finished = true;
                Running = false;
            }
        }
    }
    public void Start()
    {
        Finished = false;
        Running = true;
        _timerValue = Period;
    }

    public void Stop()
    {
        Finished = false;
        Running = false;
    }

}
