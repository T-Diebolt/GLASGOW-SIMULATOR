using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using GLASGOW_SIMULATOR.Properties;

namespace GLASGOW_SIMULATOR
{
    public partial class GameScreen : UserControl
    {
        bool leftArrowDown, rightArrowDown, spaceKeyDown, moveRight, moveLeft, shoot;
        public static int difficulty, d, score, secondTimer;
        
        Player p1;
        List<Enemies> enemies = new List<Enemies>();

        List<Shot> fShots = new List<Shot>();
        List<Shot> eShots = new List<Shot>();

        SolidBrush bB = new SolidBrush(Color.Black);
        SolidBrush gB = new SolidBrush(Color.Lime);

        Random randGen = new Random();

        SoundPlayer shotSound = new SoundPlayer(Resources.gunshot8bit);

        public GameScreen()
        {
            InitializeComponent();
            InitializeGame();
        }

        

        public void InitializeGame()
        {
            Cursor.Hide();
            //create entities
            d = this.Width / (difficulty * 6);
            p1 = new Player();
            for(int i = 0; i < difficulty * 6; i++)
            {
                for(int ii = 0; ii < difficulty * 3; ii++)
                {
                    enemies.Add(new Enemies(i, ii));
                }
            }
            moveLeft = true;
            moveRight = true;
            shoot = true;

            friendlyEngine.Enabled = true;
            enemyEngine.Enabled = true;

            enemyEngine.Interval = 600 / difficulty;

            score = 0;
            secondTimer = 50;
        }

