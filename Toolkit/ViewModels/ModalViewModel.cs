using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoApp.Gui.Toolkit.ViewModels
{
    public abstract class ModalViewModel : ScreenViewModel
    {

        public event Action ModalClosed = delegate {};

        public void FireModalClose()
        {
            ModalClosed();
        }

    }
}
