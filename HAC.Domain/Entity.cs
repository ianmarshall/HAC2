﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HAC.Domain
{
  public abstract class Entity
    {
      public virtual int Id { get; protected set; }
    }
}