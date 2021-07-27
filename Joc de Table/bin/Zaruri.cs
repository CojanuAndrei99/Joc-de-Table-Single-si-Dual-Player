using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proiect.bin
{
    public class Zaruri
    {
        public int zar1, zar2;

        public PictureBox zar1Image, zar2Image;

        public bool zar_dublu;

        private Random random = new Random();

        string[] paths=new string[7];

        public Zaruri(string[] cai)
        {
            for(int i=1;i<=6;i++)
                paths[i] = cai[i];
        }   
        public void aruncaZarurile()
        {
            zar1 = random.Next(1, 7); 
            zar2 = random.Next(1, 7);//se genereaza zarurile
            zar1Image.Image = Image.FromFile(paths[zar1]);
            zar2Image.Image = Image.FromFile(paths[zar2]);
            //se verifica daca este zar dublu
            if (zar1==zar2)
                zar_dublu = true;
            else
                zar_dublu = false;

            
        }
        public void resetZar1()
        {
            zar1 = 0;
            zar1Image.Image = null;
            zar1Image.BorderStyle = BorderStyle.None;
        }
        public void resetZar2()
        {
            zar2 = 0;
            zar2Image.Image = null;
            zar2Image.BorderStyle = BorderStyle.None;
        }
    }
}
