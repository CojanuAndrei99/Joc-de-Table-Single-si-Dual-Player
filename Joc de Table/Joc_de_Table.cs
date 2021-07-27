using proiect.bin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace joc_table
{
    public partial class Joc_de_Table : Form
    {
        public string[] pathColoanaNegre,
                         pathColoanaAlbe,
                         pathZaruri,
                         pathCasaAlba,
                         pathCasaNeagra;
        public int afaraAlb, afaraNegru, lSelected;
        public Zaruri zar;
        public bool selected = false, rand = true, game_start = false,single_player;//rand=0=albe   rand=1=negre
        public Loc[] poz = new Loc[26];
        public Cursor cursorNegru = new Cursor(Application.StartupPath + "\\piesa_neagra.cur"), cursorAlb = new Cursor(Application.StartupPath + "\\piesa_alba.cur");
        public SortedList<int, Tuple<int,int,int>> prioritateMutare=new SortedList<int, Tuple<int, int, int>>(new DuplicateKeyComparer<int>());

        public class DuplicateKeyComparer<TKey>
                :
             IComparer<TKey> where TKey : IComparable
        {
            public int Compare(TKey x, TKey y)
            {
                int result = x.CompareTo(y);

                if (result == 0)
                    return -1;   // Handle equality as beeing greater
                else
                    return result;
            }

        }
        
        //funtiile de initializare
        public Joc_de_Table()
        {
            int i;
            PictureBox p;
            //MessageBox.Show(Application.StartupPath);
            InitializeComponent();
            pathColoanaNegre = new string[20];
            pathColoanaAlbe = new string[20];
            //initializare cai coloanaNegre coloanaAlbe
            for (i = 0; i < 20; i++)
            {
                if (i > 15)

                {
                    pathColoanaNegre[i] = Environment.CurrentDirectory + "/locasuri/negru/" + (i - 15) + "r.png";
                    pathColoanaAlbe[i] = Environment.CurrentDirectory + "/locasuri/alb/" + (i - 15) + "r.png";
                }
                else
                {
                    pathColoanaNegre[i] = Environment.CurrentDirectory + "/locasuri/negru/" + i + ".png";
                    pathColoanaAlbe[i] = Environment.CurrentDirectory + "/locasuri/alb/" + i + ".png";
                }
            }

            pathCasaAlba = new string[16];
            pathCasaNeagra = new string[16];
            //initializare cai casaAlba casaNeagra
            for (i = 0; i < 16; i++)
            {
                pathCasaAlba[i] = Environment.CurrentDirectory + "/casa/alb/" + i + ".png";
                pathCasaNeagra[i] = Environment.CurrentDirectory + "/casa/negru/" + i + ".png";
            }

            //initializare cai zaruri
            pathZaruri = new string[7];
            pathZaruri[0] = null;
            for (i = 1; i <= 6; i++)
            {
                pathZaruri[i] = Environment.CurrentDirectory + "/zaruri/" + i + ".png";
            }
            zar = new Zaruri(pathZaruri);
            //actualizarea matricei de ImageBox
            for (i = 1; i <= 24; i++)
            {
                p = new PictureBox();
                p.Parent = tabla;
                if (i <= 6)
                    p.SetBounds(20 + (6 - i) * 50 + 340, 230, 50, 150);
                if (i > 6 && i <= 12)
                    p.SetBounds(20 + (12 - i) * 50, 230, 50, 150);
                if (i > 12 && i <= 18)
                    p.SetBounds(20 + (i - 13) * 50, 20, 50, 150);
                if (i > 18 && i <= 24)
                    p.SetBounds(20 + (i - 19) * 50 + 340, 20, 50, 150);
                p.Name = "poz_" + i;
                p.BackColor = Color.Transparent;
                p.Image = Image.FromFile(pathColoanaAlbe[0]);
                p.BringToFront();
                p.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Tri_Click);
                p.MouseHover += new System.EventHandler(this.tri_MouseHover);
                p.MouseLeave += new System.EventHandler(this.tri_MouseLeave);
                poz[i] = new Loc(null, 0, p);
            }
            //init zar
            zar.zar1Image = zar1;
            zar.zar2Image = zar2;


            //actualizare case
            Afara_Alb.Image = Image.FromFile(pathCasaAlba[0]);
            Afara_Negru.Image = Image.FromFile(pathCasaNeagra[0]);
            poz[0] = new Loc(Culoare.ALB, 0, poz_0);
            poz[25] = new Loc(Culoare.NEGRU, 0, poz_25);
            poz[0].imagine.Image = Image.FromFile(pathCasaAlba[0]);
            poz[25].imagine.Image = Image.FromFile(pathCasaNeagra[0]);

            turn.Text = "Aici se va afisa randul";
        }
        public void joc_in_doi()
        {
            single_player = false;
            game_start = true;
            afaraAlb = 0;
            afaraNegru = 0;
            turn.Text = "Este randul albelor";
            rand = true;
            zar.resetZar1();
            zar.resetZar2();

            Afara_Alb.Image = Image.FromFile(pathCasaAlba[0]);
            Afara_Negru.Image = Image.FromFile(pathCasaNeagra[0]);
            poz[0].locSet(Culoare.ALB, 0, Image.FromFile(pathCasaAlba[0]));
            poz[25].locSet(Culoare.NEGRU, 0, Image.FromFile(pathCasaNeagra[0]));

            for (int i = 1; i <= 24; i++)
            {
                poz[i].locSet(null, 0, Image.FromFile(pathColoanaAlbe[0]));
            }

            poz[1].locSet(Culoare.NEGRU, 2, Image.FromFile(pathColoanaNegre[2]));
            poz[6].locSet(Culoare.ALB, 5, Image.FromFile(pathColoanaAlbe[5]));
            poz[8].locSet(Culoare.ALB, 3, Image.FromFile(pathColoanaAlbe[3]));
            poz[12].locSet(Culoare.NEGRU, 5, Image.FromFile(pathColoanaNegre[5]));
            poz[13].locSet(Culoare.ALB, 5, Image.FromFile(pathColoanaAlbe[5]));
            poz[17].locSet(Culoare.NEGRU, 3, Image.FromFile(pathColoanaNegre[18]));
            poz[19].locSet(Culoare.NEGRU, 5, Image.FromFile(pathColoanaNegre[5]));
            poz[24].locSet(Culoare.ALB, 2, Image.FromFile(pathColoanaAlbe[17]));
        }

        public void joc_single()
        {
            joc_in_doi();
            single_player = true;
            Roll.Text = "Roll/End Turn";
        }
        //ond of functii de initializare
        
        //functii ajutatoare
        public void setPriorityList()
        {
            prioritateMutare.Clear();
            if (afaraNegru != 0)//piesa data afara
            {
                this.Refresh();
                highlightMovesFrom(0);
                if (zar.zar1 != 0 && exceptiiSelected(zar.zar1))
                {
                    if (poz[zar.zar1].culoarePiese == Culoare.ALB)
                        prioritateMutare.Add(1, Tuple.Create(0, zar.zar1,1));
                    if (poz[zar.zar1].culoarePiese == Culoare.NEGRU)
                        prioritateMutare.Add(2, Tuple.Create(0, zar.zar1,1));
                    if (poz[zar.zar1].culoarePiese == null)
                        prioritateMutare.Add(3, Tuple.Create(0, zar.zar1,1));
                }
                if (zar.zar2 != 0 && exceptiiSelected(zar.zar2))
                {
                    if (poz[zar.zar2].culoarePiese == Culoare.ALB)
                        prioritateMutare.Add(1, Tuple.Create(0, zar.zar2,2));
                    if (poz[zar.zar2].culoarePiese == Culoare.NEGRU)
                        prioritateMutare.Add(2, Tuple.Create(0, zar.zar2,2));
                    if (poz[zar.zar2].culoarePiese == null)
                        prioritateMutare.Add(3, Tuple.Create(0, zar.zar2,2));
                }
                if (zar.zar2 != 0 && zar.zar1 != 0 && exceptiiSelected(zar.zar1+zar.zar2) && (exceptiiSelected(zar.zar2) || exceptiiSelected(zar.zar1)))
                {
                    if (poz[zar.zar1+zar.zar2].culoarePiese == Culoare.ALB)
                        prioritateMutare.Add(7, Tuple.Create(0, zar.zar1+zar.zar2,3));
                    if (poz[zar.zar1+zar.zar2].culoarePiese == Culoare.NEGRU)
                        prioritateMutare.Add(8, Tuple.Create(0, zar.zar1+zar.zar2,3));
                    if (poz[zar.zar1+zar.zar2].culoarePiese == null)
                        prioritateMutare.Add(9, Tuple.Create(0, zar.zar1+zar.zar2,3));
                }
                //repunerePiesa();
                this.Refresh();
                deselect();
            }
            else//mutare normala
            {
                for (int i=1;i<=24;i++)
                {
                    if(poz[i].culoarePiese==Culoare.NEGRU)
                    {
                        this.Refresh();
                        highlightMovesFrom(i);
                        this.Refresh();
                        if (zar.zar1!=0 && i + zar.zar1 < 25 && exceptiiSelected(i+zar.zar1))
                            {
                                if (poz[i+zar.zar1].culoarePiese == Culoare.ALB)
                                    prioritateMutare.Add(21, Tuple.Create(i, i+zar.zar1,1));
                                if (poz[i+zar.zar1].culoarePiese == Culoare.NEGRU)
                                    prioritateMutare.Add(22, Tuple.Create(i, i+zar.zar1,1));
                                if (poz[i+zar.zar1].culoarePiese == null)
                                    prioritateMutare.Add(23, Tuple.Create(i, i+zar.zar1,1));
                            }
                        if (zar.zar2 != 0 && i + zar.zar2 < 25 && exceptiiSelected(i+zar.zar2))
                            {
                                if (poz[i+zar.zar2].culoarePiese == Culoare.ALB)
                                    prioritateMutare.Add(21, Tuple.Create(i,i+ zar.zar2,2));
                                if (poz[i+zar.zar2].culoarePiese == Culoare.NEGRU)
                                    prioritateMutare.Add(22, Tuple.Create(i, i+zar.zar2,2));
                                if (poz[i+zar.zar2].culoarePiese == null)      
                                    prioritateMutare.Add(23, Tuple.Create(i, i+zar.zar2,2));
                            }
                        if (zar.zar2 != 0 && zar.zar1 != 0 && i + zar.zar1 + zar.zar2 < 25 &&exceptiiSelected(i+zar.zar1 + zar.zar2) && (exceptiiSelected(i+zar.zar2) || exceptiiSelected(i+zar.zar1)))
                            {
                                if (poz[i+zar.zar1 + zar.zar2].culoarePiese == Culoare.ALB)
                                    prioritateMutare.Add(21, Tuple.Create(i, i+zar.zar1 + zar.zar2,3));
                                if (poz[i+zar.zar1 + zar.zar2].culoarePiese == Culoare.NEGRU)
                                    prioritateMutare.Add(22, Tuple.Create(i, i+zar.zar1 + zar.zar2,3));
                                if (poz[i+zar.zar1 + zar.zar2].culoarePiese == null)
                                    prioritateMutare.Add(23, Tuple.Create(i, i+zar.zar1 + zar.zar2,3));
                            }
                        else
                        {
                            if(zar.zar1 != 0 && zar.zar2 != 0 && i + zar.zar1 + zar.zar2 == 25 && exceptiiSelected(i + zar.zar1 + zar.zar2) && (exceptiiSelected(i + zar.zar2) || exceptiiSelected(i + zar.zar1)))
                                prioritateMutare.Add(11, Tuple.Create(i, i + zar.zar1 + zar.zar2,3));
                        }

                        if(zar.zar1 != 0 && i + zar.zar1 == 25 && exceptiiSelected(i + zar.zar1))
                            prioritateMutare.Add(12, Tuple.Create(i, i + zar.zar1,1));
                        if (zar.zar2 != 0 && i + zar.zar2 == 25 && exceptiiSelected(i + zar.zar2))
                            prioritateMutare.Add(12, Tuple.Create(i, i + zar.zar2,2));
                        if(zar.zar1 != 0 && i + zar.zar1>25 && exceptiiSelected(25) && i + zar.zar1>= i + zar.zar2)
                            prioritateMutare.Add(11, Tuple.Create(i, 25,1));
                        if (zar.zar2 != 0 && i + zar.zar2 > 25 && exceptiiSelected(25) && i + zar.zar2 >= i + zar.zar1)
                            prioritateMutare.Add(11, Tuple.Create(i, 25,2));
                        deselect();
                        this.Refresh();
                    }
                }

            }
        }
        public void deselect()
        {
            Afara_Alb.BorderStyle = BorderStyle.None;
            Afara_Negru.BorderStyle = BorderStyle.None;
            for (int i = 0; i <= 25; i++)
                poz[i].imagine.BorderStyle = BorderStyle.None;
        }
        void repunerePiesa()
        {
            if (selected)
            {//revenire cursor
                if (this.Cursor == cursorNegru)
                {
                    if (lSelected == 0)
                    {
                        afaraNegru++;
                        Afara_Negru.Image = Image.FromFile(pathCasaNeagra[afaraNegru]);
                    }
                    else
                    {
                        poz[lSelected].numarPiese++;
                        poz[lSelected].locSet(Culoare.NEGRU, poz[lSelected].numarPiese, Image.FromFile(pathColoanaNegre[corectieNumar(lSelected, poz[lSelected].numarPiese)]));
                    }
                }
                else
                {
                    if (lSelected == 25)
                    {
                        afaraAlb++;
                        Afara_Alb.Image = Image.FromFile(pathCasaAlba[afaraAlb]);
                    }
                    else
                    {
                        poz[lSelected].numarPiese++;
                        poz[lSelected].locSet(Culoare.ALB, poz[lSelected].numarPiese, Image.FromFile(pathColoanaAlbe[corectieNumar(lSelected, poz[lSelected].numarPiese)]));
                    }
                }
            }
            selected = false;
            this.Cursor = Cursors.Default;
        }
        int indexPoz(PictureBox p)
        {
            for (int i = 0; i <= 25; i++)
                if (p.Name == poz[i].imagine.Name)
                    return i;
            if (p == Afara_Alb)
                return 25;
            if (p == Afara_Negru)
                return 0;
            return -1;
        }
        void fereastraTipJoc()
        {
            DialogResult option = MessageBox.Show("Doriti sa jucati in doi? \n Daca nu, jocul va opta ptr single player!", "Tip joc", MessageBoxButtons.YesNo);
            if (option == DialogResult.Yes)
                joc_in_doi();
            else
                joc_single();
        }
        
        //end of functii ajutatoare

        //validatori
        void Castigator()
        {
            if (poz[25].numarPiese == 15)
            {
                MessageBox.Show("NEGRUL A CASTIGAT!!!");
                DialogResult option = MessageBox.Show("Doriti sa jucati un joc nou? \n Daca nu, jocul se va inchide!", "Alegere", MessageBoxButtons.YesNo);
                game_start = false;
                if (option == DialogResult.Yes)
                    fereastraTipJoc();
                else
                    Close();

            }
            if (poz[0].numarPiese == 15)
            {
                MessageBox.Show("ALBUL A CASTIGAT!!!");
                DialogResult option = MessageBox.Show("Doriti sa jucati un joc nou? \n Daca nu, jocul se va inchide!", "Alegere", MessageBoxButtons.YesNo);
                game_start = false;
                if (option == DialogResult.Yes)
                    fereastraTipJoc();
                else
                    Close();
            }

        }
        bool exceptiiNotSelected(int l, PictureBox p)
        {
            //nu s-a selectat casa
            if ((p == poz_0) || (p == poz_25) && !selected)
            {
                label.Text = "Nu se poate selecta casa fara o piesa selectata!";
                return false;
            }
            //piesa data afara
            if ((!rand && afaraAlb > 0 && l < 25) || (l > 0 && rand && afaraNegru > 0))
            {
                label.Text = "Va rugam reintroduceti piesa data afara!";
                return false;
            }
            //nu sunt piese
            //if(((rand && afaraNegru==0) || (!rand && afaraAlb == 0)))
            //{
            //    MessageBox.Show("Nu sunt piese aici!");
            //    return false;
            //}
            if ((afaraNegru == 0 && p == Afara_Negru) || (afaraAlb == 0 && p == Afara_Alb))
            {
                label.Text = "Nu sunt piese aici!";
                return false;
            }
            if (poz[l].numarPiese == 0 && p != Afara_Alb && p != Afara_Negru)
            {
                label.Text = "Nu sunt piese aici!";
                return false;
            }
            //culoare potrivita
            if (((!rand && poz[l].culoarePiese == Culoare.NEGRU) || (rand && poz[l].culoarePiese == Culoare.ALB)) && p != Afara_Alb && p != Afara_Negru)
            {
                label.Text = "Culoarea piesei este incorecta!";
                return false;
            }
            return true;
        }
        bool exceptiiSelected(int l)
        {
            if (l == -1)
            {
                label.Text = "Nu se poate muta aici!";
                return false;
            }
            if (poz[l].imagine.BorderStyle == BorderStyle.None || poz[l].imagine.BorderStyle == BorderStyle.FixedSingle)
            {
                label.Text = "Nu se poate muta aici!";
                return false;
            }
            return true;
        }
        bool validareIntrareCasa(bool rand)
        {
            int nr_piese = 0;
            if (rand)
            {
                for (int i = 19; i <= 25; i++)
                {
                    if (poz[i].culoarePiese == Culoare.NEGRU)
                        nr_piese += poz[i].numarPiese;
                }
                if (nr_piese + 1 == 15)
                    return true;
            }
            else
            {
                for (int i = 0; i <= 6; i++)
                {
                    if (poz[i].culoarePiese == Culoare.ALB)
                        nr_piese += poz[i].numarPiese;
                }
                if (nr_piese + 1 == 15)
                    return true;
            }
            return false;
        }
        bool locNeocupat(int l)
        {
            if (rand)
            {
                if (poz[l].numarPiese > 1 && poz[l].culoarePiese == Culoare.ALB)
                    return false;
            }
            else
            {
                if (poz[l].numarPiese > 1 && poz[l].culoarePiese == Culoare.NEGRU)
                    return false;
            }
            return true;
        }
        int corectieNumar(int l, int nr)//functia corecteaza pozitia pieselor in functie de pozitie
        {
            if (l >= 13 && l <= 24 && nr < 5 && nr > 0)
                return nr + 15;
            else
                return nr;
        }
        bool verificareNicioMutarePos(int l)
        {
            if (selected)
            {
                for (int i = 0; i <= 25; i++)
                    if (poz[i].imagine.BorderStyle == BorderStyle.Fixed3D && i != l)
                        return false;
            }
            return true;
        }
        private bool validareIntrareCasaNesel()
        {
            int nr_piese = 0;
            for (int i = 19; i <= 25; i++)
            {
                if (poz[i].culoarePiese == Culoare.NEGRU)
                    nr_piese += poz[i].numarPiese;
            }
            if (nr_piese == 15)
                return true;
            return false;
        }
        //end of validatori
        
        //mutari
        public void highlightMovesFrom(int l)
        {
            if (l == 0)//dat afara
            {
                Afara_Negru.BorderStyle = BorderStyle.Fixed3D;
                if (locNeocupat(l + zar.zar1)&&zar.zar1!=0)
                    poz[l + zar.zar1].imagine.BorderStyle = BorderStyle.Fixed3D;
                if (locNeocupat(l + zar.zar2) && zar.zar2 != 0)
                    poz[l + zar.zar2].imagine.BorderStyle = BorderStyle.Fixed3D;
                if (locNeocupat(l + zar.zar1 + zar.zar2) && (locNeocupat(l + zar.zar2) || locNeocupat(l + zar.zar1)))
                    poz[l + zar.zar1 + zar.zar2].imagine.BorderStyle = BorderStyle.Fixed3D;
            }
            else
            {
                poz[l].imagine.BorderStyle = BorderStyle.Fixed3D;
                
                if (l + zar.zar1 <= 24 && locNeocupat(l + zar.zar1))
                    poz[l + zar.zar1].imagine.BorderStyle = BorderStyle.Fixed3D;
                if (l + zar.zar2 <= 24 && locNeocupat(l + zar.zar2))
                    poz[l + zar.zar2].imagine.BorderStyle = BorderStyle.Fixed3D;
                if (l + zar.zar1 + zar.zar2 < 25)
                    if (locNeocupat(l + zar.zar1 + zar.zar2) && (locNeocupat(l + zar.zar2) || locNeocupat(l + zar.zar1)))
                        poz[l + zar.zar1 + zar.zar2].imagine.BorderStyle = BorderStyle.Fixed3D;
                    else
                    if (l + zar.zar1 + zar.zar2 == 25 && validareIntrareCasaNesel() && (locNeocupat(l + zar.zar2) || locNeocupat(l + zar.zar1)))
                        poz[l + zar.zar1 + zar.zar2].imagine.BorderStyle = BorderStyle.Fixed3D;

                if ((l + zar.zar1 > 24 || l + zar.zar2 > 24) && validareIntrareCasaNesel())//intrare in casa
                {
                    if (l + zar.zar1 == 25 || l + zar.zar2 == 25)//intra fix in casa
                    {
                        poz[25].imagine.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else//depaseste si necesita o verificare suplimentara
                    {
                        bool ok = false;
                        for (int index = 1; index < l && !ok; index++)
                            if (poz[index].culoarePiese==Culoare.NEGRU)
                                ok = true;
                        if (!ok)
                            poz[25].imagine.BorderStyle = BorderStyle.Fixed3D;
                    }
                }

            }
        }
        void mutNegruDin(int l)
        {
            if(l==0)//dat afara
            {
                afaraNegru--;
                this.Cursor = cursorNegru;
                Afara_Negru.Image = Image.FromFile(pathCasaNeagra[afaraNegru]);
                Afara_Negru.BorderStyle = BorderStyle.Fixed3D;
                if (locNeocupat(l + zar.zar1)&& zar.zar1!=0)
                    poz[l + zar.zar1].imagine.BorderStyle = BorderStyle.Fixed3D;
                if (locNeocupat(l + zar.zar2) && zar.zar2 != 0)
                    poz[l + zar.zar2].imagine.BorderStyle = BorderStyle.Fixed3D;
                if (locNeocupat(l + zar.zar1 + zar.zar2) && (locNeocupat(l + zar.zar2) || locNeocupat(l + zar.zar1)) && afaraNegru==0)
                    poz[l + zar.zar1 + zar.zar2].imagine.BorderStyle = BorderStyle.Fixed3D;
            }
            else
            {
                poz[l].numarPiese--;
                //if(!single_player)
                    this.Cursor = cursorNegru;
                poz[l].imagine.Image = Image.FromFile(pathColoanaNegre[corectieNumar(l, poz[l].numarPiese)]);
                poz[l].imagine.BorderStyle = BorderStyle.Fixed3D;
                if (poz[l].numarPiese == 0)
                    poz[l].culoarePiese = null;

                if (l + zar.zar1 <= 24 && locNeocupat(l + zar.zar1))
                    poz[l + zar.zar1].imagine.BorderStyle = BorderStyle.Fixed3D;
                if (l + zar.zar2 <= 24 && locNeocupat(l + zar.zar2))
                    poz[l + zar.zar2].imagine.BorderStyle = BorderStyle.Fixed3D;
                if (l + zar.zar1 + zar.zar2 < 25)
                {
                    if (locNeocupat(l + zar.zar1 + zar.zar2) && (locNeocupat(l + zar.zar2) || locNeocupat(l+ zar.zar1)))
                        poz[l + zar.zar1 + zar.zar2].imagine.BorderStyle = BorderStyle.Fixed3D;
                }
                else
                    if (l + zar.zar1 + zar.zar2 == 25 && validareIntrareCasa(true) && (locNeocupat(l + zar.zar2) || locNeocupat(l + zar.zar1)))
                        poz[l + zar.zar1 + zar.zar2].imagine.BorderStyle = BorderStyle.Fixed3D;

                if ((l + zar.zar1 > 24 || l + zar.zar2 > 24) && validareIntrareCasa(true))//intrare in casa
                {
                    if (l + zar.zar1 == 25 || l + zar.zar2 == 25)//intra fix in casa
                    {
                        poz[25].imagine.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else//depaseste si necesita o verificare suplimentara
                    { 
                        bool ok = false;
                        for (int index = 19; index < l && !ok; index++)
                            if (poz[index].numarPiese != 0)
                                ok = true;
                        if (!ok)
                            poz[25].imagine.BorderStyle = BorderStyle.Fixed3D;
                    }
                }

            }
        }
        void mutAlbDin(int l)
        {
            if (l == 25)//dat afara
            {
                afaraAlb--;
                this.Cursor = cursorAlb;
                Afara_Alb.Image = Image.FromFile(pathCasaAlba[afaraAlb]);
                Afara_Alb.BorderStyle = BorderStyle.Fixed3D;
                if (locNeocupat(l - zar.zar1)&& zar.zar1!=0)
                    poz[l - zar.zar1].imagine.BorderStyle = BorderStyle.Fixed3D;
                if (locNeocupat(l - zar.zar2)&&zar.zar2 != 0)
                    poz[l - zar.zar2].imagine.BorderStyle = BorderStyle.Fixed3D;
                if (locNeocupat(l - zar.zar1 - zar.zar2) && (locNeocupat(l - zar.zar2)|| locNeocupat(l - zar.zar1)) && afaraAlb==0)
                    poz[l - zar.zar1 - zar.zar2].imagine.BorderStyle = BorderStyle.Fixed3D;
            }
            else
            {
                poz[l].numarPiese--;
                this.Cursor = cursorAlb;
                poz[l].imagine.Image = Image.FromFile(pathColoanaAlbe[corectieNumar(l, poz[l].numarPiese)]);
                poz[l].imagine.BorderStyle = BorderStyle.Fixed3D;
                if (poz[l].numarPiese == 0)
                    poz[l].culoarePiese = null;

                if (l - zar.zar1 >= 1 && locNeocupat(l - zar.zar1))
                    poz[l - zar.zar1].imagine.BorderStyle = BorderStyle.Fixed3D;
                if (l - zar.zar2 >= 1 && locNeocupat(l - zar.zar2))
                    poz[l - zar.zar2].imagine.BorderStyle = BorderStyle.Fixed3D;
                if (l - zar.zar1 - zar.zar2 > 0)
                {
                    if (locNeocupat(l - zar.zar1 - zar.zar2) && (locNeocupat(l - zar.zar2) || locNeocupat(l - zar.zar1)))
                        poz[l - zar.zar1 - zar.zar2].imagine.BorderStyle = BorderStyle.Fixed3D;
                }
                else
                    if (l - zar.zar1 - zar.zar2 == 0 && validareIntrareCasa(false) && (locNeocupat(l - zar.zar2) || locNeocupat(l - zar.zar1)))
                        poz[l - zar.zar1 - zar.zar2].imagine.BorderStyle = BorderStyle.Fixed3D;

                if ((l - zar.zar1 < 1 || l - zar.zar2 < 1) && validareIntrareCasa(false))//intrare in casa
                {
                    if (l - zar.zar1 == 0 || l - zar.zar2 == 0)//intra fix in casa
                    {
                        poz[0].imagine.BorderStyle = BorderStyle.Fixed3D;
                    }
                    else//depaseste si necesita o verificare suplimentara
                    {
                        bool ok = false;
                        for (int index = 6; index > l && !ok; index--)
                            if (poz[index].numarPiese != 0)
                                ok = true;
                        if (!ok)
                            poz[0].imagine.BorderStyle = BorderStyle.Fixed3D;
                    }

                }
            }
        }
        void mutAlbLa(int l)
        {
            //zar dublu
            if(zar.zar_dublu)
            {
                if(zar1.BorderStyle == BorderStyle.Fixed3D || zar2.BorderStyle == BorderStyle.Fixed3D)//cel putin 1 un zar este 2
                {
                    if ((l == lSelected - zar.zar2 || lSelected - zar.zar2 < 0) && zar2.BorderStyle == BorderStyle.Fixed3D && zar1.BorderStyle == BorderStyle.None)//al doilea zar neconsumat
                    {
                        zar2.BorderStyle = BorderStyle.None;
                    }
                    if ((l == lSelected - zar.zar1 || lSelected - zar.zar1 < 0) && zar1.BorderStyle == BorderStyle.Fixed3D)//primul zar neconsumat
                    {
                        zar1.BorderStyle=BorderStyle.None;
                    }
                    if (l == lSelected - zar.zar1 - zar.zar2)//se folosesc ambele zaruri
                    {
                        if (zar1.BorderStyle == BorderStyle.Fixed3D && zar2.BorderStyle == BorderStyle.Fixed3D)//2 2
                        {
                            zar1.BorderStyle = BorderStyle.None;
                            zar2.BorderStyle = BorderStyle.None;
                        }
                        if(zar1.BorderStyle == BorderStyle.Fixed3D)//2 1
                        {
                            zar1.BorderStyle = BorderStyle.None;
                            zar2.Image = null;
                            zar.resetZar2();
                        }
                        if (zar2.BorderStyle == BorderStyle.Fixed3D)//1 2
                        {
                            zar2.BorderStyle = BorderStyle.None;
                            zar1.Image = null;
                            zar.resetZar1();
                        }
                    }
                }
                else
                {
                    if (l == lSelected - zar.zar1 - zar.zar2)//1 1
                    {
                        zar1.Image = null;
                        zar.resetZar1();
                        zar2.Image = null;
                        zar.resetZar2();
                    }
                    if ((l == lSelected - zar.zar2 || lSelected - zar.zar2 < 0)&& zar1.Image == null)//al doilea zar neconsumat
                    {
                        zar2.Image = null;
                        zar.resetZar2();
                    }
                    if ((l == lSelected - zar.zar1 || lSelected - zar.zar1 < 0))//primul zar neconsumat
                    {
                        zar1.Image = null;
                        zar.resetZar1();
                    }
                }
            }
            //zar nedublu
            else
            {
                if (l == lSelected - zar.zar1 || lSelected-zar.zar1<0)
                {
                    zar.resetZar1();
                    zar1.BorderStyle = BorderStyle.None;
                    zar1.Image = null;
                }
                else if (l == lSelected - zar.zar2 || lSelected - zar.zar2 < 0)
                {
                    zar.resetZar2();
                    zar2.BorderStyle = BorderStyle.None;
                    zar2.Image = null;
                }
                else if(l== lSelected-zar.zar1-zar.zar2)
                {
                    zar.resetZar1();
                    zar.resetZar2();
                    zar1.BorderStyle = BorderStyle.None;
                    zar2.BorderStyle = BorderStyle.None;
                    zar1.Image = null;
                    zar2.Image = null;
                }

            }
            //fac mutare
            if (poz[l].culoarePiese == Culoare.NEGRU)//dat afara
            {
                afaraNegru++;
                poz[l].culoarePiese = Culoare.ALB;
                Afara_Negru.Image = Image.FromFile(pathCasaNeagra[afaraNegru]);
                poz[l].imagine.Image = Image.FromFile(pathColoanaAlbe[corectieNumar(l,1)]);
            }
            else//mutare normala
            {
                if(l==0)
                {
                    poz[l].numarPiese++;
                    poz[l].imagine.Image = Image.FromFile(pathCasaAlba[poz[l].numarPiese]);
                    return;
                }
                poz[l].culoarePiese = Culoare.ALB;
                poz[l].numarPiese++;
                poz[l].imagine.Image = Image.FromFile(pathColoanaAlbe[corectieNumar(l, poz[l].numarPiese)]);
            }
        } 
        void mutNegruLa(int l)
        {
            //zar dublu
            if (zar.zar_dublu)
            {
                if (zar1.BorderStyle == BorderStyle.Fixed3D || zar2.BorderStyle == BorderStyle.Fixed3D)//cel putin 1 un zar este 2
                {
                    if ((l == lSelected + zar.zar2 || lSelected + zar.zar2 > 25) && zar2.BorderStyle == BorderStyle.Fixed3D && zar1.BorderStyle == BorderStyle.None)//al doilea zar neconsumat
                    {
                        zar2.BorderStyle = BorderStyle.None;
                    }
                    if ((l == lSelected + zar.zar1 || lSelected + zar.zar1 > 25) && zar1.BorderStyle == BorderStyle.Fixed3D)//primul zar neconsumat
                    {
                        zar1.BorderStyle = BorderStyle.None;
                    }
                    if (l == lSelected + zar.zar1 + zar.zar2)//se folosesc ambele zaruri
                    {
                        if (zar1.BorderStyle == BorderStyle.Fixed3D && zar2.BorderStyle == BorderStyle.Fixed3D)//2 2
                        {
                            zar1.BorderStyle = BorderStyle.None;
                            zar2.BorderStyle = BorderStyle.None;
                        }
                        if (zar1.BorderStyle == BorderStyle.Fixed3D)//2 1
                        {
                            zar1.BorderStyle = BorderStyle.None;
                            zar2.Image = null;
                            zar.resetZar2();
                        }
                        if (zar2.BorderStyle == BorderStyle.Fixed3D)//1 2
                        {
                            zar2.BorderStyle = BorderStyle.None;
                            zar1.Image = null;
                            zar.resetZar1();
                        }
                    }
                }
                else
                {
                    if (l == lSelected + zar.zar1 + zar.zar2)//1 1
                    {
                        zar1.Image = null;
                        zar.resetZar1();
                        zar2.Image = null;
                        zar.resetZar2();
                    }
                    if ((l == lSelected + zar.zar2 || lSelected + zar.zar2 > 25) && zar1.Image == null)//al doilea zar neconsumat
                    {
                        zar2.Image = null;
                        zar.resetZar2();
                    }
                    if ((l == lSelected + zar.zar1 || lSelected + zar.zar1 > 25))//primul zar neconsumat
                    {
                        zar1.Image = null;
                        zar.resetZar1();
                    }
                }
            }
            //zar nedublu
            else
            {
                if (l == lSelected + zar.zar1 || lSelected + zar.zar1 > 25)
                {
                    zar.resetZar1();
                    zar1.BorderStyle = BorderStyle.None;
                    zar1.Image = null;
                }
                else if (l == lSelected + zar.zar2 || lSelected + zar.zar2 > 25)
                {
                    zar.resetZar2();
                    zar2.BorderStyle = BorderStyle.None;
                    zar2.Image = null;
                }
                else if (l == lSelected + zar.zar1 + zar.zar2)
                {
                    zar.resetZar1();
                    zar.resetZar2();
                    zar1.BorderStyle = BorderStyle.None;
                    zar2.BorderStyle = BorderStyle.None;
                    zar1.Image = null;
                    zar2.Image = null;
                }

            }
            //fac mutare
            if (poz[l].culoarePiese == Culoare.ALB)//dat afara
            {
                afaraAlb++;
                poz[l].culoarePiese = Culoare.NEGRU;
                Afara_Alb.Image = Image.FromFile(pathCasaAlba[afaraAlb]);
                poz[l].imagine.Image = Image.FromFile(pathColoanaNegre[corectieNumar(l, 1)]);
            }
            else//mutare normala
            {
                if (l == 25)
                {
                    poz[l].numarPiese++;
                    poz[l].imagine.Image = Image.FromFile(pathCasaNeagra[poz[l].numarPiese]);
                    return;
                }
                poz[l].culoarePiese = Culoare.NEGRU;
                poz[l].numarPiese++;
                poz[l].imagine.Image = Image.FromFile(pathColoanaNegre[corectieNumar(l, poz[l].numarPiese)]);
            }
        }
        //end of mutari

        //actions
        private void tri_MouseHover(object sender, EventArgs e)
        {
            PictureBox p = (PictureBox)(sender);
            if (p.BorderStyle != BorderStyle.Fixed3D)
                p.BorderStyle = BorderStyle.FixedSingle;
        }
        private void tri_MouseLeave(object sender, EventArgs e)
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
                label.Text="Jocul nu a inceput inca!";
            }
            else
            {
                zar.resetZar1();
                zar.resetZar2();
                repunerePiesa();
                for (i = 0; i <= 25; i++)
                    poz[i].imagine.BorderStyle = BorderStyle.None;
            }
            
        }
        private void Tri_Click(object sender, MouseEventArgs e)
        {
            PictureBox p = (PictureBox)(sender);
            int l = indexPoz(p);
            label.Text = "Afisez nereguli";
           
            if (zar.zar1 <= 0 && zar.zar2<=0) //nu s-a dat cu zarul
            {
                label.Text = "Nu s-a dat cu zarul!";
                return;
            }
            //la selectia piesei se vor calcula si mutarile posibile si aceasta functie va avea rol si de arbitru
            if(!selected)
            {
                if(exceptiiNotSelected(l,p))
                {
                    // daca piesa selectata indeplineste conditiile trec la marcarea pozitiilor posibile ptr realizarea unei mutari
                    selected = true;
                    if (rand)
                        mutNegruDin(l);
                    else
                        mutAlbDin(l);

                    lSelected = l;
                    //verific daca sunt mutari posibile
                    if (verificareNicioMutarePos(l))
                    {
                        MessageBox.Show("Nu sunt mutari posibile din aceasta pozitie!");
                        repunerePiesa();
                        deselect();
                        return;
                    }
                }

            }
            else
            {
                if(exceptiiSelected(l))
                {
                    this.Cursor = Cursors.Default;
                    selected = false;
                    if (rand)//negru
                    {
                        mutNegruLa(l);
                    }
                    else//alb
                    {
                        mutAlbLa(l);
                    }
                    deselect();
                }
            }
            Castigator();
        }

        public void mutarePlayer()
        {
            while(zar.zar1!=0 || zar.zar2!=0)
            {
                setPriorityList();
                if(prioritateMutare.Count>0)
                {
                    Tuple<int, int, int> mutare = prioritateMutare.Values[0];
                    int din=mutare.Item1, la=mutare.Item2,zarul=mutare.Item3,zar_folosit=0;
                    prioritateMutare.RemoveAt(0);
                    if (zar.zar1 == 0)
                        zar_folosit = 1;
                    if (zar.zar2 == 0)
                        zar_folosit = 2;
                    while(zar_folosit==zarul && (zarul==3 && zar_folosit!=0))
                    {
                        mutare = prioritateMutare.Values[0];
                        din = mutare.Item1;
                        la = mutare.Item2;
                        zarul = mutare.Item3;
                        prioritateMutare.RemoveAt(0);
                    }
                    this.Refresh();
                    System.Threading.Thread.Sleep(800);
                    selected = true;
                    mutNegruDin(din);
                    this.Refresh();
                    System.Threading.Thread.Sleep(200);
                    lSelected = din;
                    this.Cursor = Cursors.Default;
                    selected = false;
                    mutNegruLa(la);
                    this.Refresh();
                    //repunerePiesa();
                    deselect();
                }
                else
                {
                    MessageBox.Show("Nu sunt mutari posibile!");
                    zar.resetZar1();
                    zar.resetZar2();
                    repunerePiesa();
                    deselect();
                    return;
                }
            }
            Castigator();
        }

        private void Roll_Click(object sender, EventArgs e)
        {
            if(!game_start)
            {
                label.Text = "Jocul nu a inceput inca!";
            }
            else
            {
                if (zar.zar1 == 0 && zar.zar2 == 0)
                {
                    zar.aruncaZarurile();
                    //se verifica daca este zar dublu
                    if (zar.zar_dublu)
                    {
                        zar1.BorderStyle = BorderStyle.Fixed3D;
                        zar2.BorderStyle = BorderStyle.Fixed3D;
                    }
                    //se atribuie randul
                    if (rand)
                    {
                        turn.Text = "Este randul albelor";
                        rand = false;
                    }
                    else
                    {
                        turn.Text = "Este randul negrelor";
                        rand = true;
                    }
                    if (single_player && rand)
                        mutarePlayer();
                }
                else
                    label.Text = "Nu s-au efectuat toate mutarile!";
            }
        }

        private void salveazaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!game_start)
            {
                label.Text = "Jocul nu a inceput inca!";
            }
            else
            {
                // Prepare a dummy string, thos would appear in the dialog
                string dummyFileName = "Save Here";

                SaveFileDialog sf = new SaveFileDialog();
                // Feed the dummy name to the save dialog
                sf.FileName = dummyFileName;
                sf.Filter = "Data Files (*.savbac)|*.savbac";
                sf.DefaultExt = "savbac";
                sf.AddExtension = true;
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    // Now here's our save folder
                    string savePath = Path.GetDirectoryName(sf.FileName);

                    // Do whatever
                    using (StreamWriter sw = File.CreateText(sf.FileName))
                    {
                        sw.WriteLine(rand);
                        sw.WriteLine(single_player);
                        sw.WriteLine(zar.zar1 + " " + zar.zar2);
                        sw.WriteLine(afaraAlb + " " + afaraNegru);
                        for (int i = 0; i <= 25; i++)
                        {
                            sw.WriteLine(string.Format("{0};{1};{2};", i, poz[i].culoarePiese.ToString(), poz[i].numarPiese));

                        }
                    }
                    label.Text = "Joc salvat cu succes!";
                }


            }

        }
        private void incarcaToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "savbac files (*.savbac)|*.savbac";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                    string[] rows=fileContent.Split('\n');
                    single_player = bool.Parse(rows[1]);
                    game_start = true;
                    afaraAlb = 0;
                    afaraNegru = 0;

                    rand = bool.Parse(rows[0]);
                    if (rand)
                        turn.Text = "Este randul albelor";
                    else
                        turn.Text = "Este randul negrelor";

                    zar.resetZar1();
                    zar.resetZar2();
                    zar.zar1 = rows[2][0]-48;
                    zar.zar2 = rows[2][2]-48;
                    if(zar.zar1==0)
                        zar.zar1Image.Image = null;
                    else
                        zar.zar1Image.Image = Image.FromFile(pathZaruri[zar.zar1]);

                    if (zar.zar2 == 0)
                        zar.zar1Image.Image = null;
                    else
                        zar.zar2Image.Image = Image.FromFile(pathZaruri[zar.zar2]);

                    //se verifica daca este zar dublu
                    if (zar.zar1 == zar.zar2 && zar.zar1!=0)
                        zar.zar_dublu = true;
                    else
                        zar.zar_dublu = false;
                    if (zar.zar_dublu)
                    {
                        zar1.BorderStyle = BorderStyle.Fixed3D;
                        zar2.BorderStyle = BorderStyle.Fixed3D;
                    }

                    afaraAlb = rows[3][0]-48;
                    afaraNegru = rows[3][2]-48;
                    Afara_Alb.Image = Image.FromFile(pathCasaAlba[afaraAlb]);
                    Afara_Negru.Image = Image.FromFile(pathCasaNeagra[afaraNegru]);

                    string[] pozitii = rows[4].Split(';');

                    poz[0].locSet(Culoare.ALB, 0, Image.FromFile(pathCasaAlba[int.Parse(pozitii[2])]));

                    for (int i = 5; i < 29; i++)
                    {
                        pozitii = rows[i].Split(';');
                        if(pozitii[1]=="ALB")
                            poz[int.Parse(pozitii[0])].locSet(Culoare.ALB, int.Parse(pozitii[2]), Image.FromFile(pathColoanaAlbe[corectieNumar(int.Parse(pozitii[0]), int.Parse(pozitii[2]))]));
                        if (pozitii[1] == "NEGRU")
                            poz[int.Parse(pozitii[0])].locSet(Culoare.NEGRU, int.Parse(pozitii[2]), Image.FromFile(pathColoanaNegre[corectieNumar(int.Parse(pozitii[0]), int.Parse(pozitii[2]))]));
                    }
                    pozitii = rows[29].Split(';');
                    poz[25].locSet(Culoare.NEGRU, int.Parse(pozitii[2]), Image.FromFile(pathCasaNeagra[int.Parse(pozitii[2])]));

                    
                }
            }

            
        }
        //end of actions

        //Strip menus inits
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
        
        public void DualPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            joc_in_doi();
        }

        private void soloPlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            joc_single();
        }
        
        //End of Ajutor strip menu inits
        //End of Strip menus inits
    }
}
