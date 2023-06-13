using System;
using System.Collections.Generic;
using System.IO;
using ut;
using Args;

#pragma warning disable 642 



namespace tstCurveGraph
{                         
    static partial class gbl
    {

         static List<double[]> readFile (string flNm){
          string line="";
          try {
            List<double[]> _x = new List<double[]>();
            using( StreamReader sr = new StreamReader(flNm))
            {   
                char[] dels = new char[] {' ','\t'};
                int lineno = 0;
                while (!sr.EndOfStream)
                {
                    lineno++;
                    line  = sr.ReadLine();
                    int Pos = line.IndexOf('#');
                    //Если был найден, удалить подстроку, начиная с этой позиции
                    if (Pos >= 0)
                        line = line.Remove(Pos);
                    line = line.Trim();
                    string[] ss = line.Split( dels,  StringSplitOptions.RemoveEmptyEntries);
                    if (ss.Length >= 2)  {        /// becouse x, y, z only 3 coors
                       double[] r = new double[2];  //  это три координаты одна точка
                       int j = 0; 
                       for (j=0; j < 2; j++)
                       {
                          r[j] = Convert.ToDouble(ss[j]);
                       }
                       _x.Add(r);                             /// список для   накапливания строчек
                    }
                    else 
                      if (!String.IsNullOrEmpty(line))
                       Console.Error.WriteLine (
                            ">> readFile: wrong line #{0}: '{1}' {2}"
                            , lineno, line, ss.Length) ;
                }
            }
            return _x;
         }
         catch (Exception e){
            Console.Error.WriteLine ("*** readFile: error while reading data file. line :'{}'", line);
            Console.Error.WriteLine ("Exception '{0}'\n'{1}'\n'{2}'\n'{3}' ***", e.Message
                               , e.StackTrace
                                 , e.TargetSite
                                    , e.Source
                                         );

            return null;
         }
        } 
   }
}                	