        private void GameScreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
                case Keys.Space:
                    spaceKeyDown = true;
                    break;
            }
        }

        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = false;
                    moveLeft = true;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    moveRight = true;
                    break;
                case Keys.Space:
                    spaceKeyDown = false;
                    shoot = true;
                    break;
            }
        }

        private void gameEngine_Tick(object sender, EventArgs e)
        {
            //time or score
            secondTimer--;
            if(secondTimer == 0)
            {
                secondTimer = 50;
                score++;
            }

            //move player
            List<int> columns = new List<int>();
            foreach(Enemies E in enemies)
            {
                columns.Add(E.x);
            }
            if(leftArrowDown && moveLeft && columns[0] < p1.x)
            {
                int newX = 0;
                for(int i = 0; i < columns.Count(); i++)
                {
                    if (columns[columns.Count() - 1 - i] < p1.x)
                    {
                        newX = columns[columns.Count() - 1 - i];
                        break;
                    }
                }
                p1.x = newX;
                moveLeft = false;
            }
            if (rightArrowDown && moveRight && columns[columns.Count() - 1] > p1.x)
            {
                int newX = 0;
                for (int i = 0; i < columns.Count(); i++)
                {
                    if (columns[i] > p1.x)
                    {
                        newX = columns[i];
                        break;
                    }
                }
                p1.x = newX;
                moveRight = false;
            }

            //clear last row to one bot
            bool oneColumn = true;
            foreach(Enemies E in enemies)
            {
                if (enemies[0].x != E.x)
                {
                    oneColumn = false;
                }
            }
            if (oneColumn)
            {
                while (enemies.Count != 1)
                {
                    enemies.RemoveAt(1);
                }
            }

            //player shoot and prevent double ups
            bool open = true;
            foreach(Shot S in fShots)
            {
                if(S.x == p1.x && S.y == this.Height - 2 * d)
                {
                    open = false;
                }
            }
            if (spaceKeyDown && shoot && open && enemies.Count == 1)
            {
                fShots.Add(new Shot(p1.x, this.Height - 2 * d, 3));
                shoot = false;
                shotSound.Play();
            }
            else if (spaceKeyDown && shoot && open)
            {
                fShots.Add(new Shot(p1.x, d * (difficulty * 7 - 2), 1));
                shoot = false;
                shotSound.Play();
            }
            

            //move player shot
            foreach (Shot S in fShots)
            {
                S.MovePlayerShot();
            }

            //remove out of bound player shots
            for (int i = 0; i < fShots.Count(); i++)
            {
                if (fShots[i].y < 0)
                {
                    fShots.RemoveAt(i);
                    i--;
                }
            }

            //player shot vs. enemy shot (ps lose)
            foreach(Shot ES in eShots)
            {
                for (int i = 0; i < fShots.Count(); i++)
                {
                    if (ES.x == fShots[i].x && ES.y >= fShots[i].y && fShots[i].ks == false)
                    {
                        fShots.RemoveAt(i);
                        i--;
                    }
                } 
            }

            //kill shot vs. enemy shot (ks win)
            for(int i = 0; i < eShots.Count(); i++)
            {
                foreach(Shot KS in fShots)
                {
                    if(eShots.Count > 0)
                    {
                        if (KS.x == eShots[i].x && KS.y <= eShots[i].y && KS.ks)
                        {
                            eShots.RemoveAt(i);
                            i--;
                        }
                    }
                    
                }
            }

            
            //player shot vs. enemy
            for(int i = 0; i < enemies.Count(); i++)
            {
                for(int ii = 0; ii < fShots.Count(); ii++)
                {
                    if (enemies[i].x == fShots[ii].x && enemies[i].y == fShots[ii].y && fShots[ii].ks)
                    {
                        fShots.RemoveAt(ii);
                        enemies[i].h = 0;
                    }
                    else if (enemies[i].x == fShots[ii].x && enemies[i].y == fShots[ii].y)
                    {
                        fShots.RemoveAt(ii);
                        enemies[i].h--;
                    }
                }
                if (enemies[i].h == 0)
                {
                    eShots.Add(new Shot(enemies[i].x, enemies[i].y, 2));
                    enemies.RemoveAt(i);
                    i--;
                    
                }
            }

            //enemy shot vs. player
            for (int i = 0; i < eShots.Count(); i++)
            {
                if (eShots[i].x == p1.x && eShots[i].y == this.Height - d)
                {
                    eShots.RemoveAt(i);
                    i++;
                    p1.h--;
                }
            }
            if(p1.h == 0)
            {
                friendlyEngine.Stop();
                enemyEngine.Stop();
                DeathScreen.result = "LOSS";
                Form1.ChangeScreen(this, new DeathScreen());
            }

            //win
            if(enemies.Count() == 0)
            {
                friendlyEngine.Stop();
                enemyEngine.Stop();
                DeathScreen.result = "WIN";
                Form1.ChangeScreen(this, new DeathScreen());
            }

            Refresh();
        }

        private void gameEngine_Tick_1(object sender, EventArgs e)
        {
            
            //computer shots move
            foreach (Shot S in eShots)
            {
                S.MoveEnemyShot();
            }

            //computer shoot
            List<int> eligibles = new List<int>();
            for(int i = 0; i < enemies.Count - 1; i++)
            {
                if (enemies[i + 1].x != enemies[i].x)
                {
                    eligibles.Add(i);
                }
            }
            eligibles.Add(enemies.Count() - 1);
            //foreach(Shot S in eShots)
            //{
            //    for(int i = 0; i < eligibles.Count(); i++)
            //    {
            //        if (eligibles.Count == 2 && S.y < enemies[eligibles[i]].y + 3 * d)
            //        {
            //            eligibles.RemoveAt(i);
            //            i--;
            //        }
            //    }
            //}
            if(eligibles.Count == 2)
            {
                eligibles.RemoveAt(randGen.Next(0, 2));
            }
            foreach (int i in eligibles)
            {
                int shoot = randGen.Next(1, 11);
                if(shoot == 1)
                {
                    eShots.Add(new Shot(enemies[i].x, enemies[i].y + d, 2));
                }
            }

            //remove out of bound enemy shots
            for(int i = 0; i < eShots.Count(); i++)
            {
                if (eShots[i].y > this.Height)
                {
                    eShots.RemoveAt(i);
                    i--;
                }
            }

            Refresh();
        }

        private void GameScreen_Paint(object sender, PaintEventArgs e)
        {
            //pixels sizing
            int p = d / 16;

            //draw player
            e.Graphics.FillRectangle(gB, p1.x + 3 * p, this.Height - 12 * p, p, 8 * p);
            e.Graphics.FillRectangle(gB, p1.x + 4 * p, this.Height - 13 * p, p, 8 * p);
            e.Graphics.FillRectangle(gB, p1.x + 5 * p, this.Height - 10 * p, p, 7 * p);
            e.Graphics.FillRectangle(gB, p1.x + 6 * p, this.Height - 12 * p, p, 9 * p);
            e.Graphics.FillRectangle(gB, p1.x + 7 * p, this.Height - 15 * p, p, 11 * p);
            e.Graphics.FillRectangle(gB, p1.x + 8 * p, this.Height - 15 * p, p, 11 * p);
            e.Graphics.FillRectangle(gB, p1.x + 9 * p, this.Height - 12 * p, p, 9 * p);
            e.Graphics.FillRectangle(gB, p1.x + 10 * p, this.Height - 10 * p, p, 7 * p);
            e.Graphics.FillRectangle(gB, p1.x + 11 * p, this.Height - 13 * p, p, 8 * p);
            e.Graphics.FillRectangle(gB, p1.x + 12 * p, this.Height - 12 * p, p, 8 * p);
            e.Graphics.FillRectangle(bB, p1.x + 7 * p, this.Height - 9 * p, 2 * p, 3 * p);

            //draw enemies
            foreach(Enemies E in enemies)
            {
                e.Graphics.FillRectangle(gB, E.x + 3 * p, E.y + 4 * p, p, 6 * p);
                e.Graphics.FillRectangle(gB, E.x + 4 * p, E.y + 3 * p, p, 9 * p);
                e.Graphics.FillRectangle(gB, E.x + 5 * p, E.y + 5 * p, p, 8 * p);
                e.Graphics.FillRectangle(gB, E.x + 6 * p, E.y + 6 * p, p, 9 * p);
                e.Graphics.FillRectangle(gB, E.x + 7 * p, E.y + 7 * p, p, 9 * p);
                e.Graphics.FillRectangle(gB, E.x + 8 * p, E.y + 7 * p, p, 9 * p);
                e.Graphics.FillRectangle(gB, E.x + 9 * p, E.y + 6 * p, p, 9 * p);
                e.Graphics.FillRectangle(gB, E.x + 10 * p, E.y + 5 * p, p, 8 * p);
                e.Graphics.FillRectangle(gB, E.x + 11 * p, E.y + 3 * p, p, 9 * p);
                e.Graphics.FillRectangle(gB, E.x + 12 * p, E.y + 4 * p, p, 6 * p);
                e.Graphics.FillRectangle(bB, E.x + 6 * p, E.y + 8 * p, p, 3 * p);
                e.Graphics.FillRectangle(bB, E.x + 9 * p, E.y + 8 * p, p, 3 * p);
                e.Graphics.FillRectangle(bB, E.x + 7 * p, E.y + 11 * p, 2 * p, p);
            }

            //draw friendly shots
            foreach(Shot S in fShots)
            {
                e.Graphics.FillRectangle(gB, S.x + 4 * p, S.y + 3 * p, p, 8 * p);
                e.Graphics.FillRectangle(gB, S.x + 11 * p, S.y + 3 * p, p, 8 * p);
            }

            //draw enemy shots
            foreach(Shot S in eShots)
            {
                e.Graphics.FillRectangle(gB, S.x + 4 * p, S.y + 7 * p, p, p);
                e.Graphics.FillRectangle(gB, S.x + 5 * p, S.y + 8 * p, 2 * p, p);
                e.Graphics.FillRectangle(gB, S.x + 7 * p, S.y + 9 * p, 2 * p, p);
                e.Graphics.FillRectangle(gB, S.x + 9 * p, S.y + 8 * p, 2 * p, p);
                e.Graphics.FillRectangle(gB, S.x + 11 * p, S.y + 7 * p, p, p);
            }
        }
    }
}
