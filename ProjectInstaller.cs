using System.ComponentModel;
using System.Configuration.Install;

namespace PGLRefresher
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            this.Installers.AddRange(new Installer[] {
                this.serviceProcessInstaller1
            });
        }
    }
}
