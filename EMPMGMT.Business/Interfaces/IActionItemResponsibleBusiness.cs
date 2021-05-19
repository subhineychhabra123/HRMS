﻿using System;
using EMPMGMT.Domain;
using EMPMGMT.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMPMGMT.Utility;


namespace EMPMGMT.Business.Interfaces
{
    public interface IActionItemResponsibleBusiness
    {
       void SaveActionItemResponsible(List<ActionItemResponsibleModel> listActionItemResponsibleModel);
       List<ActionItemResponsibleModel> GetActionItemResponsibleByActionItem(int ActionItemId);
    }
}
