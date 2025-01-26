using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace forged_fury;

public interface IScoreTracker
{
    public void AddScore(int amount);
}
