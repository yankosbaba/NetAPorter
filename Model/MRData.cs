﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NetAPorter.Model
{
    public class MRData
    {
        public string xmlns { get; set; }
        public string series { get; set; }
        public string url { get; set; }
        public string limit { get; set; }
        public string offset { get; set; }
        public string total { get; set; }
        public DriverTable DriverTable { get; set; }
        public RaceTable RaceTable { get; set; }
    }
}
