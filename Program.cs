using System.Threading;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Globalization;
using ut;
using Args;
using wnd;

#pragma warning disable 642 



namespace tstCurveGraph {                         
    static partial class gbl {
        static public ArgFlg  hlpF ;
        static public ArgFlg  dbgF ;
//        static public ArgFlg  expF ;
        static public ArgFlg  vF ;    
        static public ArgFlg  cF ;    
        static public ArgFlg  emptyF ;    
        //  подписывать номера точек
        static public ArgFlg  lnF ;  // рисовать линии или точки
      //  static public ArgFloatMM  perCent ;
        static public ArgStr  flNm ;  // имя файла с точками
         static  gbl (){
           hlpF   =  new ArgFlg(false, "?","help",    "to see this help");
           vF     =  new ArgFlg(false, "v",  "verbose", "additional info");
           cF     =  new ArgFlg(false, "c",  "chart", " to show histogram");
           lnF    =  new ArgFlg(false, "ln",  "line",    "line flag");
           emptyF =  new ArgFlg(false, "e",  "empty",   "to hide  empty class");
           dbgF   =  new ArgFlg(false, "d",  "debug",   "debug mode");
//           expF   =  new ArgFlg(false, "e",  "expFunc",   "new func");
            //logLvl.show = false;
           flNm   =  new ArgStr("null", "f", "file", "data file", "FLNM");
         //  flNm.required = true;
/*
           perCent   =  new ArgFloatMM(0.05, "p", "percent", "percent for something",  "PPP");
           perCent.setMax(100.0);
*/      }
        static public  void usage(){
           Args.Arg.mkVHelp("to show functions of one argument", "", vF
                ,hlpF
                ,dbgF
                ,cF
//                ,expF
                ,emptyF
                ,vF
                ,flNm
                );
                Environment.Exit(1);
        }

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(	string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU", false);
            Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";
//            Application.EnableVisualStyles();
//            Application.SetCompatibleTextRenderingDefault(false);

           for (int i = 0; i<args.Length; i++){
             if (hlpF.check(ref i, args))
               usage();
             else if (dbgF.check(ref i, args))
               ;
             else if (vF.check(ref i, args))
               ;
             else if (cF.check(ref i, args))
               ;
             else if (emptyF.check(ref i, args))
               ;
             else if (lnF.check(ref i, args))
               ;
             else if (flNm.check(ref i, args))
               ;
           }

           DateTime st = 	DateTime.Now;

              if (vF)
                Console.Error.WriteLine("I'l started  with file/dbg/chart {0}/{1}/{2} "
                         ,  (string)flNm, (bool)dbgF, (bool)cF);

              Process proc = Process.GetCurrentProcess();

              long gTMst, pWSst, pVMst; 

              if ((bool)dbgF) graphPnl.debug = true;

              List<double[]> data= null;
                                                                      
              if ((bool)dbgF)  
                 graphWnd.debug = true;



              Form   x =  null; 




              if (flNm != "null") {

  
                data = readFile (flNm);
                 
                if ((bool)cF ) {
                    Console.Error.WriteLine("does not ready yet!");
                    graphChart.debug = dbgF;
                    graphWndC.debug  = dbgF; 
                    graphChart   p = new graphChart(emptyF);
                    x =  new graphWndC("histogramm test", 10, p);
                    p.data = data;
                    
                    x.ShowDialog();


                    Environment.Exit(1);
                 }
                 else   {
                    graphPnl   p = new graphPnl();
                    x =  new graphWnd("curve test", 10, p);
                    ((graphWnd)x).data = data;

                 }
                 if ((bool)dbgF)   { 
                   Console.Error.WriteLine("data  count {0}", ((graphWnd)x).data.Count );
                 }
              }
              else {
                 ArgFloatMM  minX   =  new ArgFloatMM(0.00, "min", "minX", "minimum X",  "MIN");
                 minX.setMax(90.0);
                 ArgFloatMM  maxX   =  new ArgFloatMM(760.00, "max", "maxX", "maximum X",  "MAX");
                 maxX.setMin(760.0);
                 ArgFloatMM  stepX   =  new ArgFloatMM(10.00, "step", "step X", "step X",  "STEP");
                 stepX.setMin(10.0);
                 stepX.setMax(100.0);

                 OkCancelDlg tblPar = new OkCancelDlg( "window to input some params", null //Logger
                                      , minX
                                      , maxX
                                      , stepX );

                 DialogResult rc = tblPar.ShowDialog();



                 if (rc == DialogResult.OK)   {
                     if ((bool)dbgF)   { 
                       Console.Error.WriteLine("min/max/step: {0}/{1}/{2}"
                           , (double)minX, (double)maxX, (double)stepX);
                     }
                     graphPnl   p = new graphPnl();
                     x =  new graphWnd("test1", 10, p);

                     ((graphWnd)x).table(minX, maxX, stepX);
                    if ((bool)dbgF)   { 
                      Console.Error.WriteLine("data  count {0}", ((graphWnd)x).data.Count );
                    }
                 }
                 else
                     goto finish;

              }




              string info = String.Format(
                "Total Mem/virMem/GC mem: {0}/{1}/{2} kBytes"
                    , (pWSst = proc.PeakWorkingSet64/1024)
                       , (pVMst = proc.PeakVirtualMemorySize64/1024)
                          , (gTMst = GC.GetTotalMemory(false)/1024));   //Retrieves the number of bytes currently thought to be allocated. 


              if (vF)
                Console.Error.WriteLine(info);
                

              
              
              x.ShowDialog();


              finish:
              proc = Process.GetCurrentProcess();

              info = String.Format(
                "Total Mem/virMem/GC mem: {0}/{1}/{2} kBytes"
                    , (proc.PeakWorkingSet64/1024 )
                       , (proc.PeakVirtualMemorySize64/1024 )
                          , (GC.GetTotalMemory(false)/1024));   //Retrieves the number of bytes currently thought to be allocated. 


              if (vF)
              Console.Error.WriteLine(info);

              DateTime fn = DateTime.Now;

              if (vF)
              Console.Error.WriteLine( "time of work with file '{1}' is {0} secs"
                   , (fn - st).TotalSeconds, (string)flNm);

//              Thread.Sleep(1000);
           }

        }


}                	