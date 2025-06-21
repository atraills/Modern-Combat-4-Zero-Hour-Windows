using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MC4Component;

namespace ModernCombat4
{
    public partial class Windows : Component
    {
        public Windows()
        {
            InitializeComponent();
        }

        public Windows(IContainer container)
        {
            container.Add(this);
            container = IDrawingSurfaceManipulationHandler;
            InitializeComponent();
        }

        public IContainer IDrawingSurfaceManipulationHandler { get; set; }
    }
}
