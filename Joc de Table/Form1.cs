using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proiect_atestat
{
    public partial class Form1 : Form
    {

        public struct pozitia
        {
            public int nr;
            public char culoare;
        };
        public string  cale = Environment.CurrentDirectory;
        public string[] coloanaNegre,
                         coloanaAlbe,
                         zaruri,
                         casaAlba,
                         casaNeagra;
        public PictureBox[] poz_ = new PictureBox[26];
        public Random rnd = new Random();
        public int x, y, calba, cneagra, afalba, afneagra;
        public bool selected = false, rand = true;//rand=0=albe   rand=1=negre
        public PictureBox temp = new PictureBox();
        public pozitia[] poz = new pozitia[26];
        public char randul;


        public Form1()
        {
            int i;
            InitializeComponent();
            coloanaNegre = new string[20];
            coloanaAlbe = new string[20];
            //initializare cai coloanaNegre coloanaAlbe
            for (i = 0; i < 20; i++)
            {
                if (i > 15)
                {
                    coloanaNegre[i] = cale + "/locasuri/negru/" + (i - 15) + "r.png";
                    coloanaAlbe[i] = cale + "/locasuri/alb/" + (i - 15) + "r.png";
                }
                else
                {
                    coloanaNegre[i] = cale + "/locasuri/negru/" + i + ".png";
                    coloanaAlbe[i] = cale + "/locasuri/alb/" + i + ".png";
                }
            }
            
            casaAlba = new string[16];
            casaNeagra = new string[16];
            //initializare cai casaAlba casaNeagra
            for(i=0;i<16;i++)
            {
                casaAlba[i] = cale + "/casa/alb/" + i + ".png";
                casaNeagra[i] = cale + "/casa/negru/" + i + ".png";
            }
            
            //initializare cai zaruri
            zaruri = new string[7];
            zaruri[0] = null;
            for(i=1;i<=6;i++)
            {
                zaruri[i] = cale + "/zaruri/" + i + ".png";
            }
                   
            //actualizarea matricei de ImageBox
            for (i = 1; i <= 24; i++)
            {
                poz_[i] = new PictureBox();
                poz_[i].Parent = tabla;
                if(i<=6)
                    poz_[i].SetBounds(20 + (6 - i) * 50 + 340, 230, 50, 150);
                if (i>6 && i<=12)
                    poz_[i].SetBounds(20 + (12 - i) * 50, 230, 50, 150);
                if (i>12 && i<=18)
                    poz_[i].SetBounds(20 + (i - 13) * 50, 20, 50, 150);
                if (i>18 && i<=24)
                    poz_[i].SetBounds(20 + (i - 19) * 50 + 340, 20, 50, 150);
                poz_[i].Name = "poz_" + i;
                poz_[i].BackColor = Color.Transparent;
                poz_[i].Image = Image.FromFile(coloanaAlbe[0]);
                poz_[i].BringToFront();
                poz_[i].MouseClick += new System.Windows.Forms.MouseEventHandler(this.Mutare);
                poz_[i].MouseHover += new System.EventHandler(this.loc_1_MouseHover);
                poz_[i].MouseLeave += new System.EventHandler(this.loc_1_MouseLeave);
            }
            
            turn.Text = "Aici se va afisa randul";
            
            //actualizare case
            Afara_Negru.Image = Image.FromFile(casaAlba[0]);
            Afara_Alb.Image = Image.FromFile(casaNeagra[0]);
            poz_0.Image = Image.FromFile(casaAlba[0]);
            poz_25.Image = Image.FromFile(casaNeagra[0]);
            poz_[25] = new PictureBox();
            poz_[25] = poz_25;
            poz_[0] = poz_0;
            turn.Text = "Aici se va afisa randul";
        }
        private void loc_1_MouseHover(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)(sender);
            if (p.BorderStyle != BorderStyle.Fixed3D)
                p.BorderStyle = BorderStyle.FixedSingle;
        }
        private void loc_1_MouseLeave(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)(sender);
            if(p.BorderStyle!=BorderStyle.Fixed3D)
                p.BorderStyle = BorderStyle.None;
        }

        private void Pass_MouseClick(object sender, MouseEventArgs e)
        {
            int i;
            if(turn.Text == "Aici se va afisa randul")
            {
                MessageBox.Show("Jocul nu a inceput inca!");
            }
            else
            {
                selected = false;
                x = 0;
                y = 0;
                zar1.Image = null;
                zar2.Image = null;
                zar1.BorderStyle = BorderStyle.None;
                zar2.BorderStyle = BorderStyle.None;
                poz_0.BorderStyle = BorderStyle.None;
                poz_25.BorderStyle = BorderStyle.None;
                for (i = 1; i <= 24; i++)
                    poz_[i].BorderStyle = BorderStyle.None;
            }
            
        }

        Int32 ultimulNumar(string x)
        {
            int i=Convert.ToInt32(x.Substring(x.Length -1, 1)),j;

            char s = x[x.Length - 2];
            if (s == '_' || s=='z')
                return i;
            else
            {
                if (s == '1')
                    j = 1;
                else
                    j = 2;
                return j * 10 + i;
            }
        }
        
        int indexPoz(PictureBox p)
        {
            for(int i=0;i<=25;i++)
                if (p.Name == poz_[i].Name)
                    return i;
            return -1;
        }
        char Castigator()
        {
            if (cneagra == 15)
            {
                MessageBox.Show("NEGRUL A CASTIGAT!!!");
                return 'n';
            }
            if (calba == 15)
            {
                MessageBox.Show("ALBUL A CASTIGAT!!!");
                return 'a';
            }
            return '\0';
        }
        bool exceptiiNotSelected(int l)
        {
            //nu s-a selectat casa
            if (l == 0 || l == 25)
            {
                MessageBox.Show("Nu se poate selecta casa fara o piesa selectata!");
                return false;
            }
            //piesa data afara
            if ((!rand && afalba > 0) || (rand && afneagra > 0))
            {
                MessageBox.Show("Va rugam reintroduceti piesa data afara!");
                return false;
            }
            //nu sunt piese
            if(l==-1 && ((rand && afneagra==0) || (!rand && afalba == 0)))
            {
                MessageBox.Show("Nu sunt piese aici!");
                return false;
            }
            if (poz[l].nr == 0)
            {
                MessageBox.Show("Nu sunt piese aici!");
                return false;
            }
            //culoare potrivita
            if ((!rand && poz[l].culoare == 'n') || ((rand && poz[l].culoare == 'a')))
            {
                MessageBox.Show("Culoarea piesei este incorecta!");
                return false;
            }
            return true;
        }
       
        bool exceptiiSelected(int l)
        {
            if(l==-1)
            {
                MessageBox.Show("Nu se poate muta aici!");
                return false;
            }
            if ((!rand && poz[l].culoare == 'n' && poz[l].nr>1) || ((rand && poz[l].culoare == 'a' && poz[l].nr > 1)))
            {
                MessageBox.Show("Nu se poate muta aici!");
                return false;
            }
            if(poz_[l].BorderStyle==BorderStyle.None || poz_[l].BorderStyle == BorderStyle.FixedSingle)
            {
                MessageBox.Show("Nu se poate muta aici!");
                return false;
            }
            return true;
        }
        bool validareIntrareCasa(bool rand)
        {
            int nr_piese = 0;
            if(rand)
            {
                for(int i=19;i<=25;i++)
                {
                    if(poz[i].culoare=='n')
                        nr_piese += poz[i].nr;
                }
                if (nr_piese+1 == 15)
                    return true;
            }
            else
            {
                for (int i = 0; i <= 6; i++)
                {
                    if (poz[i].culoare == 'a')
                        nr_piese += poz[i].nr;
                }
                if (nr_piese+1 == 15)
                    return true;
            }
            return false;
        }
        
        int corectieNumar(int l,int nr)//functia corecteaza pozitia pieselor in functie de pozitie
        {
            if (l >= 13 && l <= 24 && nr<5)
                return nr + 15;
            else
                return nr;
        }

        private void Mutare(object sender, MouseEventArgs e)
        {
            PictureBox p = (PictureBox)(sender);
            int l = indexPoz(p);
            //s-a dat cu zarul
            if(x<=0&&y<=0)
            {
                MessageBox.Show("Nu s-a dat cu zarul!");
                return;
            }
            //la selectia piesei se vor calcula si mutarile posibile si aceasta functie va avea rol si de arbitru
            if(!selected)
            {
                if(exceptiiNotSelected(l))
                {
                    // daca piesa selectata indeplineste conditiile trec la marcarea pozitiilor posibile ptr realizarea unei mutari
                    selected = true;
                    poz[l].nr--;
                    if(rand)
                    {
                        this.Cursor = new Cursor(Application.StartupPath + "\\piesa_neagra.cur");
                        poz_[l].Image = Image.FromFile(coloanaNegre[corectieNumar(l,poz[l].nr)]);
                    }
                    else
                    {
                        this.Cursor = new Cursor(Application.StartupPath + "\\piesa_alba.cur");
                        poz_[l].Image = Image.FromFile(coloanaAlbe[corectieNumar(l, poz[l].nr)]);
                    }
                    poz_[l].BorderStyle = BorderStyle.Fixed3D;
                    //intrare in casa dintr-un zar
                    if(((l+x>24 || l+y>24) ||(l-x<1 ||l-y<1)))
                    {
                        if(validareIntrareCasa(rand))
                        {

                            //zar fix de intrare in casa
                            if(l+x==25||l+y==25 ||l-x==0||l-y==0)
                        {
                            if (rand)//negru
                            {
                                poz_[25].BorderStyle = BorderStyle.Fixed3D;
                            }
                            else//alb
                            {
                                poz_[0].BorderStyle = BorderStyle.Fixed3D;
                            }
                        }
                            //zarul depaseste nr dorit si trebuie validat daca mai sunt piese in urma
                            else
                            {
                                bool ok = false;
                                if(rand)//negru
                            {
                                for (int index=19;index<=l && !ok;index++)
                                    if (poz[index].nr != 0)
                                        ok = true;
                                if(!ok)
                                    poz_[25].BorderStyle = BorderStyle.Fixed3D;
                            }
                                else//alb
                            {
                                for (int index = 6; index >= l && !ok; index--)
                                    if (poz[index].nr != 0)
                                        ok = true;
                                if (!ok)
                                    poz_[0].BorderStyle = BorderStyle.Fixed3D;
                            }
                            }
                            
                        }
                        if (l + x <= 24 && rand)
                            poz_[l + x].BorderStyle = BorderStyle.Fixed3D;
                        if (l - x >= 1 && !rand)
                            poz_[l - x].BorderStyle = BorderStyle.Fixed3D;
                        if (l + y <= 24 && rand)
                            poz_[l + y].BorderStyle = BorderStyle.Fixed3D;
                        if (l - y >= 1 && !rand)
                            poz_[l - y].BorderStyle = BorderStyle.Fixed3D;
                    }
                    else
                    {
                        //nu se depaseste suma necesara intrarii in casa
                        if(rand)//negru
                        {
                            poz_[l + x].BorderStyle = BorderStyle.Fixed3D;
                            poz_[l + y].BorderStyle = BorderStyle.Fixed3D;
                            if(l+x+y<25)
                                poz_[l + x + y].BorderStyle = BorderStyle.Fixed3D;
                            else
                                if (l + x + y == 25 && validareIntrareCasa(rand))
                                    poz_[l + x + y].BorderStyle = BorderStyle.Fixed3D;
                        }
                        else//alb
                        {
                            poz_[l - x].BorderStyle = BorderStyle.Fixed3D;
                            poz_[l - y].BorderStyle = BorderStyle.Fixed3D;
                            if(l-x-y>0)
                                poz_[l - x - y].BorderStyle = BorderStyle.Fixed3D;
                            else
                                if(l - x - y == 0 && validareIntrareCasa(rand))
                                    poz_[l - x - y].BorderStyle = BorderStyle.Fixed3D;
                        }
                        
                    }
                }
            }
            else
            {
                if(exceptiiSelected(l))
                {
                    this.Cursor = Cursors.Default;
                    if(rand)
                    {
                        
                    }
                    else
                    {

                    }
                }
            }
            
            //    else
            //    {
            //        int ltemp = ultimulNumar(temp.Name),sn,sa;
            //        sn = 0;
            //        sa = 0;
            //        for (i = 19; i <= 24; i++)
            //            if (poz[i].culoare == 'n')
            //                sn += poz[i].nr;
            //        for (i = 1; i <= 6; i++)
            //            if (poz[i].culoare == 'a')
            //                sa += poz[i].nr;
            //        if(ltemp>=1 && ltemp<=24)
            //        {
            //            if (poz[ltemp].culoare=='n')
            //            {
            //                if(l==ltemp+x+1)
            //                {
            //                    if ((l == 25 && sn == 15) || (l == 25 && cneagra > 0 && sn+cneagra==15))
            //                    {
            //                        cneagra++;
            //                        selected = false;
            //                        poz[ltemp].nr--;
            //                        if (poz[ltemp].nr == 0)
            //                        {
            //                            poz[ltemp].culoare = ' ';
            //                            poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr]);
            //                        }
            //                        else
            //                        {
            //                            if(poz[ltemp].nr < 5)
            //                                poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr + 10]);
            //                            else
            //                                poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr]);
            //                        }
            //                        poz_25.Image = Image.FromFile(casaNeagra[cneagra]);
            //                        if(zar1.BorderStyle==BorderStyle.Fixed3D)
            //                        {
            //                            zar1.BorderStyle = BorderStyle.None;
            //                        }
            //                        else
            //                        {
            //                            x = -1;
            //                            zar1.Image = null;
            //                        }
            //                        poz_0.BorderStyle = BorderStyle.None;
            //                        poz_25.BorderStyle = BorderStyle.None;
            //                        for (i = 1; i <= 24; i++)
            //                            poz_[i].BorderStyle = BorderStyle.None;
            //                    }
            //                    else
            //                    {
            //                        if (poz[l].culoare=='a')
            //                        {
            //                            if (poz[l].nr == 1)//dat afara
            //                            {
            //                                selected = false;
            //                                poz[l].culoare = poz[ltemp].culoare;
            //                                poz[ltemp].nr--;
            //                                afalba++;
            //                                if (poz[ltemp].nr == 0)
            //                                    poz[ltemp].culoare = ' ';
            //                                Afara_Negru.Image = Image.FromFile(casaAlba[afalba]);
            //                                if (zar1.BorderStyle == BorderStyle.Fixed3D)
            //                                {
            //                                    zar1.BorderStyle = BorderStyle.None;
            //                                }
            //                                else
            //                                {
            //                                    x = -1;
            //                                    zar1.Image = null;
            //                                }
            //                                    poz_0.BorderStyle = BorderStyle.None;
            //                                poz_25.BorderStyle = BorderStyle.None;
            //                                for (i = 1; i <= 24; i++)
            //                                    poz_[i].BorderStyle = BorderStyle.None;
            //                                if (l > 12 && poz[l].nr < 5)
            //                                {
            //                                    poz_[l].Image = Image.FromFile(coloanaNegre[poz[l].nr + 10]);
            //                                    if (ltemp > 12 && poz[ltemp].nr < 5 && poz[ltemp].nr > 0)
            //                                        poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr + 10]);
            //                                    else
            //                                        poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr]);
            //                                }
            //                                else
            //                                {
            //                                    poz_[l].Image = Image.FromFile(coloanaNegre[poz[l].nr]);
            //                                    if (ltemp > 12 && poz[ltemp].nr < 5 && poz[ltemp].nr > 0)
            //                                        poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr + 10]);
            //                                    else
            //                                        poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr]);
            //                                }//mutarea piesei
            //                            }
            //                            else
            //                            {
            //                                MessageBox.Show("Culoare necorespunzatoare!");
            //                                selected = false;
            //                            }
            //                        }
            //                        else
            //                        {
            //                            selected = false;
            //                            poz[l].culoare = poz[ltemp].culoare;
            //                            poz[l].nr++;
            //                            poz[ltemp].nr--;
            //                            if (poz[ltemp].nr == 0)
            //                                poz[ltemp].culoare = ' ';
            //                            if (l > 12 && poz[l].nr < 5)
            //                                {
            //                                    poz_[l].Image = Image.FromFile(coloanaNegre[poz[l].nr + 10]);
            //                                    if (ltemp > 12 && poz[ltemp].nr < 5 && poz[ltemp].nr > 0)
            //                                        poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr + 10]);
            //                                    else
            //                                        poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr]);
            //                                }
            //                            else
            //                            {
            //                                poz_[l].Image = Image.FromFile(coloanaNegre[poz[l].nr]);
            //                                if (ltemp > 12 && poz[ltemp].nr < 5 && poz[ltemp].nr > 0)
            //                                    poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr + 10]);
            //                                else
            //                                    poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr]);
            //                            }//mutarea piesei
            //                            if (zar1.BorderStyle == BorderStyle.Fixed3D)
            //                            {
            //                                zar1.BorderStyle = BorderStyle.None;
            //                            }
            //                            else
            //                            {
            //                                x = -1;
            //                                zar1.Image = null;
            //                            }

            //                        }
            //                    }
            //                    poz_0.BorderStyle = BorderStyle.None;
            //                    poz_25.BorderStyle = BorderStyle.None;
            //                    for (i = 1; i <= 24; i++)
            //                        poz_[i].BorderStyle = BorderStyle.None;
            //                }
            //                else
            //                {
            //                    if(l==ltemp+y+1)
            //                    {
            //                        if ((l == 25 && sn == 15) || (l == 25 && cneagra > 0 && sn+cneagra==15))
            //                        {
            //                            cneagra++;
            //                            selected = false;
            //                            poz[ltemp].nr--;
            //                            if (poz[ltemp].nr == 0)
            //                            {
            //                                poz[ltemp].culoare = ' ';
            //                                poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr]);
            //                            }
            //                            else
            //                            {
            //                                if (poz[ltemp].nr < 5)
            //                                    poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr + 10]);
            //                                else
            //                                    poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr]);
            //                            }
            //                            poz_25.Image = Image.FromFile(casaNeagra[cneagra]);
            //                            if (zar2.BorderStyle == BorderStyle.Fixed3D)
            //                            {
            //                                zar2.BorderStyle = BorderStyle.None;
            //                            }
            //                            else
            //                            {
            //                                y = -1;
            //                                zar2.Image = null;
            //                            }
            //                        }
            //                        else
            //                        {
            //                            if (poz[l].culoare == 'a')
            //                            {
            //                                if (poz[l].nr == 1)//dat afara
            //                                {
            //                                    selected = false;
            //                                    poz[l].culoare = poz[ltemp].culoare;
            //                                    poz[ltemp].nr--;
            //                                    afalba++;
            //                                    if (poz[ltemp].nr == 0)
            //                                        poz[ltemp].culoare = ' ';
            //                                    Afara_Negru.Image = Image.FromFile(casaAlba[afalba]);
            //                                    if (zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                    {
            //                                        zar2.BorderStyle = BorderStyle.None;
            //                                    }
            //                                    else
            //                                    {
            //                                        y = -1;
            //                                        zar2.Image = null;
            //                                    }
            //                                    poz_0.BorderStyle = BorderStyle.None;
            //                                    poz_25.BorderStyle = BorderStyle.None;
            //                                    for (i = 1; i <= 24; i++)
            //                                        poz_[i].BorderStyle = BorderStyle.None;
            //                                    if (l > 12 && poz[l].nr < 5)
            //                                    {
            //                                        poz_[l].Image = Image.FromFile(coloanaNegre[poz[l].nr + 10]);
            //                                        if (ltemp > 12 && poz[ltemp].nr < 5 && poz[ltemp].nr > 0)
            //                                            poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr + 10]);
            //                                        else
            //                                            poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr]);
            //                                    }
            //                                    else
            //                                    {
            //                                        poz_[l].Image = Image.FromFile(coloanaNegre[poz[l].nr]);
            //                                        if (ltemp > 12 && poz[ltemp].nr < 5 && poz[ltemp].nr > 0)
            //                                            poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr + 10]);
            //                                        else
            //                                            poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr]);
            //                                    }//mutarea piesei
            //                                }
            //                                else
            //                                {
            //                                    MessageBox.Show("Culoare necorespunzatoare!");
            //                                    selected = false;
            //                                }
            //                            }
            //                            else
            //                            {
            //                                selected = false;
            //                                poz[l].culoare = poz[ltemp].culoare;
            //                                poz[l].nr++;
            //                                poz[ltemp].nr--;
            //                                if (zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                {
            //                                    zar2.BorderStyle = BorderStyle.None;
            //                                }
            //                                else
            //                                {
            //                                    y = -1;
            //                                    zar2.Image = null;
            //                                }
            //                                if (poz[ltemp].nr == 0)
            //                                    poz[ltemp].culoare = ' ';
            //                                if (l > 12 && poz[l].nr < 5)
            //                                {
            //                                    poz_[l].Image = Image.FromFile(coloanaNegre[poz[l].nr + 10]);
            //                                    if (ltemp > 12 && poz[ltemp].nr < 5 && poz[ltemp].nr > 0)
            //                                        poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr + 10]);
            //                                    else
            //                                        poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr]);
            //                                }
            //                                else
            //                                {
            //                                    poz_[l].Image = Image.FromFile(coloanaNegre[poz[l].nr]);
            //                                    if (ltemp > 12 && poz[ltemp].nr < 5 && poz[ltemp].nr > 0)
            //                                        poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr + 10]);
            //                                    else
            //                                        poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr]);
            //                                }//mutarea piesei
            //                            }
            //                        }
            //                            poz_0.BorderStyle = BorderStyle.None;
            //                            poz_25.BorderStyle = BorderStyle.None;
            //                            for (i = 1; i <= 24; i++)
            //                                poz_[i].BorderStyle = BorderStyle.None;
            //                        }
            //                    else
            //                    {
            //                        if (l == ltemp + x + y + 2)
            //                        {
            //                            if(poz[ltemp+x+1].nr>=2 && poz[ltemp + x + 1].culoare=='a' && poz[ltemp + y + 1].nr >= 2 && poz[ltemp + y + 1].culoare == 'a')
            //                            {
            //                                MessageBox.Show("Mutare invalida!");
            //                                selected = false;
            //                                poz_0.BorderStyle = BorderStyle.None;
            //                                poz_25.BorderStyle = BorderStyle.None;
            //                                for (i = 1; i <= 24; i++)
            //                                    poz_[i].BorderStyle = BorderStyle.None;
            //                            }
            //                            else
            //                            {
            //                                if ((l == 25 && sn == 15) || (l == 25 && cneagra > 0 && sn+cneagra==15))
            //                                {
            //                                    cneagra++;
            //                                    selected = false;
            //                                    poz[ltemp].nr--;
            //                                    if (poz[ltemp].nr == 0)
            //                                {
            //                                    poz[ltemp].culoare = ' ';
            //                                    poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr]);
            //                                }
            //                                    else
            //                                {
            //                                    if (poz[ltemp].nr < 5)
            //                                        poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr + 10]);
            //                                    else
            //                                        poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr]);
            //                                }
            //                                    poz_25.Image = Image.FromFile(casaNeagra[cneagra]);
            //                                    if (zar1.BorderStyle==BorderStyle.Fixed3D && zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                {
            //                                    zar1.BorderStyle = BorderStyle.None;
            //                                    zar2.BorderStyle = BorderStyle.None;
            //                                }
            //                                    else
            //                                {
            //                                    if(zar1.BorderStyle==BorderStyle.Fixed3D)
            //                                    {
            //                                        zar1.BorderStyle = BorderStyle.None;
            //                                        y = -1; zar2.Image = null;
            //                                    }
            //                                    else
            //                                    {
            //                                        if(zar2.BorderStyle==BorderStyle.Fixed3D)
            //                                        {
            //                                            x = -1; zar1.Image = null;
            //                                            zar2.BorderStyle = BorderStyle.None;
            //                                        }
            //                                        else
            //                                        {
            //                                            x = -1; zar1.Image = null;
            //                                            y = -1; zar2.Image = null;
            //                                        }
            //                                    }
            //                                }
            //                                    poz_0.BorderStyle = BorderStyle.None;
            //                                    poz_25.BorderStyle = BorderStyle.None;
            //                                    for (i = 1; i <= 24; i++)
            //                                        poz_[i].BorderStyle = BorderStyle.None;
            //                                }
            //                                else
            //                                {
            //                                    if (poz[l].culoare == 'a')
            //                                {
            //                                    if (poz[l].nr == 1)//dat afara
            //                                    {
            //                                        selected = false;
            //                                        poz[l].culoare = poz[ltemp].culoare;
            //                                        poz[ltemp].nr--;
            //                                        afalba++;
            //                                        if (poz[ltemp].nr == 0)
            //                                            poz[ltemp].culoare = ' ';
            //                                        Afara_Negru.Image = Image.FromFile(casaAlba[afalba]);
            //                                        if (zar1.BorderStyle == BorderStyle.Fixed3D && zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                    {
            //                                        zar1.BorderStyle = BorderStyle.None;
            //                                        zar2.BorderStyle = BorderStyle.None;
            //                                    }
            //                                        else
            //                                        {
            //                                            if (zar1.BorderStyle == BorderStyle.Fixed3D)
            //                                            {
            //                                                zar1.BorderStyle = BorderStyle.None;
            //                                                y = -1; zar2.Image = null;
            //                                            }
            //                                            else
            //                                            {
            //                                                if (zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                                {
            //                                                    x = -1; zar1.Image = null;
            //                                                    zar2.BorderStyle = BorderStyle.None;
            //                                                }
            //                                                else
            //                                                {
            //                                                    x = -1; zar1.Image = null;
            //                                                    y = -1; zar2.Image = null;
            //                                                }
            //                                            }
            //                                        }
            //                                        poz_0.BorderStyle = BorderStyle.None;
            //                                        poz_25.BorderStyle = BorderStyle.None;
            //                                        for (i = 1; i <= 24; i++)
            //                                            poz_[i].BorderStyle = BorderStyle.None;
            //                                        if (l > 12 && poz[l].nr < 5)
            //                                        {
            //                                            poz_[l].Image = Image.FromFile(coloanaNegre[poz[l].nr + 10]);
            //                                            if (ltemp > 12 && poz[ltemp].nr < 5 && poz[ltemp].nr > 0)
            //                                                poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr + 10]);
            //                                            else
            //                                                poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr]);
            //                                        }
            //                                        else
            //                                        {
            //                                            poz_[l].Image = Image.FromFile(coloanaNegre[poz[l].nr]);
            //                                            if (ltemp > 12 && poz[ltemp].nr < 5 && poz[ltemp].nr > 0)
            //                                                poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr + 10]);
            //                                            else
            //                                                poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr]);
            //                                        }//mutarea piesei
            //                                    }
            //                                    else
            //                                        {
            //                                            MessageBox.Show("Culoare necorespunzatoare!");
            //                                            selected = false;
            //                                        }
            //                                }
            //                                    else
            //                                {
            //                                    selected = false;
            //                                    poz[l].culoare = poz[ltemp].culoare;
            //                                    poz[l].nr++;
            //                                    poz[ltemp].nr--;
            //                                    if (zar1.BorderStyle == BorderStyle.Fixed3D && zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                        {
            //                                            zar1.BorderStyle = BorderStyle.None;
            //                                            zar2.BorderStyle = BorderStyle.None;
            //                                        }
            //                                    else
            //                                    {
            //                                        if (zar1.BorderStyle == BorderStyle.Fixed3D)
            //                                        {
            //                                            zar1.BorderStyle = BorderStyle.None;
            //                                            y = -1; zar2.Image = null;
            //                                        }
            //                                        else
            //                                        {
            //                                            if (zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                            {
            //                                                x = -1; zar1.Image = null;
            //                                                zar2.BorderStyle = BorderStyle.None;
            //                                            }
            //                                            else
            //                                            {
            //                                                x = -1; zar1.Image = null;
            //                                                y = -1; zar2.Image = null;
            //                                            }
            //                                        }
            //                                    }
            //                                    if (poz[ltemp].nr == 0)
            //                                        poz[ltemp].culoare = ' ';
            //                                    if (l > 12 && poz[l].nr < 5)
            //                                {
            //                                    poz_[l].Image = Image.FromFile(coloanaNegre[poz[l].nr + 10]);
            //                                    if (ltemp > 12 && poz[ltemp].nr < 5 && poz[ltemp].nr > 0)
            //                                        poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr + 10]);
            //                                    else
            //                                        poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr]);
            //                                }
            //                                    else
            //                                {
            //                                    poz_[l].Image = Image.FromFile(coloanaNegre[poz[l].nr]);
            //                                    if (ltemp > 12 && poz[ltemp].nr < 5 && poz[ltemp].nr > 0)
            //                                        poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr + 10]);
            //                                    else
            //                                        poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr]);
            //                                }//mutarea piesei
            //                                }

            //                                }
            //                                poz_0.BorderStyle = BorderStyle.None;
            //                                poz_25.BorderStyle = BorderStyle.None;
            //                                for (i = 1; i <= 24; i++)
            //                                    poz_[i].BorderStyle = BorderStyle.None;

            //                            }
                                        
            //                        }
            //                        else
            //                        {
            //                            if(sn+cneagra==15 && ltemp+x+1>24 && l==25)
            //                            {
            //                                cneagra++;
            //                                selected = false;
            //                                poz[ltemp].nr--;
            //                                if (poz[ltemp].nr == 0)
            //                                {
            //                                    poz[ltemp].culoare = ' ';
            //                                    poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr]);
            //                                }
            //                                else
            //                                {
            //                                    if (poz[ltemp].nr < 5)
            //                                        poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr + 10]);
            //                                    else
            //                                        poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr]);
            //                                }
            //                                poz_25.Image = Image.FromFile(casaNeagra[cneagra]);
            //                                if (zar1.BorderStyle == BorderStyle.Fixed3D)
            //                                {
            //                                    zar1.BorderStyle = BorderStyle.None;
            //                                }
            //                                else
            //                                {
            //                                    x = -1;
            //                                    zar1.Image = null;
            //                                }
            //                                poz_0.BorderStyle = BorderStyle.None;
            //                                poz_25.BorderStyle = BorderStyle.None;
            //                                for (i = 1; i <= 24; i++)
            //                                    poz_[i].BorderStyle = BorderStyle.None;
            //                            }
            //                            else
            //                            {
            //                                if (sn + cneagra == 15 && ltemp + y + 1 > 24 && l == 25)
            //                                {
            //                                    cneagra++;
            //                                    selected = false;
            //                                    poz[ltemp].nr--;
            //                                    if (poz[ltemp].nr == 0)
            //                                    {
            //                                        poz[ltemp].culoare = ' ';
            //                                        poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr]);
            //                                    }
            //                                    else
            //                                    {
            //                                        if (poz[ltemp].nr < 5)
            //                                            poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr + 10]);
            //                                        else
            //                                            poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr]);
            //                                    }
            //                                    poz_25.Image = Image.FromFile(casaNeagra[cneagra]);
            //                                    if (zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                    {
            //                                        zar2.BorderStyle = BorderStyle.None;
            //                                    }
            //                                    else
            //                                    {
            //                                        y = -1;
            //                                        zar2.Image = null;
            //                                    }
            //                                    poz_0.BorderStyle = BorderStyle.None;
            //                                    poz_25.BorderStyle = BorderStyle.None;
            //                                    for (i = 1; i <= 24; i++)
            //                                        poz_[i].BorderStyle = BorderStyle.None;
            //                                }
            //                                else
            //                                {
            //                                    if (sn + cneagra == 15 && ltemp + x + y + 2 > 24 && l == 25)
            //                                    {
            //                                        if (poz[ltemp + x + 1].nr >= 2 && poz[ltemp + x + 1].culoare == 'a' && poz[ltemp + y + 1].nr >= 2 && poz[ltemp + y + 1].culoare == 'a' && l!=ltemp+x+y+2)
            //                                        {
            //                                            MessageBox.Show("Mutare invalida!");
            //                                            selected = false;
            //                                            poz_0.BorderStyle = BorderStyle.None;
            //                                            poz_25.BorderStyle = BorderStyle.None;
            //                                            for (i = 1; i <= 24; i++)
            //                                                poz_[i].BorderStyle = BorderStyle.None;
            //                                        }
            //                                        else
            //                                        {
            //                                            cneagra++;
            //                                            selected = false;
            //                                            poz[ltemp].nr--;
            //                                            if (poz[ltemp].nr == 0)
            //                                        {
            //                                            poz[ltemp].culoare = ' ';
            //                                            poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr]);
            //                                        }
            //                                            else
            //                                        {
            //                                            if (poz[ltemp].nr < 5)
            //                                                poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr + 10]);
            //                                            else
            //                                                poz_[ltemp].Image = Image.FromFile(coloanaNegre[poz[ltemp].nr]);
            //                                        }
            //                                            poz_25.Image = Image.FromFile(casaNeagra[cneagra]);
            //                                            if (zar1.BorderStyle == BorderStyle.Fixed3D && zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                        {
            //                                            zar1.BorderStyle = BorderStyle.None;
            //                                            zar2.BorderStyle = BorderStyle.None;
            //                                        }
            //                                            else
            //                                        {
            //                                            if (zar1.BorderStyle == BorderStyle.Fixed3D)
            //                                            {
            //                                                zar1.BorderStyle = BorderStyle.None;
            //                                                y = -1; zar2.Image = null;
            //                                            }
            //                                            else
            //                                            {
            //                                                if (zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                                {
            //                                                    x = -1; zar1.Image = null;
            //                                                    zar2.BorderStyle = BorderStyle.None;
            //                                                }
            //                                                else
            //                                                {
            //                                                    x = -1; zar1.Image = null;
            //                                                    y = -1; zar2.Image = null;
            //                                                }
            //                                            }
            //                                        }
            //                                            poz_0.BorderStyle = BorderStyle.None;
            //                                            poz_25.BorderStyle = BorderStyle.None;
            //                                            for (i = 1; i <= 24; i++)
            //                                                poz_[i].BorderStyle = BorderStyle.None;
            //                                        }
                                                        
            //                                    }
            //                                    else
            //                                    {
            //                                        MessageBox.Show("Mutare invalida!");
            //                                        selected = false;
            //                                        poz_0.BorderStyle = BorderStyle.None;
            //                                        poz_25.BorderStyle = BorderStyle.None;
            //                                        for (i = 1; i <= 24; i++)
            //                                            poz_[i].BorderStyle = BorderStyle.None;
            //                                    }
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }//piesa neagra
            //            else
            //            {
            //                if(l==ltemp-x-1)
            //                {
            //                    if ((l == 0 && sa == 15) || (l == 0 && calba > 0 && sa+calba==15))
            //                        {
            //                            calba++;
            //                            selected = false;
            //                            poz[ltemp].nr--;
            //                            if (poz[ltemp].nr == 0)
            //                                poz[ltemp].culoare = ' ';
            //                            poz_0.Image = Image.FromFile(casaAlba[calba]);
            //                            poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr]);
            //                        if (zar1.BorderStyle == BorderStyle.Fixed3D)
            //                        {
            //                            zar1.BorderStyle = BorderStyle.None;
            //                        }
            //                        else
            //                        {
            //                            x = -1;
            //                            zar1.Image = null;
            //                        }
            //                        poz_0.BorderStyle = BorderStyle.None;
            //                            poz_25.BorderStyle = BorderStyle.None;
            //                            for (i = 1; i <= 24; i++)
            //                                poz_[i].BorderStyle = BorderStyle.None;
            //                        }
            //                    else
            //                    {
            //                        if (poz[l].culoare == 'n')
            //                            {
            //                                if (poz[l].nr == 1)//dat afara
            //                                {
            //                                    selected = false;
            //                                    poz[l].culoare = poz[ltemp].culoare;
            //                                    poz[ltemp].nr--;
            //                                    afneagra++;
            //                                    if (poz[ltemp].nr == 0)
            //                                        poz[ltemp].culoare = ' ';
            //                                    Afara_Alb.Image = Image.FromFile(casaNeagra[afneagra]);
            //                                    if (zar1.BorderStyle == BorderStyle.Fixed3D)
            //                                {
            //                                    zar1.BorderStyle = BorderStyle.None;
            //                        }
            //                                    else
            //                                {
            //                                    x = -1;
            //                                    zar1.Image = null;
            //                                }
            //                                    poz_0.BorderStyle = BorderStyle.None;
            //                                    poz_25.BorderStyle = BorderStyle.None;
            //                                    for (i = 1; i <= 24; i++)
            //                                        poz_[i].BorderStyle = BorderStyle.None;
            //                                    if (l > 12 && poz[l].nr < 5)
            //                                {
            //                                    poz_[l].Image = Image.FromFile(coloanaAlbe[poz[l].nr + 10]);
            //                                    if (ltemp > 12 && poz[ltemp].nr < 5 && poz[ltemp].nr > 0)
            //                                        poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr + 10]);
            //                                    else
            //                                        poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr]);
            //                                }
            //                                    else
            //                                {
            //                                    poz_[l].Image = Image.FromFile(coloanaAlbe[poz[l].nr]);
            //                                    if (ltemp > 12 && poz[ltemp].nr < 5 && poz[ltemp].nr > 0)
            //                                        poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr + 10]);
            //                                    else
            //                                        poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr]);
            //                                }//mutarea piesei
            //                                }
            //                                else
            //                                {
            //                                    MessageBox.Show("Culoare necorespunzatoare!");
            //                                    selected = false;
            //                                    poz_0.BorderStyle = BorderStyle.None;
            //                                    poz_25.BorderStyle = BorderStyle.None;
            //                                    for (i = 1; i <= 24; i++)
            //                                        poz_[i].BorderStyle = BorderStyle.None;
            //                                }
            //                            }
            //                        else
            //                        {
            //                            selected = false;
            //                            poz[l].culoare = poz[ltemp].culoare;
            //                            poz[l].nr++;
            //                            if (zar1.BorderStyle == BorderStyle.Fixed3D)
            //                            {
            //                                zar1.BorderStyle = BorderStyle.None;
            //                            }
            //                            else
            //                            {
            //                                x = -1;
            //                                zar1.Image = null;
            //                            }
            //                            poz[ltemp].nr--;
            //                            if (poz[ltemp].nr == 0)
            //                                poz[ltemp].culoare = ' ';
            //                            if (l > 12 && poz[l].nr < 5)
            //                            {
            //                                poz_[l].Image = Image.FromFile(coloanaAlbe[poz[l].nr + 10]);
            //                                if (ltemp > 12 && poz[ltemp].nr < 5 && poz[ltemp].nr > 0)
            //                                    poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr + 10]);
            //                                else
            //                                    poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr]);
            //                            }
            //                            else
            //                            {
            //                                poz_[l].Image = Image.FromFile(coloanaAlbe[poz[l].nr]);
            //                                if (ltemp > 12 && poz[ltemp].nr < 5 && poz[ltemp].nr > 0)
            //                                    poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr + 10]);
            //                                else
            //                                    poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr]);
            //                            }//mutarea piesei
            //                        }
            //                        poz_0.BorderStyle = BorderStyle.None;
            //                        poz_25.BorderStyle = BorderStyle.None;
            //                        for (i = 1; i <= 24; i++)
            //                            poz_[i].BorderStyle = BorderStyle.None;
            //                    }
            //                }
            //                else
            //                {
            //                    if(l==ltemp-y-1)
            //                    {
            //                        if ((l == 0 && sa == 15) || (l == 0 && calba > 0 && sa+calba==15) )
            //                            {
            //                                calba++;
            //                                selected = false;
            //                                poz[ltemp].nr--;
            //                                if (poz[ltemp].nr == 0)
            //                                    poz[ltemp].culoare = ' ';
            //                                poz_0.Image = Image.FromFile(casaAlba[calba]);
            //                                poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr]);
            //                            if (zar2.BorderStyle == BorderStyle.Fixed3D)
            //                            {
            //                                zar2.BorderStyle = BorderStyle.None;
            //                            }
            //                            else
            //                            {
            //                                y = -1;
            //                                zar2.Image = null;
            //                            }
            //                            poz_0.BorderStyle = BorderStyle.None;
            //                                poz_25.BorderStyle = BorderStyle.None;
            //                                for (i = 1; i <= 24; i++)
            //                                    poz_[i].BorderStyle = BorderStyle.None;
            //                            }
            //                        else
            //                        {
            //                            if (poz[l].culoare == 'n')
            //                            {
            //                                if (poz[l].nr == 1)//dat afara
            //                                {
            //                                    selected = false;
            //                                    poz[l].culoare = poz[ltemp].culoare;
            //                                    poz[ltemp].nr--;
            //                                    afneagra++;
            //                                    if (poz[ltemp].nr == 0)
            //                                        poz[ltemp].culoare = ' ';
            //                                    Afara_Alb.Image = Image.FromFile(casaNeagra[afneagra]);
            //                                    if (zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                    {
            //                                        zar2.BorderStyle = BorderStyle.None;
            //                                    }
            //                                    else
            //                                    {
            //                                        y = -1;
            //                                        zar2.Image = null;
            //                                    }
            //                                    poz_0.BorderStyle = BorderStyle.None;
            //                                    poz_25.BorderStyle = BorderStyle.None;
            //                                    for (i = 1; i <= 24; i++)
            //                                        poz_[i].BorderStyle = BorderStyle.None;
            //                                    if (l > 12 && poz[l].nr < 5)
            //                                    {
            //                                        poz_[l].Image = Image.FromFile(coloanaAlbe[poz[l].nr + 10]);
            //                                        if (ltemp > 12 && poz[ltemp].nr < 5 && poz[ltemp].nr > 0)
            //                                            poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr + 10]);
            //                                        else
            //                                            poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr]);
            //                                    }
            //                                    else
            //                                    {
            //                                        poz_[l].Image = Image.FromFile(coloanaAlbe[poz[l].nr]);
            //                                        if (ltemp > 12 && poz[ltemp].nr < 5 && poz[ltemp].nr > 0)
            //                                            poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr + 10]);
            //                                        else
            //                                            poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr]);
            //                                    }//mutarea piesei
            //                                }
            //                                else
            //                                {
            //                                    MessageBox.Show("Culoare necorespunzatoare!");
            //                                    selected = false;
            //                                    poz_0.BorderStyle = BorderStyle.None;
            //                                    poz_25.BorderStyle = BorderStyle.None;
            //                                    for (i = 1; i <= 24; i++)
            //                                        poz_[i].BorderStyle = BorderStyle.None;
            //                                }
            //                            }
            //                            else
            //                            {
            //                                selected = false;
            //                                poz[l].culoare = poz[ltemp].culoare;
            //                                poz[l].nr++;
            //                                poz[ltemp].nr--;
            //                                if (zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                {
            //                                    zar2.BorderStyle = BorderStyle.None;
            //                                }
            //                                else
            //                                {
            //                                    y = -1;
            //                                    zar2.Image = null;
            //                                }
            //                                if (poz[ltemp].nr == 0)
            //                                    poz[ltemp].culoare = ' ';
            //                                if (l > 12 && poz[l].nr < 5)
            //                            {
            //                                poz_[l].Image = Image.FromFile(coloanaAlbe[poz[l].nr + 10]);
            //                                if (ltemp > 12 && poz[ltemp].nr < 5 && poz[ltemp].nr > 0)
            //                                    poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr + 10]);
            //                                else
            //                                    poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr]);
            //                            }
            //                                else
            //                            {
            //                                poz_[l].Image = Image.FromFile(coloanaAlbe[poz[l].nr]);
            //                                if (ltemp > 12 && poz[ltemp].nr < 5 && poz[ltemp].nr > 0)
            //                                    poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr + 10]);
            //                                else
            //                                    poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr]);
            //                            }//mutarea piesei
            //                            }
            //                            poz_0.BorderStyle = BorderStyle.None;
            //                            poz_25.BorderStyle = BorderStyle.None;
            //                            for (i = 1; i <= 24; i++)
            //                                poz_[i].BorderStyle = BorderStyle.None;
            //                        }
                                    
            //                    }
            //                    else
            //                    {
            //                        if (l == ltemp - x - y - 2)
            //                        {
            //                            if (poz[ltemp - x - 1].nr >= 2 && poz[ltemp - x - 1].culoare == 'n' && poz[ltemp - y - 1].nr >= 2 && poz[ltemp - y - 1].culoare == 'n')
            //                            {
            //                                MessageBox.Show("Mutare invalida!");
            //                                selected = false;
            //                                poz_0.BorderStyle = BorderStyle.None;
            //                                poz_25.BorderStyle = BorderStyle.None;
            //                                for (i = 1; i <= 24; i++)
            //                                    poz_[i].BorderStyle = BorderStyle.None;
            //                            }
            //                            else
            //                            {
            //                                if ((l == 0 && sa == 15) || (l == 0 && calba > 0 && sa + calba == 15))
            //                                {
            //                                    calba++;
            //                                    selected = false;
            //                                    poz[ltemp].nr--;
            //                                    if (poz[ltemp].nr == 0)
            //                                        poz[ltemp].culoare = ' ';
            //                                    poz_0.Image = Image.FromFile(casaAlba[calba]);
            //                                    poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr]);
            //                                    if (zar1.BorderStyle == BorderStyle.Fixed3D && zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                    {
            //                                        zar1.BorderStyle = BorderStyle.None;
            //                                        zar2.BorderStyle = BorderStyle.None;
            //                                    }
            //                                    else
            //                                    {
            //                                        if (zar1.BorderStyle == BorderStyle.Fixed3D)
            //                                        {
            //                                            zar1.BorderStyle = BorderStyle.None;
            //                                            y = -1; zar2.Image = null;
            //                                        }
            //                                        else
            //                                        {
            //                                            if (zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                            {
            //                                                x = -1; zar1.Image = null;
            //                                                zar2.BorderStyle = BorderStyle.None;
            //                                            }
            //                                            else
            //                                            {
            //                                                x = -1; zar1.Image = null;
            //                                                y = -1; zar2.Image = null;
            //                                            }
            //                                        }
            //                                    }
            //                                    poz_0.BorderStyle = BorderStyle.None;
            //                                    poz_25.BorderStyle = BorderStyle.None;
            //                                    for (i = 1; i <= 24; i++)
            //                                        poz_[i].BorderStyle = BorderStyle.None;
            //                                }
            //                                else
            //                                {
            //                                    if (poz[l].culoare == 'n')
            //                                    {
            //                                        if (poz[l].nr == 1)//dat afara
            //                                        {
            //                                            selected = false;
            //                                            poz[l].culoare = poz[ltemp].culoare;
            //                                            poz[ltemp].nr--;
            //                                            afneagra++;
            //                                            if (poz[ltemp].nr == 0)
            //                                                poz[ltemp].culoare = ' ';
            //                                            Afara_Alb.Image = Image.FromFile(casaNeagra[afneagra]);
            //                                            if (zar1.BorderStyle == BorderStyle.Fixed3D && zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                            {
            //                                                zar1.BorderStyle = BorderStyle.None;
            //                                                zar2.BorderStyle = BorderStyle.None;
            //                                            }
            //                                            else
            //                                            {
            //                                                if (zar1.BorderStyle == BorderStyle.Fixed3D)
            //                                                {
            //                                                    zar1.BorderStyle = BorderStyle.None;
            //                                                    y = -1; zar2.Image = null;
            //                                                }
            //                                                else
            //                                                {
            //                                                    if (zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                                    {
            //                                                        x = -1; zar1.Image = null;
            //                                                        zar2.BorderStyle = BorderStyle.None;
            //                                                    }
            //                                                    else
            //                                                    {
            //                                                        x = -1; zar1.Image = null;
            //                                                        y = -1; zar2.Image = null;
            //                                                    }
            //                                                }
            //                                            }
            //                                            poz_0.BorderStyle = BorderStyle.None;
            //                                            poz_25.BorderStyle = BorderStyle.None;
            //                                            for (i = 1; i <= 24; i++)
            //                                                poz_[i].BorderStyle = BorderStyle.None;
            //                                            if (l > 12 && poz[l].nr < 5)
            //                                            {
            //                                                poz_[l].Image = Image.FromFile(coloanaAlbe[poz[l].nr + 10]);
            //                                                if (ltemp > 12 && poz[ltemp].nr < 5 && poz[ltemp].nr > 0)
            //                                                    poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr + 10]);
            //                                                else
            //                                                    poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr]);
            //                                            }
            //                                            else
            //                                            {
            //                                                poz_[l].Image = Image.FromFile(coloanaAlbe[poz[l].nr]);
            //                                                if (ltemp > 12 && poz[ltemp].nr < 5 && poz[ltemp].nr > 0)
            //                                                    poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr + 10]);
            //                                                else
            //                                                    poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr]);
            //                                            }//mutarea piesei
            //                                        }
            //                                        else
            //                                        {
            //                                            MessageBox.Show("Culoare necorespunzatoare!");
            //                                            selected = false;
            //                                            poz_0.BorderStyle = BorderStyle.None;
            //                                            poz_25.BorderStyle = BorderStyle.None;
            //                                            for (i = 1; i <= 24; i++)
            //                                                poz_[i].BorderStyle = BorderStyle.None;
            //                                        }
            //                                    }
            //                                    else
            //                                    {
            //                                        selected = false;
            //                                        poz[l].culoare = poz[ltemp].culoare;
            //                                        poz[l].nr++;
            //                                        poz[ltemp].nr--;
            //                                        if (zar1.BorderStyle == BorderStyle.Fixed3D && zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                        {
            //                                            zar1.BorderStyle = BorderStyle.None;
            //                                            zar2.BorderStyle = BorderStyle.None;
            //                                        }
            //                                        else
            //                                        {
            //                                            if (zar1.BorderStyle == BorderStyle.Fixed3D)
            //                                            {
            //                                                zar1.BorderStyle = BorderStyle.None;
            //                                                y = -1; zar2.Image = null;
            //                                            }
            //                                            else
            //                                            {
            //                                                if (zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                                {
            //                                                    x = -1; zar1.Image = null;
            //                                                    zar2.BorderStyle = BorderStyle.None;
            //                                                }
            //                                                else
            //                                                {
            //                                                    x = -1; zar1.Image = null;
            //                                                    y = -1; zar2.Image = null;
            //                                                }
            //                                            }
            //                                        }
            //                                        if (poz[ltemp].nr == 0)
            //                                            poz[ltemp].culoare = ' ';
            //                                        if (l > 12 && poz[l].nr < 5)
            //                                        {
            //                                            poz_[l].Image = Image.FromFile(coloanaAlbe[poz[l].nr + 10]);
            //                                            if (ltemp > 12 && poz[ltemp].nr < 5 && poz[ltemp].nr > 0)
            //                                                poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr + 10]);
            //                                            else
            //                                                poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr]);
            //                                        }
            //                                        else
            //                                        {
            //                                            poz_[l].Image = Image.FromFile(coloanaAlbe[poz[l].nr]);
            //                                            if (ltemp > 12 && poz[ltemp].nr < 5 && poz[ltemp].nr > 0)
            //                                                poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr + 10]);
            //                                            else
            //                                                poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr]);
            //                                        }//mutarea piesei
            //                                    }
            //                                    poz_0.BorderStyle = BorderStyle.None;
            //                                    poz_25.BorderStyle = BorderStyle.None;
            //                                    for (i = 1; i <= 24; i++)
            //                                        poz_[i].BorderStyle = BorderStyle.None;
            //                                }
            //                            }
            //                        }
            //                        else
            //                        {
            //                            if (sa + calba == 15 && ltemp - x - 1 < 1 && l == 0)
            //                            {
            //                                calba++;
            //                                selected = false;
            //                                poz[ltemp].nr--;
            //                                if (poz[ltemp].nr == 0)
            //                                    poz[ltemp].culoare = ' ';
            //                                poz_0.Image = Image.FromFile(casaAlba[calba]);
            //                                poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr]);
            //                                if (zar1.BorderStyle == BorderStyle.Fixed3D)
            //                                {
            //                                    zar1.BorderStyle = BorderStyle.None;
            //                                }
            //                                else
            //                                {
            //                                    x = -1;
            //                                    zar1.Image = null;
            //                                }
            //                                poz_0.BorderStyle = BorderStyle.None;
            //                                poz_25.BorderStyle = BorderStyle.None;
            //                                for (i = 1; i <= 24; i++)
            //                                    poz_[i].BorderStyle = BorderStyle.None;
            //                            }
            //                            else
            //                            {
            //                                if (sa + calba == 15 && ltemp - y - 1 < 1 && l == 0)
            //                                {
            //                                    calba++;
            //                                    selected = false;
            //                                    poz[ltemp].nr--;
            //                                    if (poz[ltemp].nr == 0)
            //                                        poz[ltemp].culoare = ' ';
            //                                    poz_0.Image = Image.FromFile(casaAlba[calba]);
            //                                    poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr]);
            //                                    if (zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                    {
            //                                        zar2.BorderStyle = BorderStyle.None;
            //                                    }
            //                                    else
            //                                    {
            //                                        y = -1;
            //                                        zar2.Image = null;
            //                                    }
            //                                    poz_0.BorderStyle = BorderStyle.None;
            //                                    poz_25.BorderStyle = BorderStyle.None;
            //                                    for (i = 1; i <= 24; i++)
            //                                        poz_[i].BorderStyle = BorderStyle.None;
            //                                }
            //                                else
            //                                {
            //                                    if (sa + calba == 15 && ltemp - x - y - 2 < 1 && l == 0)
            //                                    {
            //                                        if (poz[ltemp - x - 1].nr >= 2 && poz[ltemp - x - 1].culoare == 'n' && poz[ltemp - y - 1].nr >= 2 && poz[ltemp - y - 1].culoare == 'n' && l!=ltemp-x-y-2)
            //                                        {
            //                                            MessageBox.Show("Mutare invalida!");
            //                                            selected = false;
            //                                            poz_0.BorderStyle = BorderStyle.None;
            //                                            poz_25.BorderStyle = BorderStyle.None;
            //                                            for (i = 1; i <= 24; i++)
            //                                                poz_[i].BorderStyle = BorderStyle.None;
            //                                        }
            //                                        else
            //                                        {
            //                                            calba++;
            //                                            selected = false;
            //                                            poz[ltemp].nr--;
            //                                            if (poz[ltemp].nr == 0)
            //                                                poz[ltemp].culoare = ' ';
            //                                            poz_0.Image = Image.FromFile(casaAlba[calba]);
            //                                            poz_[ltemp].Image = Image.FromFile(coloanaAlbe[poz[ltemp].nr]);
            //                                            if (zar1.BorderStyle == BorderStyle.Fixed3D && zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                            {
            //                                                zar1.BorderStyle = BorderStyle.None;
            //                                                zar2.BorderStyle = BorderStyle.None;
            //                                            }
            //                                            else
            //                                            {
            //                                                if (zar1.BorderStyle == BorderStyle.Fixed3D)
            //                                                {
            //                                                    zar1.BorderStyle = BorderStyle.None;
            //                                                    y = -1; zar2.Image = null;
            //                                                }
            //                                                else
            //                                                {
            //                                                    if (zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                                    {
            //                                                        x = -1; zar1.Image = null;
            //                                                        zar2.BorderStyle = BorderStyle.None;
            //                                                    }
            //                                                    else
            //                                                    {
            //                                                        x = -1; zar1.Image = null;
            //                                                        y = -1; zar2.Image = null;
            //                                                    }
            //                                                }
            //                                            }
            //                                            poz_0.BorderStyle = BorderStyle.None;
            //                                            poz_25.BorderStyle = BorderStyle.None;
            //                                            for (i = 1; i <= 24; i++)
            //                                                poz_[i].BorderStyle = BorderStyle.None;
            //                                        }
            //                                    }
            //                                    else
            //                                    {
            //                                        MessageBox.Show("Mutare invalida!");
            //                                        selected = false;
            //                                        poz_0.BorderStyle = BorderStyle.None;
            //                                        poz_25.BorderStyle = BorderStyle.None;
            //                                        for (i = 1; i <= 24; i++)
            //                                            poz_[i].BorderStyle = BorderStyle.None;

            //                                    }
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }//piesa alba
            //        }
            //        else
            //        {
            //            if(ltemp==0)
            //            {
            //                if(l==ltemp+x+1)
            //                {
            //                    if (poz[l].culoare == 'a')
            //                    {
            //                        if (poz[l].nr == 1)//dat afara
            //                        {
            //                            selected = false;
            //                            poz[l].culoare = 'n';
            //                            afneagra--;
            //                            afalba++;
            //                            Afara_Negru.Image = Image.FromFile(casaAlba[afalba]);
            //                            Afara_Alb.Image = Image.FromFile(casaNeagra[afneagra]);
            //                            poz_[l].Image = Image.FromFile(coloanaNegre[1]);
            //                            if (zar1.BorderStyle == BorderStyle.Fixed3D)
            //                        {
            //                            zar1.BorderStyle = BorderStyle.None;
            //                        }
            //                            else
            //                        {
            //                            x = -1;
            //                            zar1.Image = null;
            //                        }
            //                            poz_0.BorderStyle = BorderStyle.None;
            //                            poz_25.BorderStyle = BorderStyle.None;
            //                            for (i = 1; i <= 24; i++)
            //                                poz_[i].BorderStyle = BorderStyle.None;
            //                        }//mutarea piesei
            //                        else
            //                        {
            //                            MessageBox.Show("Culoare necorespunzatoare!");
            //                            selected = false;
            //                        }
            //                    }
            //                    else
            //                    {
            //                        selected = false;
            //                        poz[l].culoare = 'n';
            //                        poz[l].nr++;
            //                        afneagra--;
            //                        poz_[l].Image = Image.FromFile(coloanaNegre[poz[l].nr]);
            //                        Afara_Alb.Image = Image.FromFile(casaNeagra[afneagra]);
            //                        if (zar1.BorderStyle == BorderStyle.Fixed3D)
            //                        {
            //                            zar1.BorderStyle = BorderStyle.None;
            //                        }
            //                        else
            //                        {
            //                            x = -1;
            //                            zar1.Image = null;
            //                        }
            //                    }
                                
            //                }
            //                else
            //                {
            //                    if (l == ltemp + y + 1)
            //                    {
            //                        if (poz[l].culoare == 'a')
            //                        {
            //                            if (poz[l].nr == 1)//dat afara
            //                            {
            //                                selected = false;
            //                                poz[l].culoare = 'n';
            //                                afneagra--;
            //                                afalba++;
            //                                Afara_Negru.Image = Image.FromFile(casaAlba[afalba]);
            //                                Afara_Alb.Image = Image.FromFile(casaNeagra[afneagra]);
            //                                poz_[l].Image = Image.FromFile(coloanaNegre[1]);
            //                            if (zar2.BorderStyle == BorderStyle.Fixed3D)
            //                            {
            //                                zar2.BorderStyle = BorderStyle.None;
            //                        }
            //                            else
            //                                {
            //                                    y = -1;
            //                                    zar2.Image = null;
            //                                }
            //                            poz_0.BorderStyle = BorderStyle.None;
            //                                poz_25.BorderStyle = BorderStyle.None;
            //                                for (i = 1; i <= 24; i++)
            //                                    poz_[i].BorderStyle = BorderStyle.None;
            //                            }//mutarea piesei
            //                            else
            //                                {
            //                                    MessageBox.Show("Culoare necorespunzatoare!");
            //                                    selected = false;
            //                                }
            //                        }
            //                        else
            //                        {
            //                            selected = false;
            //                            poz[l].culoare = 'n';
            //                            poz[l].nr++;
            //                            afneagra--;
            //                            poz_[l].Image = Image.FromFile(coloanaNegre[poz[l].nr]);
            //                            Afara_Alb.Image = Image.FromFile(casaNeagra[afneagra]);
            //                        if (zar2.BorderStyle == BorderStyle.Fixed3D)
            //                        {
            //                            zar2.BorderStyle = BorderStyle.None;
            //                        }
            //                        else
            //                        {
            //                            y = -1;
            //                            zar2.Image = null;
            //                        }
            //                    }
            //                    }
            //                    else
            //                    {
            //                        if (l == ltemp + x + y + 2)
            //                        {
            //                            if (poz[ltemp + x + 1].nr >= 2 && poz[ltemp + x + 1].culoare == 'a' && poz[ltemp + y + 1].nr >= 2 && poz[ltemp + y + 1].culoare == 'a')
            //                            {
            //                                MessageBox.Show("Mutare invalida!");
            //                                selected = false;
            //                                poz_0.BorderStyle = BorderStyle.None;
            //                                poz_25.BorderStyle = BorderStyle.None;
            //                                for (i = 1; i <= 24; i++)
            //                                    poz_[i].BorderStyle = BorderStyle.None;
            //                            }
            //                            else
            //                            {
            //                                if (poz[l].culoare == 'a')
            //                                {
            //                                    if (poz[l].nr == 1)//dat afara
            //                                    {
            //                                        selected = false;
            //                                        poz[l].culoare = 'n';
            //                                        afneagra--;
            //                                        afalba++;
            //                                        Afara_Negru.Image = Image.FromFile(casaAlba[afalba]);
            //                                        Afara_Alb.Image = Image.FromFile(casaNeagra[afneagra]);
            //                                        poz_[l].Image = Image.FromFile(coloanaNegre[1]);
            //                                        if (zar1.BorderStyle == BorderStyle.Fixed3D && zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                        {
            //                                            zar1.BorderStyle = BorderStyle.None;
            //                                            zar2.BorderStyle = BorderStyle.None;
            //                                        }
            //                                        else
            //                                        {
            //                                            if (zar1.BorderStyle == BorderStyle.Fixed3D)
            //                                            {
            //                                                zar1.BorderStyle = BorderStyle.None;
            //                                                y = -1; zar2.Image = null;
            //                                            }
            //                                            else
            //                                            {
            //                                                if (zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                                {
            //                                                    x = -1; zar1.Image = null;
            //                                                    zar2.BorderStyle = BorderStyle.None;
            //                                                }
            //                                                else
            //                                                {
            //                                                    x = -1; zar1.Image = null;
            //                                                    y = -1; zar2.Image = null;
            //                                                }
            //                                            }
            //                                        }
            //                                        poz_0.BorderStyle = BorderStyle.None;
            //                                        poz_25.BorderStyle = BorderStyle.None;
            //                                        for (i = 1; i <= 24; i++)
            //                                            poz_[i].BorderStyle = BorderStyle.None;
            //                                    }//mutarea piesei
            //                                    else
            //                                    {
            //                                        MessageBox.Show("Culoare necorespunzatoare!");
            //                                        selected = false;
            //                                    }
            //                                }
            //                                else
            //                                {
            //                                    selected = false;
            //                                    poz[l].culoare = 'n';
            //                                    poz[l].nr++;
            //                                    afneagra--;
            //                                    poz_[l].Image = Image.FromFile(coloanaNegre[poz[l].nr]);
            //                                    Afara_Alb.Image = Image.FromFile(casaNeagra[afneagra]);
            //                                    if (zar1.BorderStyle == BorderStyle.Fixed3D && zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                    {
            //                                        zar1.BorderStyle = BorderStyle.None;
            //                                        zar2.BorderStyle = BorderStyle.None;
            //                                    }
            //                                    else
            //                                    {
            //                                        if (zar1.BorderStyle == BorderStyle.Fixed3D)
            //                                        {
            //                                            zar1.BorderStyle = BorderStyle.None;
            //                                            y = -1; zar2.Image = null;
            //                                        }
            //                                        else
            //                                        {
            //                                            if (zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                            {
            //                                                x = -1; zar1.Image = null;
            //                                                zar2.BorderStyle = BorderStyle.None;
            //                                            }
            //                                            else
            //                                            {
            //                                                x = -1; zar1.Image = null;
            //                                                y = -1; zar2.Image = null;
            //                                            }
            //                                        }
            //                                    }
            //                                }
            //                            }
            //                        }
            //                        else
            //                            {
            //                                MessageBox.Show("Mutare invalida!");
            //                                selected = false;
            //                                poz_0.BorderStyle = BorderStyle.None;
            //                                poz_25.BorderStyle = BorderStyle.None;
            //                                for (i = 1; i <= 24; i++)
            //                                    poz_[i].BorderStyle = BorderStyle.None;
            //                            }
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                if (l == ltemp - x - 1)
            //                {
            //                    if (poz[l].culoare == 'n')
            //                    {
            //                        if (poz[l].nr == 1)//dat afara
            //                        {
            //                            selected = false;
            //                            poz[l].culoare = 'a';
            //                            afneagra++;
            //                            afalba--;
            //                            Afara_Negru.Image = Image.FromFile(casaAlba[afalba]);
            //                            Afara_Alb.Image = Image.FromFile(casaNeagra[afneagra]);
            //                            poz_[l].Image = Image.FromFile(coloanaAlbe[11]);
            //                        if (zar1.BorderStyle == BorderStyle.Fixed3D)
            //                        {
            //                            zar1.BorderStyle = BorderStyle.None;
            //                    }
            //                        else
            //                        {
            //                            x = -1;
            //                            zar1.Image = null;
            //                        }
            //                        poz_0.BorderStyle = BorderStyle.None;
            //                            poz_25.BorderStyle = BorderStyle.None;
            //                            for (i = 1; i <= 24; i++)
            //                                poz_[i].BorderStyle = BorderStyle.None;
            //                        }//mutarea piesei
            //                        else
            //                        {
            //                            MessageBox.Show("Culoare necorespunzatoare!");
            //                            selected = false;
            //                        }
            //                    }
            //                    else
            //                    {
            //                        selected = false;
            //                        poz[l].culoare = 'a';
            //                        poz[l].nr++;
            //                        afalba--;
            //                        poz_[l].Image = Image.FromFile(coloanaAlbe[poz[l].nr+10]);
            //                        Afara_Negru.Image = Image.FromFile(casaAlba[afalba]);
            //                    if (zar1.BorderStyle == BorderStyle.Fixed3D)
            //                    {
            //                        zar1.BorderStyle = BorderStyle.None;
            //                    }
            //                    else
            //                    {
            //                        x = -1;
            //                        zar1.Image = null;
            //                    }
            //                }

            //                }
            //                else
            //                {
            //                    if (l == ltemp - y - 1)
            //                    {
            //                        if (poz[l].culoare == 'n')
            //                        {
            //                            if (poz[l].nr == 1)//dat afara
            //                            {
            //                                selected = false;
            //                                poz[l].culoare = 'a';
            //                                afneagra++;
            //                                afalba--;
            //                                Afara_Negru.Image = Image.FromFile(casaAlba[afalba]);
            //                                Afara_Alb.Image = Image.FromFile(casaNeagra[afneagra]);
            //                                poz_[l].Image = Image.FromFile(coloanaAlbe[11]);
            //                            if (zar2.BorderStyle == BorderStyle.Fixed3D)
            //                            {
            //                                zar2.BorderStyle = BorderStyle.None;
            //                        }
            //                            else
            //                            {
            //                                y = -1;
            //                                zar2.Image = null;
            //                            }
            //                            poz_0.BorderStyle = BorderStyle.None;
            //                                poz_25.BorderStyle = BorderStyle.None;
            //                                for (i = 1; i <= 24; i++)
            //                                    poz_[i].BorderStyle = BorderStyle.None;
            //                            }//mutarea piesei
            //                            else
            //                            {
            //                                MessageBox.Show("Culoare necorespunzatoare!");
            //                                selected = false;
            //                            }
            //                        }
            //                        else
            //                        {
            //                            selected = false;
            //                            poz[l].culoare = 'a';
            //                            poz[l].nr++;
            //                            afalba--;
            //                            poz_[l].Image = Image.FromFile(coloanaAlbe[poz[l].nr+10]);
            //                            Afara_Negru.Image = Image.FromFile(casaAlba[afalba]);
            //                        if (zar2.BorderStyle == BorderStyle.Fixed3D)
            //                        {
            //                            zar2.BorderStyle = BorderStyle.None;
            //                        }
            //                        else
            //                        {
            //                            y = -1;
            //                            zar2.Image = null;
            //                        }
            //                    }
            //                    }
            //                    else
            //                    {
            //                        if (l == ltemp - x - y - 2)
            //                        {
            //                            if (poz[ltemp - x - 1].nr >= 2 && poz[ltemp - x - 1].culoare == 'n' && poz[ltemp - y - 1].nr >= 2 && poz[ltemp - y - 1].culoare == 'n')
            //                            {
            //                                MessageBox.Show("Mutare invalida!");
            //                                selected = false;
            //                                poz_0.BorderStyle = BorderStyle.None;
            //                                poz_25.BorderStyle = BorderStyle.None;
            //                                for (i = 1; i <= 24; i++)
            //                                    poz_[i].BorderStyle = BorderStyle.None;
            //                            }
            //                            else
            //                            {
            //                                if (poz[l].culoare == 'n')
            //                                {
            //                                    if (poz[l].nr == 1)//dat afara
            //                                    {
            //                                        selected = false;
            //                                        poz[l].culoare = 'a';
            //                                        afneagra++;
            //                                        afalba--;
            //                                        Afara_Negru.Image = Image.FromFile(casaAlba[afalba]);
            //                                        Afara_Alb.Image = Image.FromFile(casaNeagra[afneagra]);
            //                                        poz_[l].Image = Image.FromFile(coloanaNegre[11]);
            //                                        if (zar1.BorderStyle == BorderStyle.Fixed3D && zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                        {
            //                                            zar1.BorderStyle = BorderStyle.None;
            //                                            zar2.BorderStyle = BorderStyle.None;
            //                                        }
            //                                        else
            //                                        {
            //                                            if (zar1.BorderStyle == BorderStyle.Fixed3D)
            //                                            {
            //                                                zar1.BorderStyle = BorderStyle.None;
            //                                                y = -1; zar2.Image = null;
            //                                            }
            //                                            else
            //                                            {
            //                                                if (zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                                {
            //                                                    x = -1; zar1.Image = null;
            //                                                    zar2.BorderStyle = BorderStyle.None;
            //                                                }
            //                                                else
            //                                                {
            //                                                    x = -1; zar1.Image = null;
            //                                                    y = -1; zar2.Image = null;
            //                                                }
            //                                            }
            //                                        }
            //                                        poz_0.BorderStyle = BorderStyle.None;
            //                                        poz_25.BorderStyle = BorderStyle.None;
            //                                        for (i = 1; i <= 24; i++)
            //                                            poz_[i].BorderStyle = BorderStyle.None;
            //                                    }//mutarea piesei
            //                                    else
            //                                    {
            //                                        MessageBox.Show("Culoare necorespunzatoare!");
            //                                        selected = false;
            //                                    }
            //                                }
            //                                else
            //                                {
            //                                    selected = false;
            //                                    poz[l].culoare = 'a';
            //                                    poz[l].nr++;
            //                                    afalba--;
            //                                    poz_[l].Image = Image.FromFile(coloanaAlbe[poz[l].nr + 10]);
            //                                    Afara_Negru.Image = Image.FromFile(casaAlba[afalba]);
            //                                    if (zar1.BorderStyle == BorderStyle.Fixed3D && zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                    {
            //                                        zar1.BorderStyle = BorderStyle.None;
            //                                        zar2.BorderStyle = BorderStyle.None;
            //                                    }
            //                                    else
            //                                    {
            //                                        if (zar1.BorderStyle == BorderStyle.Fixed3D)
            //                                        {
            //                                            zar1.BorderStyle = BorderStyle.None;
            //                                            y = -1; zar2.Image = null;
            //                                        }
            //                                        else
            //                                        {
            //                                            if (zar2.BorderStyle == BorderStyle.Fixed3D)
            //                                            {
            //                                                x = -1; zar1.Image = null;
            //                                                zar2.BorderStyle = BorderStyle.None;
            //                                            }
            //                                            else
            //                                            {
            //                                                x = -1; zar1.Image = null;
            //                                                y = -1; zar2.Image = null;
            //                                            }
            //                                        }
            //                                    }
            //                                }
            //                            }
            //                        }
            //                        else
            //                            {
            //                                MessageBox.Show("Mutare invalida!");
            //                                selected = false;
            //                                poz_0.BorderStyle = BorderStyle.None;
            //                                poz_25.BorderStyle = BorderStyle.None;
            //                                for (i = 1; i <= 24; i++)
            //                                    poz_[i].BorderStyle = BorderStyle.None;
            //                            }
            //                    }
            //                }
            //            }
            //            poz_0.BorderStyle = BorderStyle.None;
            //            poz_25.BorderStyle = BorderStyle.None;
            //            for (i = 1; i <= 24; i++)
            //                poz_[i].BorderStyle = BorderStyle.None;
            //        }
            //    }//piesa deja selectata
            //}
            Castigator();
        }
        private void zar_Click(object sender, EventArgs e)
        {
            if(turn.Text == "Aici se va afisa randul")
            {
                MessageBox.Show("Jocul nu a inceput inca!");
            }
            else
            {
                if (x == 0 && y == 0)
                {
                    x = rnd.Next(1, 7); y = rnd.Next(1, 7);//se genereaza zarurile
                    //se atribuie imaginile corespunzatoare
                    zar1.Image = Image.FromFile(zaruri[x]);
                    zar2.Image = Image.FromFile(zaruri[y]);
                    //se verifica daca este zar dublu
                    if (x == y)
                    {
                        zar1.BorderStyle = BorderStyle.Fixed3D;
                        zar2.BorderStyle = BorderStyle.Fixed3D;
                    }
                    //se atribuie randul
                    if (rand)
                    {
                        turn.Text = "Este randul albelor"; randul = 'a';
                        rand = false;
                    }
                    else
                    {
                        turn.Text = "Este randul negrelor"; randul = 'n';
                        rand = true;
                    }
                }
                else
                    MessageBox.Show("Nu s-au efectuat toate mutarile!");
            }
        }
        //Strip menus
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void despreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Jocul este creat de Cojanu Andrei");
        }
        //Strip menu Ajutor
        private void CumSeJoacaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Obiectivul este sa aduceti toate cele 15 piese in „casa” si apoi sa le scoateti de pe tabla de joc inaintea adversarului. Cel care reuseste sa scoata toate piesele primul este castigator.Zarul hotaraste cate casute (triunghiuri) veti muta.");
        }

        private void UndeSeMutaPieseleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Puteti muta o piesa intr - o casuta libera, in una care are o singura piesa a adversarului sau in una in care deja aveti piese. Daca casuta contine doua sau mai multe piese ale adversarului, nu puteti sa mutati o piesa in acel loc.");
        }

        private void EliminareaPieselorAdversaruluiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Puteti elimina o piesa a adversarului daca aceasta se afla singura intr-o casuta pe care o traversati.");
        }

        private void ReintrareaInJocToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Pentru a reintra in joc, va trebui sa porniti din „casa” adversarului, acolo unde puteti patrunde doar in spatiile libere, in cele in care deja aveti piese sau intr-o casuta in care adversarul are o singura piesa.");
        }

        private void CastigatorulToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Jocul se incheie atunci cand ati reusit sa scoateti toate piesele pe care le-ati adus anterior in „casa” dumneavoastra.");
        }

        private void ZarulDubluToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Daca ambele zaruri arata aceeasi valoare inseamna ca ati dat o dubla. Aveti dreptul la 4 mutari. ");
        }

        public void jocNouToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i;
            for (i = 0; i <= 25; i++)
            {
                poz[i].nr = 0;
                poz[i].culoare = ' ';
                poz_[i].Image = null;
            }
            poz[0].culoare = 'a';
            poz[25].culoare = 'n';
            turn.Text = "Este randul albelor";
            zar1.Image = null; x = 0;
            zar2.Image = null; y = 0;
            Afara_Negru.Image = Image.FromFile(casaAlba[0]);
            Afara_Alb.Image = Image.FromFile(casaNeagra[0]);
            poz_0.Image = Image.FromFile(casaAlba[0]); calba = 0;
            //poz_[1].Image = Image.FromFile(coloanaNegre[2]); poz[1].nr = 2; poz[1].culoare = 'n';
            //poz_[6].Image = Image.FromFile(coloanaAlbe[5]); poz[6].nr = 5; poz[6].culoare = 'a';
            //poz_[8].Image = Image.FromFile(coloanaAlbe[3]); poz[8].nr = 3; poz[8].culoare = 'a';
            //poz_[12].Image = Image.FromFile(coloanaNegre[5]); poz[12].nr = 5; poz[12].culoare = 'n';
            //poz_[13].Image = Image.FromFile(coloanaAlbe[5]); poz[13].nr = 5; poz[13].culoare = 'a';
            //poz_[17].Image = Image.FromFile(coloanaNegre[13]); poz[17].nr = 3; poz[17].culoare = 'n';
            //poz_[19].Image = Image.FromFile(coloanaNegre[5]); poz[19].nr = 5; poz[19].culoare = 'n';
            //poz_[24].Image = Image.FromFile(coloanaAlbe[12]); poz[24].nr = 2; poz[24].culoare = 'a';
            poz_[1].Image = Image.FromFile(coloanaAlbe[5]); poz[1].nr = 5; poz[1].culoare = 'a';
            poz_[3].Image = Image.FromFile(coloanaNegre[1]); poz[3].nr = 1; poz[3].culoare = 'n';
            poz_[6].Image = Image.FromFile(coloanaAlbe[10]); poz[6].nr = 10; poz[6].culoare = 'a';
            poz_[19].Image = Image.FromFile(coloanaNegre[9]); poz[19].nr = 9; poz[19].culoare = 'n';
            poz_[24].Image = Image.FromFile(coloanaNegre[5]); poz[24].nr = 5; poz[24].culoare = 'n';
            poz_25.Image = Image.FromFile(casaNeagra[0]); cneagra = 0;
        }
        //End of Ajutor strip menu
    }
}
