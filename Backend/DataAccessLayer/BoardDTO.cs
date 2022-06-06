﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class BoardDTO
    {
        public int Id { set; get; }
        public string Title { set; get; }
        public string Owner { set; get; }
        public string[] Joined { set; get; }
        public TaskDTO[] BackLog { set; get; }
        public TaskDTO[] InProgress { set; get; }
        public TaskDTO[] Done { set; get; }
        public int BackLogLimit { set; get; }
        public int InProgressLimit { set; get; }
        public int DoneLimit { set; get; }
    }
}