using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLASGOW_SIMULATOR
{
    internal class Shot
    {
        public int x, y, t;
        public bool ks;
        
        public Shot(int _x, int _y, int _f) 
        {
            x = _x;
            y = _y;
            ks = false;
            if (_f == 1)
            {
                t = 600 / GameScreen.difficulty / 120;
            }
            else if(_f == 3)
            {
                t = 600 / GameScreen.difficulty / 60;
                ks = true;
            }
        }

        public void MovePlayerShot()
        {
            t--;
            if(t == 0 && ks == false)
            {
                t = 600 / GameScreen.difficulty / 120;
                y -= GameScreen.d;
            }
            else if (t == 0 && ks)
            {
                t = 600 / GameScreen.difficulty / 30;
                y -= GameScreen.d;
            }
        }

        public void MoveEnemyShot()
        {
            y += GameScreen.d;
        }

    }
}
