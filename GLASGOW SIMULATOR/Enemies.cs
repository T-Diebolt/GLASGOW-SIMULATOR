using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GLASGOW_SIMULATOR
{
    internal class Enemies
    {
        public int x, y, h;

        public Enemies(int _x, int _y)
        {
            x = _x * GameScreen.d;
            y = _y * GameScreen.d;
            h = 2 * GameScreen.difficulty;
        }
    }
}
