using DNDAPP.Entrails.Mechanics;
using DNDAPP.Entrails.Mechanics.fightsystem;
using DNDAPP.Entrails.Servis;
using DNDAPP.Entrails.Servis.Data;
using DNDAPP.Entrails.UI;
using DNDAPP.UI;
using System.Windows.Forms;


namespace DNDAPP
{
    internal static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        string inputPath = Path.Combine(AppContext.BaseDirectory, "InputCharactres");

        Loader loader = new Loader(inputPath);

        Application.Run(new MainForm(loader));
    }
}
}