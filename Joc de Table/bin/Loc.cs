using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proiect.bin
{
    public class Loc
    {
        public Culoare? culoarePiese;
        public PictureBox imagine;
        public int numarPiese;
         
        public Loc(Culoare? cul,int nrPiese,Image imag)
        {
            this.culoarePiese = cul;
            this.numarPiese = nrPiese;
            this.imagine.Image = imag;
        }

        public Loc(Culoare? cul, int nrPiese, PictureBox imag)
        {
            this.culoarePiese = cul;
            this.numarPiese = nrPiese;
            this.imagine = imag;
        }
        public Loc() { }

        public void locSet(Culoare? cul, int nrPiese,Image img)
        {
            this.culoarePiese = cul;
            this.numarPiese = nrPiese;
            this.imagine.Image = img;
        }
    }
}
