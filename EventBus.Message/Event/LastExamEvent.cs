﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Message.Event
{
    public class LastExamEvent
    {
        public int Id { get; set; }
        public int ChapterId { get; set; }
        public int? PercentageCompleted { get; set; }
        public string? Name { get; set; }
        public int? Time { get; set; }

    }
}
