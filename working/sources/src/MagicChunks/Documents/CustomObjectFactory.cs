﻿using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.ObjectFactories;

namespace MagicChunks.Documents
{
    public class CustomObjectFactory : IObjectFactory
    {
        protected readonly IObjectFactory BaseFactory;

        public CustomObjectFactory()
            : this(new DefaultObjectFactory())
        {
        }

        public CustomObjectFactory(IObjectFactory baseFactory)
        {
            if (baseFactory == null)
                throw new ArgumentNullException(nameof(baseFactory));

            BaseFactory = baseFactory;
        }

        public object Create(Type type)
        {
            if (typeof(Dictionary<object, object>).IsAssignableFrom(type))
            {
                return Activator.CreateInstance(type, new IgnoreCaseComparer());
            }

            return BaseFactory.Create(type);
        }
    }
}