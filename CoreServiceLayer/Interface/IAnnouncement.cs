using CommonModal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Interface
{
    public interface IAnnouncementService
    {
        string GetAllAnnouncementService(FetchAnnouncement fetchAnnouncement);
    }
}
