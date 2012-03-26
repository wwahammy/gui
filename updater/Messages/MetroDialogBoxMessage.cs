﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoApp.Updater.Controls;

namespace CoApp.Updater.Messages
{
    public class MetroDialogBoxMessage
    {
        public string Title { get; set; }
        public object Content { get; set; }
        public IEnumerable<ButtonDescription> Buttons { get; set; } 
    }
}
