﻿using LiteDb.Config;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleApp
{
    public class DataClassConfiguration : IEntityBuilderConfiguration<DataClass>
    {
        public void Configure(EntityBuilder<DataClass> builder)
        {
            builder.Id(x => x.Key);
        }
    }
}
