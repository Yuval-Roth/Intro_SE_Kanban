﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Frontend.ViewModel
{
    public class Button : Notifier
    {

        private int x;
        private int y;
        private string visibility;
        private string content;

        public Button(int x, int y, string content,string visibility)
        {
            this.x = x;
            this.y = y;
            this.visibility = visibility;
            this.content = content;
        }
        public int X
        {
            get { return x; }
            set
            {
                x = value;
                RaisePropertyChanged("Margin");
            }
        }
        public int Y
        {
            get { return y; }
            set
            {
                y = value;
                RaisePropertyChanged("Margin");
            }
        }
        public string Visibility
        {
            get { return visibility; }
            set
            {
                visibility = value;
                RaisePropertyChanged("Visibility");
            }
        }
        public string Margin => $"{x},{y},0,0";
        public string Content => content;

    }
}
