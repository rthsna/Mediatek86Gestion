using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mediatek86.controleur;
using Mediatek86.vue;
using Serilog;


namespace Mediatek86
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.File("logs/log.txt").CreateLogger();


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Controle controle = new Controle();
            Application.Run(new FrmAuthentification(controle));       
        }
    }
}
