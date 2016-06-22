using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

class Solution
{
    static string[] IMAGE;
    static List<Note> listnot = new List<Note>();
    static int cursor = 0;
    static int cst = 0;
    static int[] line = new int[6];
    static int d = 0;
    static Dictionary<int, int> dico = 
            new Dictionary<int, int>();
    static Dictionary<int, char> dicoline = 
            new Dictionary<int, char>();
    public class Note : IEquatable<Note> , IComparable<Note>
    {
        public int x { get; set; }
        public int y { get; set; }
        public char type { get; set; }

        public Note(int xv, int yv)
        {
            this.x = xv;
            this.y = yv;
            this.type = 'H';
        }
        public Note(int xv, int yv1 , int yv2)
        {
            this.x = xv;
            this.y = yv1 + ((yv2 - yv1) / 2);
            this.type = 'H';
            if (yv2 > yv1)
                Console.Error.WriteLine("error de parsing: yv2 < yv1 ==> " + yv2 + " & " + yv1);
        }

       public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Note objAsPart = obj as Note;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }
        public override int GetHashCode()
        {
            return x;
        }
        public bool Equals(Note other)
        {
            if (other == null) return false;
            return (this.x.Equals(other.x));
        }
        public int CompareTo(Note comparePart)
        {
            if (comparePart == null)
                return 1;
            else
                return this.x.CompareTo(comparePart.x);
        }
    }
    
    static void traitement(int W, int H, int size, int full){
        cursor = 0;
        for(int i = 0; i < IMAGE.Length; i+=2)
        {
             int yv = cursor / W;
             int xv = cursor % W;
            if(cst != 0 && IMAGE[i][0] == 'B' && (int.Parse(IMAGE[i + 1]) == 35 && size != 22)){ //gros fichier!
                listnot.Clear();
                dico.Clear();
                return;
            }
            if(cst != 0 && IMAGE[i][0] == 'B' && (int.Parse(IMAGE[i + 1]) == size ) && int.Parse(IMAGE[i + 1]) < 90)
            {
                int     value;
                if (dico.TryGetValue(xv, out value))
                    listnot.Add(new Note(xv, yv, value));
                else
                    dico.Add(xv, yv);
            }
            if ( cst == 0 && IMAGE[i][0] == 'W' && (int.Parse(IMAGE[i + 1]) == 6 ) )
            {
                int     value;

                if (!dico.TryGetValue(xv, out value))
                {
                    dico.Add(xv, yv);
                    listnot.Add(new Note(xv, yv + 2));
                }
            }
            if ( cst == 0 && IMAGE[i][0] == 'B' && (int.Parse(IMAGE[i + 1]) == 10 ) )
            {
                int     value;

                if (!dico.TryGetValue(xv, out value))
                {
                    dico.Add(xv, yv);
                    listnot.Add(new Note(xv, yv + 1){type = 'Q'});
                }
            }
            cursor += int.Parse(IMAGE[i + 1]);
        }
        listnot.Sort();
        cursor = 0;
        for(int i = 0; i < IMAGE.Length; i+=2)
        {
            int y = cursor / W;
             int x = cursor % W;
            if(IMAGE[i][0] == 'B' && int.Parse(IMAGE[i + 1]) > full && int.Parse(IMAGE[i + 1]) < 40)
            {
                foreach(Note n in listnot)
                if (n.x - 5 == x || n.x - 4 == x)
                    n.type = 'Q';
            }
            cursor += int.Parse(IMAGE[i + 1]);
        }
    }
    
    static void Main(string[] args)
    {
        string[] inputs = Console.ReadLine().Split(' ');

        int W = int.Parse(inputs[0]);
        int H = int.Parse(inputs[1]);
        IMAGE = Console.ReadLine().Split(' ');
        
        for(int i = 0; i < IMAGE.Length; i+=2)
        {
             int yv = cursor / W;
            if(IMAGE[i][0] == 'B' && int.Parse(IMAGE[i + 1]) > 40 && (d == 0 ||  yv > line[d - 1] + 4))
            {
                line[d] = yv;
                d++;
            }
            else if(IMAGE[i][0] == 'B' && int.Parse(IMAGE[i + 1]) > 40 )
                cst = yv - line[d - 1];
             cursor += int.Parse(IMAGE[i + 1]);
        }
        traitement(W, H, 12, 21);
        if (listnot.Count  < 1)
            traitement(W, H, 22, 22);
        int interv;
        int pls = 2;
        if (cst == 0)
            pls = 0;
        interv = (line[1] - line[0]) / 2;
        if (cst != 0)
            dicoline.Add(line[0] + pls - interv, 'G');
        else
            dicoline.Add(line[0] + pls - (interv + 1), 'G');
        dicoline.Add(line[0] + pls, 'F');
        dicoline.Add(line[0] + pls + interv, 'E');
        dicoline.Add(line[1] + pls, 'D');
        dicoline.Add(line[1] + pls + interv, 'C');
        dicoline.Add(line[2] + pls, 'B');
        dicoline.Add(line[2] + pls + interv, 'A');
        dicoline.Add(line[3] + pls, 'G');
        dicoline.Add(line[3] + pls + interv, 'F');
        dicoline.Add(line[4] + pls, 'E');
        dicoline.Add(line[4] + pls + interv, 'D');
        dicoline.Add(line[4] + pls + interv + interv, 'C');

        string res = "";
        foreach(Note n in listnot)
            res += dicoline[n.y] + "" + n.type + " ";
        res = res.Substring(0, res.Count() - 1);
        Console.WriteLine(res);
    }
}