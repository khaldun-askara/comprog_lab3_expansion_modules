using PluginInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace comprog_lab3_mainapp
{
    public partial class Form1 : Form
    {
        Dictionary<string, IPlugin> plugins = new Dictionary<string, IPlugin>();

        private void OnPluginClick(object sender, EventArgs args)
        {
            IPlugin plugin = plugins[((ToolStripMenuItem)sender).Text];
            var bitmap = (Bitmap)pictureBox.Image;
            plugin.Transform(bitmap);
            pictureBox.Image = bitmap;
        }


        void FindPlugins()
        {
            // папка с плагинами
            string folder = System.AppDomain.CurrentDomain.BaseDirectory;


            string[] files;
            // если конфига нет, выгружаем все длл
            if (!File.Exists(folder + "config.txt"))
                files = Directory.GetFiles(folder, "*.dll");
            else 
            {
                List<string> dll_names = new List<string>();
                using (StreamReader streamReader = new StreamReader(folder + "config.txt"))
                {
                    string line = streamReader.ReadLine();
                    // если написано, что авто, выгружаем все длл
                    if (line == "Auto")
                        files = Directory.GetFiles(folder, "*.dll");
                    else
                    {
                        dll_names.Add(folder + line + ".dll");
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            dll_names.Add(folder + line + ".dll");
                        }
                        files = dll_names.ToArray();
                    }
                }
            }

            


            foreach (string file in files)
                try
                {
                    Assembly assembly = Assembly.LoadFile(file);

                    foreach (Type type in assembly.GetTypes())
                    {
                        Type iface = type.GetInterface("PluginInterface.IPlugin");

                        if (iface != null)
                        {
                            IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                            plugins.Add(plugin.Name, plugin);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки плагина\n" + ex.Message);
                }
        }

        public void CreatePluginsMenu()
        {
            foreach (var plugin in plugins)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(plugin.Key, null, OnPluginClick);
                filtersToolStripMenuItem.DropDownItems.Add(item);
            }
            
        }

        public Form1()
        {
            InitializeComponent();
            FindPlugins();
            CreatePluginsMenu();
        }

        private void filtersInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string info = "";
            foreach (var plugin in plugins)
            {
                var plugin_attributes = plugin.Value.GetType().GetCustomAttributes(typeof(VersionAttribute), false);

                foreach (VersionAttribute attribute in plugin_attributes)
                {
                    info += "Название плагина: " + plugin.Value.Name + "\n";
                    info += "Автор: " + plugin.Value.Author + "\n";
                    info += "Версия: " + attribute.Major + "." + attribute.Minor + "\n\n";
                }
            }
            MessageBox.Show(info, "Filters info");
        }
    }
}
