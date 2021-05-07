﻿using BottomhalfCore.BottomhalfModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BottomhalfCore.ContextFactoryManager.Interface
{
    public interface IManageCodeDocuments<T>
    {
        DocCollector GenerateDocumentation(Type CurrentType);
    }
}
