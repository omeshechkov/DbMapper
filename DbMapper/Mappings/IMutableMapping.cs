﻿using System.Collections.Generic;

namespace DbMapper.Mappings
{
    public interface IMutableMapping : IMappingClassReference, IHasDiscriminatorValue
    {
        IList<ISubClassMapping> SubClasses { get; }
    }
}