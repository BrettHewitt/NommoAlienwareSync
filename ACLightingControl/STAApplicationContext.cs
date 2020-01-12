using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ACLightingControl
{
    public class STAApplicationContext : ApplicationContext
    {
        public STAApplicationContext()
        {
            _viewManager = new ViewManager();
        }

        private ViewManager _viewManager;

        // Called from the Dispose method of the base class
        protected override void Dispose(bool disposing)
        {
            _viewManager = null;
        }
    }
}
